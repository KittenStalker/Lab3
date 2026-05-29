
namespace TextDividerModule
{
    /// <summary>
    /// Тестовый двойник для интерфейса IFileReader.
    /// </summary>
    /// <remarks>
    /// Используется для модульного тестирования без доступа к реальной файловой системе.
    /// Позволяет имитировать различные сценарии: успешное чтение файла или ошибку "файл не найден".
    /// </remarks>
    public class StubFileReader : IFileReader
    {
        /// <summary>
        /// Содержимое, которое будет возвращено при чтении файла.
        /// </summary>
        private readonly string _contentToReturn;

        /// <summary>
        /// Флаг, указывающий, нужно ли имитировать ошибку "файл не найден".
        /// </summary>
        private readonly bool _simulateFileNotFound;

        /// <summary>
        /// Инициализирует новый экземпляр заглушки FileReader.
        /// </summary>
        /// <param name="contentToReturn">Текст, который будет возвращен при чтении файла.</param>
        /// <param name="simulateFileNotFound">Если true - при вызове ReadAllText будет 
        /// выброшено исключение FileNotFoundException.
        /// </param>
        public StubFileReader(string contentToReturn, bool simulateFileNotFound = false)
        {
            _contentToReturn = contentToReturn;
            _simulateFileNotFound = simulateFileNotFound;
        }

        /// <summary>
        /// Имитирует чтение файла.
        /// </summary>
        /// <param name="path">Путь к файлу (не используется в заглушке).</param>
        /// <returns>
        /// Возвращает заранее заданное содержимое _contentToReturn,
        /// если _simulateFileNotFound = false.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Выбрасывается, если _simulateFileNotFound = true.
        /// </exception>
        public string ReadAllText(string path)
        {
            if (_simulateFileNotFound)
                throw new FileNotFoundException($"Файл не найден: {path}");

            return _contentToReturn;
        }
    }

    /// <summary>
    /// Класс модульных тестов для TextDivider.
    /// </summary>
    /// <remarks>
    /// Содержит тесты для методов Divider и ProcessFile.
    /// Проверяет корректность разделения текста и обработку ошибок.
    /// </remarks>
    [TestFixture]
    public class TextDividerTests
    {
        /// <summary>
        /// Экземпляр тестируемого класса TextDivider.
        /// </summary>
        private TextDivider _textDivider;

        /// <summary>
        /// Метод инициализации, выполняемый перед каждым тестом.
        /// </summary>
        /// <remarks>
        /// Создает новый экземпляр TextDivider с заглушкой FileReader.
        /// </remarks>
        [SetUp]
        public void SetUp()
        {
            // Инициализация с тестовым двойником по умолчанию
            var stubReader = new StubFileReader("");
            _textDivider = new TextDivider(stubReader);
        }

        /// <summary>
        /// T01: Тест нормального разделения текста.
        /// </summary>
        /// <remarks>
        /// Вход: "ABCDEFGH", длина блока = 4
        /// Ожидаемый результат: ["ABCD", "EFGH"]
        /// </remarks>
        [Test]
        public void T01_Divider_NormalCase_ReturnsCorrectBlocks()
        {
            var result = _textDivider.Divider("ABCDEFGH", 4);

            Assert.That(result[0], Is.EqualTo("ABCD"));
            Assert.That(result[1], Is.EqualTo("EFGH"));
        }

        /// <summary>
        /// T02: Тест дополнения последнего блока нулевыми символами.
        /// </summary>
        /// <remarks>
        /// Вход: "ABCDE", длина блока = 3
        /// Ожидаемый результат: ["ABC", "DE\0"]
        /// Последний блок дополняется нулевыми символами до длины 3.
        /// </remarks>
        [Test]
        public void T02_Divider_LastBlockPaddedWithNulls_ReturnsPaddedBlock()
        {
            var result = _textDivider.Divider("ABCDE", 3);

            Assert.That(result[0], Is.EqualTo("ABC"));
            Assert.That(result[1], Is.EqualTo("DE\0"));
        }

