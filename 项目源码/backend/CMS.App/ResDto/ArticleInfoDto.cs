namespace CMS.App.ResDto;
public class ArticleInfoDto
{
    public Guid Id { get; set; }
    public Guid? AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;
    public string? AuthorNickName { get; set; } = null!;
    public Guid? AuthorAvatar { get; set; }
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int Visible { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int UnlikeCount { get; set; } = 0;
    public int CollectionCount { get; set; } = 0;
    public string? Remark { get; set; }
    public DateTime PublishAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    public Guid? UpdatedBy { get; set; } 
}
