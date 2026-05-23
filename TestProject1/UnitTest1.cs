
namespace TextDividerModule
{
    public class StubFileReader : IFileReader
    {
        private readonly string _contentToReturn;
        private readonly bool _simulateFileNotFound;

        public StubFileReader(string contentToReturn, bool simulateFileNotFound = false)
        {
            _contentToReturn = contentToReturn;
            _simulateFileNotFound = simulateFileNotFound;
        }

        public string ReadAllText(string path)
        {
            if (_simulateFileNotFound)
                throw new FileNotFoundException($"Файл не найден: {path}");

            return _contentToReturn;
        }
    }

    [TestFixture]
    public class TextDividerTests
    {
        private TextDivider _textDivider;

        [SetUp]
        public void SetUp()
        {
            // Инициализация с тестовым двойником по умолчанию
            var stubReader = new StubFileReader("");
            _textDivider = new TextDivider(stubReader);
        }

        // ========== ТЕСТЫ МЕТОДА DIVIDER ==========

        [Test]
        public void T01_Divider_NormalCase_ReturnsCorrectBlocks()
        {
            var result = _textDivider.Divider("ABCDEFGH", 4);

            Assert.That(result[0], Is.EqualTo("ABCD"));
            Assert.That(result[1], Is.EqualTo("EFGH"));
        }

        [Test]
        public void T02_Divider_LastBlockPaddedWithNulls_ReturnsPaddedBlock()
        {
            var result = _textDivider.Divider("ABCDE", 3);

            Assert.That(result[0], Is.EqualTo("ABC"));
            Assert.That(result[1], Is.EqualTo("DE\0"));
        }

        [Test]
        public void T03_Divider_EmptyString_ReturnsEmptyList()
        {
            var result = _textDivider.Divider("", 5);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void T04_Divider_BlockLengthGreaterThanString_ReturnsSinglePaddedBlock()
        {
            var result = _textDivider.Divider("Hi", 5);

            Assert.That(result[0], Is.EqualTo("Hi\0\0\0"));
        }

        [Test]
        public void T05_Divider_BlockLengthOne_ReturnsCharsAsBlocks()
        {
            var result = _textDivider.Divider("ABC", 1);

            Assert.That(result[0], Is.EqualTo("A"));
            Assert.That(result[1], Is.EqualTo("B"));
            Assert.That(result[2], Is.EqualTo("C"));
        }

        [Test]
        public void T06_Divider_NullString_ThrowsArgumentNullException()
        {  
            var ex = Assert.Throws<ArgumentNullException>(() => _textDivider.Divider(null, 5));

            Assert.That(ex.ParamName, Is.EqualTo("str"));
        }

        [Test]
        public void T07_Divider_ZeroBlockLength_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _textDivider.Divider("test", 0));

            Assert.That(ex.ParamName, Is.EqualTo("blockLength"));
        }

        [Test]
        public void T08_Divider_NegativeBlockLength_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _textDivider.Divider("test", -3));

            Assert.That(ex.ParamName, Is.EqualTo("blockLength"));
        }

        // ========== ТЕСТЫ МЕТОДА PROCESSFILE ==========

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

        [Test]
        public void T12_ProcessFile_FileNotFound_ThrowsFileNotFoundException()
        {
            string missingPath = "missing.txt";
            var stubReader = new StubFileReader("", simulateFileNotFound: true);
            _textDivider = new TextDivider(stubReader);

            Assert.Throws<FileNotFoundException>(() => _textDivider.ProcessFile(missingPath, 10));
        }

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