        /// <summary>
        /// T03: Тест обработки пустой строки.
        /// </summary>
        /// <remarks>
        /// Вход: "", длина блока = 5
        /// Ожидаемый результат: пустой список
        /// </remarks>
        [Test]
        public void T03_Divider_EmptyString_ReturnsEmptyList()
        {
            var result = _textDivider.Divider("", 5);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// T04: Тест, когда длина блока больше длины строки.
        /// </summary>
        /// <remarks>
        /// Вход: "Hi", длина блока = 5
        /// Ожидаемый результат: ["Hi\0\0\0"] - один блок, дополненный нулями.
        /// </remarks>
        [Test]
        public void T04_Divider_BlockLengthGreaterThanString_ReturnsSinglePaddedBlock()
        {
            var result = _textDivider.Divider("Hi", 5);

            Assert.That(result[0], Is.EqualTo("Hi\0\0\0"));
        }

        /// <summary>
        /// T05: Тест с длиной блока = 1.
        /// </summary>
        /// <remarks>
        /// Вход: "ABC", длина блока = 1
        /// Ожидаемый результат: ["A", "B", "C"] - каждый символ в отдельном блоке.
        /// </remarks>
        [Test]
        public void T05_Divider_BlockLengthOne_ReturnsCharsAsBlocks()
        {
            var result = _textDivider.Divider("ABC", 1);

            Assert.That(result[0], Is.EqualTo("A"));
            Assert.That(result[1], Is.EqualTo("B"));
            Assert.That(result[2], Is.EqualTo("C"));
        }

        /// <summary>
        /// T06: Тест передачи null в качестве строки.
        /// </summary>
        /// <remarks>
        /// Вход: null, длина блока = 5
        /// Ожидаемое исключение: ArgumentNullException с параметром "str".
        /// </remarks>
        [Test]
        public void T06_Divider_NullString_ThrowsArgumentNullException()
        {  
            var ex = Assert.Throws<ArgumentNullException>(() => _textDivider.Divider(null, 5));

            Assert.That(ex.ParamName, Is.EqualTo("str"));
        }

        /// <summary>
        /// T07: Тест с нулевой длиной блока.
        /// </summary>
        /// <remarks>
        /// Вход: "test", длина блока = 0
        /// Ожидаемое исключение: ArgumentException с параметром "blockLength".
        /// </remarks>
        [Test]
        public void T07_Divider_ZeroBlockLength_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _textDivider.Divider("test", 0));

            Assert.That(ex.ParamName, Is.EqualTo("blockLength"));
        }

