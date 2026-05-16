namespace Practice1
{
    internal class Program
    {
        static void Main()
        {
            JournalPost journalPost = new JournalPost(
                @"\path\example",
                DateTime.Now, 
                10, 
                0.1234, 
                "Релиз альбома Sanctuary от Evanescence состоится 5 июня, и группа выпустила сингл «Who Will You Follow» в честь анонса.");

            ThematicJournalPost newsPost = new ThematicJournalPost(
                @"\path\example", 
                DateTime.Now, 
                10, 
                0.1234, 
                "Релиз альбома Sanctuary от Evanescence состоится 5 июня, и группа выпустила сингл «Who Will You Follow» в честь анонса.",
                ThematicJournalPost.CategoryType.News);    

            ReviewedPost reviewedPost = new ReviewedPost(
                @"\path\example", 
                DateTime.Now, 
                10, 
                0.1234, 
                "Релиз альбома Sanctuary от Evanescence состоится 5 июня, и группа выпустила сингл «Who Will You Follow» в честь анонса.",
                "Хороший пост");

            JournalPost baseRef = (JournalPost)newsPost;
            if (newsPost is JournalPost) // is для проверки приведения
            {
                baseRef = (JournalPost)newsPost;
            }

            
            
            System.Console.WriteLine($"\r\nВывод объекта {nameof(journalPost)} типа {journalPost.GetType().Name}");
            System.Console.WriteLine(journalPost);

            System.Console.WriteLine($"\r\nВывод объекта {nameof(newsPost)} типа {newsPost.GetType().Name}");
            System.Console.WriteLine(newsPost);

            System.Console.WriteLine($"\r\nВывод объекта {nameof(reviewedPost)} типа {reviewedPost.GetType().Name}");
            System.Console.WriteLine(reviewedPost);
            
            System.Console.Read();
        }
    }
}