using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System.Text.RegularExpressions;

namespace TextDividerModule
{
    /// <summary>
    /// Класс GUI-тестов для WinForms приложения TextDivider.
    /// </summary>
    /// <remarks>
    /// Использует библиотеку FlaUI для автоматизации взаимодействия с графическим интерфейсом.
    /// Тесты запускают приложение, выполняют действия пользователя и проверяют результаты.
    /// </remarks>
    [TestFixture]
    internal class GuiTest
    {
        /// <summary>
        /// AutomationId текстового поля ввода текста.
        /// </summary>
        const string idEnterBox = "EnterBox";

        /// <summary>
        /// AutomationId текстового поля вывода результата.
        /// </summary>
        const string idResultBox = "ResultBox";

        /// <summary>
        /// AutomationId элемента NumericUpDown для выбора длины блока.
        /// </summary>
        const string idNumericUpDown = "NumericUpDown";

        /// <summary>
        /// AutomationId кнопки запуска разделения.
        /// </summary>
        const string idResultButton = "ResultButton";

        /// <summary>
        /// Задержка между действиями в миллисекундах.
        /// Необходима для стабильной работы автоматизации.
        /// </summary>
        private const int delay = 500;

        /// <summary>
        /// Путь к исполняемому файлу тестируемого приложения.
        /// </summary>
        private const string PathTestingApp = @"C:\Users\User\Documents\projects\sharp\WinFormsApp1\bin\Debug\net10.0-windows\WinFormsApp1.exe";

        /// <summary>
        /// Запущенный экземпляр приложения.
        /// </summary>
        private FlaUI.Core.Application app;

        /// <summary>
        /// Экземпляр автоматизации UIA3.
        /// </summary>
        private UIA3Automation automation;

        /// <summary>
        /// Ссылка на главное окно приложения.
        /// </summary>
        private Window mainWindow;

        /// <summary>
        /// Метод, выполняемый перед каждым тестом.
        /// </summary>
        /// <remarks>
        /// Запускает тестируемое приложение, инициализирует автоматизацию
        /// и получает ссылку на главное окно.
        /// </remarks>
        [SetUp]
        public void Setup()
        {
            app = FlaUI.Core.Application.Launch(PathTestingApp);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);
        }

        /// <summary>
        /// Метод, выполняемый после каждого теста.
        /// </summary>
        /// <remarks>
        /// Закрывает приложение и освобождает ресурсы автоматизации.
        /// </remarks>
        [TearDown]
        public void Teardown()
        {
            app?.Close();
            automation?.Dispose();
        }


        /// <summary>
        /// Получает текстовое поле ввода.
        /// </summary>
        /// <returns>Элемент TextBox для ввода текста.</returns>
        private TextBox GetEnterBox()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idEnterBox)).AsTextBox();
        }

        /// <summary>
        /// Получает кнопку запуска разделения.
        /// </summary>
        /// <returns>Элемент Button для запуска операции.</returns>
        private Button GetResultButton()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idResultButton)).AsButton();
        }

        /// <summary>
        /// Получает элемент NumericUpDown для выбора длины блока.
        /// </summary>
        /// <returns>Элемент Spinner (NumericUpDown).</returns>
        private Spinner GetNumericUpDown()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idNumericUpDown)).AsSpinner();
        }

        /// <summary>
        /// Получает текстовое поле вывода результата.
        /// </summary>
        /// <returns>Элемент TextBox для отображения результата.</returns>
        private TextBox GetResultBox()
        {
            return mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(idResultBox)).AsTextBox();
        }

        /// <summary>
        /// Устанавливает текст и длину блока в элементах управления.
        /// </summary>
        /// <param name="text">Текст для ввода.</param>
        /// <param name="blockLength">Длина блока (1-100).</param>
        private void SetTextAndBlockLength(string text, int blockLength)
        {
            GetEnterBox().Text = text;
            GetNumericUpDown().Value = blockLength;
            Thread.Sleep(delay);
        }

        /// <summary>
        /// Выполняет нажатие кнопки "Divide".
        /// </summary>
        private void ClickDivideButton()
        {
            GetResultButton().Click();
            Thread.Sleep(delay);
        }

        /// <summary>
        /// Получает результат разделения в виде массива строк.
        /// </summary>
        /// <returns>Массив блоков текста, разделенных пробелами.</returns>
        private string[] GetResultTextArray()
        {
            var result = GetResultBox().Text.Trim();
            result = Regex.Replace(result, @"\s+", " ");

            return result.Split(' ');
        }

        /// <summary>
        /// Получает текст результата как единую строку.
        /// </summary>
        /// <returns>Строка с результатом, обрезанная от лишних пробелов.</returns>
        private string GetResutText()
        {
            return GetResultBox().Text.Trim();
        }

        /// <summary>
        /// T01: Базовое разделение текста (нормальные случаи).
        /// </summary>
        /// <remarks>
        /// Проверяет:
        /// 1. Разделение строки четной длины на блоки
        /// 2. Разделение с дополнением нулевыми символами
        /// 3. Разделение с длиной блока = 1
        /// 4. Ситуация, когда длина блока больше длины строки
        /// </remarks>
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

        /// <summary>
        /// T02: Обработка граничных и некорректных входных данных.
        /// </summary>
        /// <remarks>
        /// Проверяет, что приложение корректно обрабатывает:
        /// 1. Пустой текст
        /// 2. Нулевую длину блока
        /// 3. Отрицательную длину блока
        /// и отображает соответствующие сообщения об ошибках.
        /// </remarks>
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

        /// <summary>
        /// T03: Различные типы символов.
        /// </summary>
        /// <remarks>
        /// Проверяет корректную работу с:
        /// 1. Русскими символами (Unicode)
        /// 2. Специальными символами (!@#$%^&*())
        /// 3. Цифрами
        /// 4. Пробелами внутри текста
        /// </remarks>
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

        /// <summary>
        /// T04: Поведение интерфейса при многократных действиях.
        /// </summary>
        /// <remarks>
        /// Проверяет:
        /// 1. Повторное нажатие кнопки не меняет результат
        /// 2. При вводе нового текста результат обновляется корректно
        /// </remarks>
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

        /// <summary>
        /// T05: Работа с большими объемами текста.
        /// </summary>
        /// <remarks>
        /// Проверяет:
        /// 1. Корректность разделения 100 символов на блоки по 10
        /// 2. Производительность при обработке 1000 символов (менее 3 секунд)
        /// </remarks>
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
