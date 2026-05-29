
namespace TextDividerModule
{
    /// <summary>
    /// Интерфейс для чтения файлов.
    /// </summary>
    public interface IFileReader
    {
        /// <summary>
        /// Читает все содержимое файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Содержимое файла в виде строки.</returns>
        string ReadAllText(string path);
    }

    /// <summary>
    /// Реальная реализация IFileReader для работы с файловой системой.
    /// </summary>
    public class RealFileReader : IFileReader
    {
        /// <summary>
        /// Читает содержимое файла с диска.
        /// </summary>
        /// <param name="path">Полный путь к файлу.</param>
        /// <returns>Содержимое файла.</returns>
        /// <exception cref="FileNotFoundException">Файл не найден.</exception>
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }

    /// <summary>
    /// Основной класс для разделения текста на блоки заданной длины.
    /// </summary>
    /// <remarks>
    /// Класс реализует логику разбиения строки на блоки указанной длины.
    /// Последний блок дополняется нулевыми символами ('\0') при необходимости.
    /// </remarks>
    public class TextDivider
    {
        /// <summary>
        /// Объект для чтения файлов для обработки ввода.
        /// </summary>
        private readonly IFileReader _fileReader;

        /// <summary>
        /// Конструктр класса TextDivider.
        /// </summary>
        /// <param name="fileReader">Реализация интерфейса IFileReader.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если fileReader = null.</exception>
        public TextDivider(IFileReader fileReader)
        {
            _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        }

        /// <summary>
        /// Разделяет строку на блоки заданной длины.
        /// </summary>
        /// <param name="str">Входная строка для разделения.</param>
        /// <param name="blockLength">Длина каждого блока (должна быть > 0).</param>
        /// <returns>Список строк-блоков. Последний блок дополнен '\0'.</returns>
        /// <exception cref="ArgumentNullException">str = null.</exception>
        /// <exception cref="ArgumentException">blockLength ≤ 0.</exception>
        public List<string> Divider(string str, int blockLength)
        {
            // Проверка на null
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Строка не может быть null.");

            // Проверка длины блока
            if (blockLength <= 0)
                throw new ArgumentException("Длина блока должна быть больше нуля.", nameof(blockLength));

            // Предварительное выделение памяти для списка
            List<string> blocks = new List<string>(str.Length / blockLength + 1);

            // Основной цикл разделения
            for (int i = 0; i < str.Length; i += blockLength)
            {
                if (str.Length - i > blockLength)
                    // Полный блок - просто подстрока
                    blocks.Add(str.Substring(i, blockLength));
                else
                    // Последний блок - дополнение нулевыми символами
                    blocks.Add(str.Substring(i, str.Length - i)
                               + new string('\0', blockLength - (str.Length - i)));
            }
            return blocks;
        }

        /// <summary>
        /// Обрабатывает файл, читает его содержимое и разделяет на блоки.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="blockLength">Длина каждого блока.</param>
        /// <returns>Список строк-блоков из содержимого файла.</returns>
        /// <exception cref="FileNotFoundException">Файл не найден.</exception>
        /// <exception cref="ArgumentNullException">filePath = null.</exception>
        public List<string> ProcessFile(string filePath, int blockLength)
        {
            string content = _fileReader.ReadAllText(filePath);
            return Divider(content, blockLength);
        }
    }
}