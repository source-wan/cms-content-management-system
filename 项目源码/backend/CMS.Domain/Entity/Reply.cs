namespace CMS.Domain.Entity
{
    public class Reply:Base.BaseEntity
    {
        public string Content {get;set;}=null!;

        public Guid CommentId {get;set;}
        public int LikeCount {get;set;}
        public int UnlikeCount {get;set;}
    }
}