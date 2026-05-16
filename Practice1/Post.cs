using System.Xml.Serialization;

namespace Practice1
{
    internal class JournalPost
    {
        public string Path = null;
        public DateTime? CreatedDateTime = null;
        public int ViewCount= 0;
        public double Rating = 0.0;
        public String Text = null;

        public JournalPost(
            string path,
            DateTime createdDateTime,
            int viewCount,
            double rating,
            String text
            )
        {
            Path = path;
            CreatedDateTime = createdDateTime;
            ViewCount = viewCount;
            Rating = rating;
            Text = text;
        }

        public virtual void Print()
        {
            System.Console.Out.WriteLine(
                $"Путь до поста {Path}. Дата: {CreatedDateTime}.\r\n" +   
                $"Количество просмотров: {ViewCount}. Рейтинг: {Rating:F3}.\r\n" +
                Text
            );
        } 

        public override string ToString()
        {
            return $"Путь до поста {Path}. Дата: {CreatedDateTime}.\r\n" +
                $"Количество просмотров: {ViewCount}. Рейтинг: {Rating:F3}.\r\n" +
                Text;
        }
    }

    internal class ReviewedPost : JournalPost
    {
        public string Review = null;
        public ReviewedPost(
            string path,
            DateTime createdDateTime, 
            int viewCount, 
            double rating, 
            string text,
            string review) : base(path, createdDateTime, viewCount, rating, text)
        {
            Review = review;
        }

        public override void Print()
        {
            base.Print();
            System.Console.Out.WriteLine($"Рецензия: {Review}.");
        }

        public override string ToString()
        {
            return base.ToString() + $"\r\nРецензия: {Review}.";
        }
    }

    internal class ThematicJournalPost : JournalPost
    {
        public enum CategoryType {None, News = 10, Info}
        public CategoryType Category = CategoryType.None;
        public ThematicJournalPost(
            string path,
            DateTime createdDateTime, 
            int viewCount, 
            double rating, 
            string text,
            CategoryType category) : base(path, createdDateTime, viewCount, rating, text)
        {
            Category = category;
        }

        public override void Print()
        {
            base.Print();
            System.Console.Out.WriteLine($"Категория: {Category}.");
        }

        public override string ToString()
        {
            return base.ToString() + $"\r\nКатегория: {Category}.";
        }
    }
}