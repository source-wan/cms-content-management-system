namespace CMS.App.ReqDto
{
    public class ArticleDto
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Remarks { get; set; } = null!;

        public Guid CategoryId { get; set; }
    }
}