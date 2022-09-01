using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Domain.Entity
{
public class AppException : Base.BaseEntity
    {
        public string ShortMessage { get; set; } = null!;
        public string FullMessage { get; set; } = null!;
    }

}