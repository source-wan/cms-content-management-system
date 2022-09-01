namespace CMS.Domain.Entity
{
    public class ArticleInfo:Base.BaseEntity
    {
        public string Title {get;set;}=null!;

        public string  Content {get;set;}=null!;

        public int? CommentCount {get;set;}

        public int VisibleCount {get;set;}=0;

        public Guid CategoryId {get;set;}

    //记得加关系 public CategoryInfo CategoryInfo  {get;set;}
    }
}