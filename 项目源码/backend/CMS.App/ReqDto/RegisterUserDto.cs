namespace CMS.App.ReqDto
{
    public class RegisterUserDto
    {
        public string Username {get;set;}=null!;
        public string Password {get;set;}=null!;
        public string? NickName {get;set;}
         public bool isAdmin{get;set;}=false;
    }
}