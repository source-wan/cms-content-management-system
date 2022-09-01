using CMS.App.Common.Interface;
using CMS.App.Util;
using Microsoft.AspNetCore.Mvc;
using FileInfo = CMS.Domain.Entity.FileInfo;

namespace CMS.Api.Controllers
{
    public class FileController:ControllerBase
    {
        private readonly IAppFileUpload _AppFileUpload;
        private readonly IRepository<FileInfo> _AppFileInfo;

        public FileController(IAppFileUpload AppFileUpload,IRepository<FileInfo> AppFileInfo)
        {
            _AppFileUpload = AppFileUpload;
            _AppFileInfo=AppFileInfo;
        }



        [HttpPost]
        [Route("/files")]
        public async Task<string> OnPostUploadAsync(IFormCollection files)
        {
            var list = await _AppFileUpload.UploadFile(files);
            var listdata=_AppFileInfo.Table.FirstOrDefault(x=>x.RelativePath==list.FirstOrDefault());
            if (list.Count() > 0)
            {
                return new
                {
                    Code = 1000,
                    Msg = "上传成功",
                    Data =listdata
                }.SerializeObject();
            }
            else
            {
                return new
                {
                    Code = 4000,
                    Msg = "上传失败，文件扩展名不被允许、文件大小不被允许",
                    Data = list
                }.SerializeObject();
            }
        }


        [HttpGet]
        [Route("/files/{id}")]
        public async Task<FileContentResult> GetFileAsync(Guid id)
        {
            var result=await _AppFileUpload.GetFileById(id);
            return result;
        }        
    }
}