        /// <summary>
        /// T08: Тест с отрицательной длиной блока.
        /// </summary>
        /// <remarks>
        /// Вход: "test", длина блока = -3
        /// Ожидаемое исключение: ArgumentException с параметром "blockLength".
        /// </remarks>
        [Test]
        public void T08_Divider_NegativeBlockLength_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _textDivider.Divider("test", -3));

            Assert.That(ex.ParamName, Is.EqualTo("blockLength"));
        }

        /// <summary>
        /// T11: Тест обработки существующего файла.
        /// </summary>
        /// <remarks>
        /// Вход: фиктивный файл "fake.txt" с содержимым "HelloWorld", длина блока = 5
        /// Ожидаемый результат: ["Hello", "World"]
        /// Проверяет, что метод ProcessFile корректно читает файл и разделяет содержимое.
        /// </remarks>
        [Test]
        public void T11_ProcessFile_FileExists_CallsFileReaderAndReturnsBlocks()
        {
            string fakePath = "fake.txt";
            string fakeContent = "HelloWorld";
            var stubReader = new StubFileReader(fakeContent);
            _textDivider = new TextDivider(stubReader);

            var result = _textDivider.ProcessFile(fakePath, 5);

            Assert.That(result[0], Is.EqualTo("Hello"));
            Assert.That(result[1], Is.EqualTo("World"));
        }

        /// <summary>
        /// T12: Тест обработки отсутствующего файла.
        /// </summary>
        /// <remarks>
        /// Вход: путь к несуществующему файлу "missing.txt"
        /// Ожидаемое исключение: FileNotFoundException
        /// Проверяет корректную обработку ошибки отсутствия файла.
        /// </remarks>
        [Test]
        public void T12_ProcessFile_FileNotFound_ThrowsFileNotFoundException()
        {
            string missingPath = "missing.txt";
            var stubReader = new StubFileReader("", simulateFileNotFound: true);
            _textDivider = new TextDivider(stubReader);

            Assert.Throws<FileNotFoundException>(() => _textDivider.ProcessFile(missingPath, 10));
        }

        /// <summary>
        /// T13: Тест обработки пустого файла.
        /// </summary>
        /// <remarks>
        /// Вход: фиктивный пустой файл "empty.txt", длина блока = 3
        /// Ожидаемый результат: пустой список
        /// </remarks>
        [Test]
        public void T13_ProcessFile_WithEmptyFile_ReturnsEmptyList()
        {
            string fakePath = "empty.txt";
            string emptyContent = "";
            var stubReader = new StubFileReader(emptyContent);
            _textDivider = new TextDivider(stubReader);

            var result = _textDivider.ProcessFile(fakePath, 3);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// T14: Тест обработки длинного текста.
        /// </summary>
        /// <remarks>
        /// Вход: длинная строка "This is a long text for testing", длина блока = 10
        /// Ожидаемый результат: список блоков, первый блок имеет длину 10 символов
        /// Проверяет производительность и корректность на длинных строках.
        /// </remarks>
        [Test]
        public void T14_ProcessFile_WithLongText_PerformsCorrectDivision()
        {
            string fakePath = "long.txt";
            string longContent = "This is a long text for testing";
            var stubReader = new StubFileReader(longContent);
            _textDivider = new TextDivider(stubReader);

            var result = _textDivider.ProcessFile(fakePath, 10);

            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result[0].Length, Is.EqualTo(10));
        }

        /// <summary>
        /// T15: Тест обработки русского текста.
        /// </summary>
        /// <remarks>
        /// Вход: русская строка "ПриветМир", длина блока = 5
        /// Ожидаемый результат: ["Приве", "тМир\0"]
        /// Проверяет корректную работу с Unicode/многобайтовыми символами.
        /// </remarks>
        [Test]
        public void T15_ProcessFile_WithRussianText_CorrectlyDivides()
        {
            string fakePath = "russian.txt";
            string russianContent = "ПриветМир";
            var stubReader = new StubFileReader(russianContent);
            _textDivider = new TextDivider(stubReader);

            var result = _textDivider.ProcessFile(fakePath, 5);

            Assert.That(result[0], Is.EqualTo("Приве"));
            Assert.That(result[1], Is.EqualTo("тМир\0"));
        }

        /// <summary>
        /// T16: Тест обработки специальных символов.
        /// </summary>
        /// <remarks>
        /// Вход: строка спецсимволов "!@#$%^&*()", длина блока = 3
        /// Ожидаемый результат: ["!@#", "$%^", "&*(", ")\0\0"]
        /// Проверяет корректную работу с символами, не являющимися буквами/цифрами.
        /// </remarks>
        [Test]
        public void T16_ProcessFile_WithSpecialCharacters_CorrectlyDivides()
        {
            string fakePath = "special.txt";
            string specialContent = "!@#$%^&*()";
            var stubReader = new StubFileReader(specialContent);
            _textDivider = new TextDivider(stubReader);

            var result = _textDivider.ProcessFile(fakePath, 3);

            Assert.That(result[0], Is.EqualTo("!@#"));
            Assert.That(result[1], Is.EqualTo("$%^"));
            Assert.That(result[2], Is.EqualTo("&*("));
            Assert.That(result[3], Is.EqualTo(")\0\0"));
        }
    }
}