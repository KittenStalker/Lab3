using System;
using System.Dynamic;
using System.IO;
using System.Xml;

namespace Practice2
{
    internal class FileLineParser
    {
        string Path;
        // массив считанных строк
        string[] SourceLines;
        // таблица строковых значений
        string[][] Table;

        public delegate void ExtraProcessor(ref string[][] currentTable);
        ExtraProcessor Processor;
        
        public void SetExtraProcessor(ExtraProcessor processor)
        {
            Processor = processor;
        }
        public FileLineParser(in string path, out int linesCount)
        {
            Path = path;
            linesCount = 0;

            if (!File.Exists(path))
            {
                throw new System.ArgumentException($"Файл по пути {path} не существует");
            }

            SourceLines = File.ReadAllLines(path);
            linesCount = SourceLines.Length;
        }

        public void SpltLines(char splitter = '*')
        {
            Table = new string[SourceLines.Length][];

            var i = 0;
            foreach(string element in SourceLines)
            {
                Table[i] = element.Split(splitter);
                i++;
            }
        }

        public void AddNewLines(params string[] newLines)
        {
            if (newLines.Length == 0) return;
        
            var finalStringArrayLength = newLines.Length + SourceLines.Length;
            var bufferLines = new string [finalStringArrayLength];

            var i = 0;

            foreach(string element in SourceLines)
            {
                bufferLines[i] = element;
                i++;
            }
            
            foreach(string element in newLines)
            {
                bufferLines[i] = element;
                i++;
            }

            SourceLines = bufferLines;
        }

        public void Save()
        {
            if (Processor == null)
            {
                throw new System.ArgumentException($"Параметр Process не определен.");
            }

            File.Copy(Path, Path + ".old", true);
            SpltLines();

            Processor(ref Table);

            var file = File.Create(Path);
            using (System.IO.StreamWriter writer = new StreamWriter(file))
            {
                for (int i = 0; i < Table.Length; i++)
                {
                    for (int j = 0; j < Table[i].Length; j++)
                    {
                        writer.Write(Table[i][j]);
                        writer.Write(";");
                    }
                    writer.Write("\r\n");
                }
            }
            file.Close();
        }

    } 
}