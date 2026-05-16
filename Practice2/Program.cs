using Practice2;

try
{
    int linesCount = 0;
    FileLineParser parser = new FileLineParser(@"C:\Users\User\Documents\projects\sharp\Practice2\example.txt", out linesCount);

    parser.AddNewLines("Новая строка * для нас");
    parser.AddNewLines("Еще строка * для нас");
    parser.AddNewLines();
    string[] newLines = {"Еще пару * линий", "Еще одна * линия"};
    parser.AddNewLines(newLines);

    parser.SpltLines();

    parser.SetExtraProcessor(
        (ref string[][] currentTable) =>
        {
            for (int i = 0; i < currentTable.Length; i++)
            {
                for (int j = 0; j < currentTable[i].Length; j++)
                {
                    currentTable[i][j] = currentTable[i][j].Trim().ToLower();
                }
            }
        }
    );

    parser.Save();
    Console.WriteLine("Done");
}
catch (Exception exp)
{
    Console.WriteLine(exp.Message);
}
