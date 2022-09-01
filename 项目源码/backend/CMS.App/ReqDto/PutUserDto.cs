using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.App.ReqDto
{
    public class PutUserDto
    {
        public string Username { get; set; }=null!; 
        public string Password {get;set;}=null!;

        public Guid? Avatar { get; set; }

        public string NickName { get; set; }=null!;

        public bool isAdmin {get;set;}=false;
    }
}