using NUnit.Framework;
using System;
using System.IO;

namespace TextDividerModule
{
    // Ручной тестовый двойник (Stub) для IFileReader
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
        public void Divider_NormalCase_ReturnsCorrectBlocks()
        {
            // Act
            var result = _textDivider.Divider("ABCDEFGH", 4);

            // Assert
            Assert.That(result, Is.EqualTo(new[] { "ABCD", "EFGH" }));
        }

        [Test]
        public void Divider_LastBlockPaddedWithNulls_ReturnsPaddedBlock()
        {
            // Act
            var result = _textDivider.Divider("ABCDE", 3);

            // Assert
            Assert.That(result[0], Is.EqualTo("ABC"));
            Assert.That(result[1], Is.EqualTo("DE\0"));
        }

        [Test]
        public void Divider_EmptyString_ReturnsEmptyList()
        {
            // Act
            var result = _textDivider.Divider("", 5);

            // Assert - исправлено: используем Is.Empty
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Divider_BlockLengthGreaterThanString_ReturnsSinglePaddedBlock()
        {
            // Act
            var result = _textDivider.Divider("Hi", 5);

            // Assert
            Assert.That(result[0], Is.EqualTo("Hi\0\0\0"));
        }

        [Test]
        public void Divider_BlockLengthOne_ReturnsCharsAsBlocks()
        {
            // Act
            var result = _textDivider.Divider("ABC", 1);

            // Assert
            Assert.That(result, Is.EqualTo(new[] { "A", "B", "C" }));
        }

        [Test]
        public void Divider_NullString_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _textDivider.Divider(null, 5));
            Assert.That(ex.ParamName, Is.EqualTo("str"));
        }

        [Test]
        public void Divider_ZeroBlockLength_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _textDivider.Divider("test", 0));
            Assert.That(ex.ParamName, Is.EqualTo("blockLength"));
        }

        [Test]
        public void Divider_NegativeBlockLength_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _textDivider.Divider("test", -3));
            Assert.That(ex.ParamName, Is.EqualTo("blockLength"));
        }

        // ========== ТЕСТЫ МЕТОДА PROCESSFILE ==========

        [Test]
        public void ProcessFile_FileExists_CallsFileReaderAndReturnsBlocks()
        {
            // Arrange
            string fakePath = "fake.txt";
            string fakeContent = "HelloWorld";
            var stubReader = new StubFileReader(fakeContent);
            _textDivider = new TextDivider(stubReader);

            // Act
            var result = _textDivider.ProcessFile(fakePath, 5);

            // Assert
            Assert.That(result, Is.EqualTo(new[] { "Hello", "World" }));
        }

        [Test]
        public void ProcessFile_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            string missingPath = "missing.txt";
            var stubReader = new StubFileReader("", simulateFileNotFound: true);
            _textDivider = new TextDivider(stubReader);

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _textDivider.ProcessFile(missingPath, 10));
        }

        [Test]
        public void ProcessFile_WithEmptyFile_ReturnsEmptyList()
        {
            // Arrange
            string fakePath = "empty.txt";
            string emptyContent = "";
            var stubReader = new StubFileReader(emptyContent);
            _textDivider = new TextDivider(stubReader);

            // Act
            var result = _textDivider.ProcessFile(fakePath, 3);

            // Assert - исправлено: используем Is.Empty
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ProcessFile_WithLongText_PerformsCorrectDivision()
        {
            // Arrange
            string fakePath = "long.txt";
            string longContent = "This is a long text for testing";
            var stubReader = new StubFileReader(longContent);
            _textDivider = new TextDivider(stubReader);

            // Act
            var result = _textDivider.ProcessFile(fakePath, 10);

            // Assert
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result[0].Length, Is.EqualTo(10));
        }

        [Test]
        public void ProcessFile_WithRussianText_CorrectlyDivides()
        {
            // Arr ange
            string fakePath = "russian.txt";
            string russianContent = "ПриветМир";
            var stubReader = new StubFileReader(russianContent);
            _textDivider = new TextDivider(stubReader);

            // Act
            var result = _textDivider.ProcessFile(fakePath, 5);

            // Assert
            Assert.That(result[0], Is.EqualTo("Приве"));
            Assert.That(result[1], Is.EqualTo("тМир\0"));
        }

        [Test]
        public void ProcessFile_WithSpecialCharacters_CorrectlyDivides()
        {
            // Arrange
            string fakePath = "special.txt";
            string specialContent = "!@#$%^&*()";
            var stubReader = new StubFileReader(specialContent);
            _textDivider = new TextDivider(stubReader);

            // Act
            var result = _textDivider.ProcessFile(fakePath, 3);

            // Assert
            Assert.That(result[0], Is.EqualTo("!@#"));
            Assert.That(result[1], Is.EqualTo("$%^"));
            Assert.That(result[2], Is.EqualTo("&*("));
            Assert.That(result[3], Is.EqualTo(")\0\0"));
        }
    }
}