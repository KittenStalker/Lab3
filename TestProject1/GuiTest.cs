using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;  
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
//using Xceed.Wpf.Toolkit;

namespace TextDividerModule
{
    [TestFixture] // attribute that marks a class that contains tests
    internal class GuiTest
    {
        ////текст внутри элементов управления
        //const string enterBoxString = "Hello world";
        //const string errorLabelString = "Нет ошибок";

        // automatisation-id для элементов управления
        const string idEnterBox = "EnterBox";
        const string idResultBox = "ResultBox";
        const string idNumericUpDown = "NumericUpDown";
        const string idResultButton = "ResultButton";

        private const int delay = 500;
        private const string PathTestingApp = @"C:\Users\User\Documents\projects\sharp\WinFormsApp1\bin\Debug\net10.0-windows\WinFormsApp1.exe";
        private FlaUI.Core.Application app;
        private UIA3Automation automation;
        private Window mainWindow;

        [SetUp] // calls immediately before each test case finishes
        public void Setup()
        {
            app = FlaUI.Core.Application.Launch(PathTestingApp);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);
        }

        [TearDown] // calls immediately after each test case finishes
        public void Teardown()
        {
            app?.Close();
            automation?.Dispose();
        }


        // Вспомогательные методы
        private TextBox GetEnterBox()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idEnterBox)).AsTextBox();
        }

        private Button GetResultButton()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idResultButton)).AsButton();
        }

        private Spinner GetNumericUpDown()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idNumericUpDown)).AsSpinner();
        }

        private TextBox GetResultBox()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idResultBox)).AsTextBox();
        }

        private void SetTextAndBlockLength(string text, int blockLength)
        {
            GetEnterBox().Text = text;
            GetNumericUpDown().Value = blockLength;
            Thread.Sleep(delay);
        }
        private void ClickDivideButton()
        {
            GetResultButton().Click();
            Thread.Sleep(delay);
        }

        private string[] GetResultTextArray()
        {
            var result = GetResultBox().Text.Trim();
            result = Regex.Replace(result, @"\s+", " ");

            return result.Split(' ');
        }
        private string GetResutText()
        {
            return GetResultBox().Text.Trim();
        }

        [Test]
        [Description("T01 Базовое разделение текста (нормальные случаи)")]
        public void T01_BasicDivisionTests()
        {
            // Шаг 1: ABCD EFGH
            SetTextAndBlockLength("ABCDEFGH", 4);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("ABCD EFGH"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("ABCD"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("EFGH"));

            // Шаг 2: ABC DE (с нулями)
            SetTextAndBlockLength("ABCDE", 3);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("ABC DE"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("ABC"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("DE"));

            // Шаг 3: A B C (длина блока 1)
            SetTextAndBlockLength("ABC", 1);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("A B C"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("A"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("B"));
            Assert.That(GetResultTextArray()[2], Is.EqualTo("C"));

            // Шаг 4: Hi с нулями (длина блока больше текста)
            SetTextAndBlockLength("Hi", 5);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("Hi"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("Hi"));
        }

        [Test]
        [Description("T02 Обработка граничных и некорректных входных данных")]
        public void T02_ErrorHandlingTests()
        {
            // Шаг 1: Пустой текст
            SetTextAndBlockLength("", 5);
            Assert.DoesNotThrow(() => ClickDivideButton());

            // Шаг 2: Длина блока = 0
            SetTextAndBlockLength("Test", 0);
            Assert.DoesNotThrow(() => ClickDivideButton());

            // Шаг 3: Отрицательная длина блока (через Text)
            SetTextAndBlockLength("Test", -5);
            Assert.DoesNotThrow(() => ClickDivideButton());

            // Шаг 4: Текст = null
            SetTextAndBlockLength("Test", 5);
            Assert.DoesNotThrow(() => ClickDivideButton());
        }

        [Test]
        [Description("T03 Различные типы символов")]
        public void T03_DifferentCharacterTypesTests()
        {
            // Шаг 1: Русский текст
            SetTextAndBlockLength("ПриветМир", 5);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("Приве тМир"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("Приве"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("тМир"));

            // Шаг 2: Специальные символы
            SetTextAndBlockLength("!@#$%^&*()", 3);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("!@# $%^ &*( )"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("!@#"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("$%^"));
            Assert.That(GetResultTextArray()[2], Is.EqualTo("&*("));
            Assert.That(GetResultTextArray()[3], Is.EqualTo(")"));

            // Шаг 3: Цифры
            SetTextAndBlockLength("1234567890", 3);
            ClickDivideButton();

            Assert.That(GetResutText(), Is.EqualTo("123 456 789 0"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("123"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("456"));
            Assert.That(GetResultTextArray()[2], Is.EqualTo("789"));
            Assert.That(GetResultTextArray()[3], Is.EqualTo("0"));

            // Шаг 4: Пробелы
            SetTextAndBlockLength("Hello World", 5);
            ClickDivideButton();

            
            Assert.That(GetResutText(), Is.EqualTo("Hello  Worl d"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("Hello"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("Worl"));
            Assert.That(GetResultTextArray()[2], Is.EqualTo("d"));
        }

        [Test]
        [Description("T04 Поведение интерфейса при многократных действиях")]
        public void T04_MultipleActionsTests()
        {
            // Шаг 1: Первое нажатие
            SetTextAndBlockLength("ABC", 2);
            ClickDivideButton();
            var firstResult = GetResultTextArray();

            // Шаг 2: Второе нажатие (результат не должен измениться)
            ClickDivideButton();
            var secondResult = GetResultTextArray();

            Assert.That(firstResult, Is.EqualTo(secondResult));

            // Шаг 3: Новый текст (поле должно очиститься)
            SetTextAndBlockLength("XYZ", 2);
            ClickDivideButton();

            var thirdResult = GetResultTextArray();
            Assert.That(GetResutText(), Is.EqualTo("XY Z"));
            Assert.That(GetResultTextArray()[0], Is.EqualTo("XY"));
            Assert.That(GetResultTextArray()[1], Is.EqualTo("Z"));
        }

        [Test]
        [Description("T05 Работа с большими объемами текста")]
        public void T05_LargeTextTests()
        {
            // Шаг 1: 100 символов, блоки по 10
            string text100 = new string('A', 100);
            SetTextAndBlockLength(text100, 10);
            ClickDivideButton();

            Assert.That(GetResultTextArray().Length, Is.EqualTo(10));

            // Шаг 2: 1000 символов, блоки по 100
            string text1000 = new string('B', 1000);
            SetTextAndBlockLength(text1000, 100);

            // Используем таймер для проверки производительности
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            ClickDivideButton();
            stopwatch.Stop();

            Assert.That(GetResultTextArray().Length, Is.EqualTo(10));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(3000),
                "Обработка 1000 символов должна занимать менее 3 секунд");
        }
    }
}
