namespace CMS.Domain.Entity
{
    public class Comments:Base.BaseEntity
    {
        public Guid ArtocleId {get;set;}

        public string Content {get;set;}=null!;
        public int LikeCount {get;set;}=0;
        public int UnlikeCount {get;set;}=0;
    
        //记得加关系 public ArticleInfo ArticleInfo{get;set;}
    }
}