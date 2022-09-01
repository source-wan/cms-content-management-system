namespace CMS.Domain.Entity
{
    public class UserInfo:Base.BaseEntity
    {
        public string Username {get;set;}=null!;

        public string Password {get;set;}=null!;

        public Guid? Avatar {get;set;}

        public string? NickName {get;set;}

        public bool isAdmin{get;set;}=false;
        //普通用户为false 管理员为true
        
    }
}