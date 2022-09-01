using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Common.Interface
{
    public interface IAppFileUpload
    {
        Task<IEnumerable<string>> UploadFile(IFormCollection files);

        Task<FileContentResult> GetFileById(Guid id);
    }
}