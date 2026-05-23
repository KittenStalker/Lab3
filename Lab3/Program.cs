
namespace TextDividerModule
{
    public interface IFileReader
    {
        string ReadAllText(string path);
    }

    public class RealFileReader : IFileReader
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }

    public class TextDivider
    {
        private readonly IFileReader _fileReader;

        public TextDivider(IFileReader fileReader)
        {
            _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        }

        public List<string> Divider(string str, int blockLength)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Строка не может быть null.");

            if (blockLength <= 0)
                throw new ArgumentException("Длина блока должна быть больше нуля.", nameof(blockLength));

            List<string> blocks = new List<string>(str.Length / blockLength + 1);

            for (int i = 0; i < str.Length; i += blockLength)
            {
                if (str.Length - i > blockLength)
                    blocks.Add(str.Substring(i, blockLength));
                else
                    blocks.Add(str.Substring(i, str.Length - i)
                               + new string('\0', blockLength - (str.Length - i)));
            }
            return blocks;
        }

        public List<string> ProcessFile(string filePath, int blockLength)
        {
            string content = _fileReader.ReadAllText(filePath);
            return Divider(content, blockLength);
        }
    }
}