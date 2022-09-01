using CMS.App.Common.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FileInfo = CMS.Domain.Entity.FileInfo;

namespace CMS.Infrastructure.File
{
    public class AppFileUpload : IAppFileUpload
    {

        private readonly IConfiguration _configuration;
        private readonly IRepository<FileInfo> _AppFileInfo;

        public AppFileUpload(IConfiguration configuration, IRepository<FileInfo> AppFileInfo)
        {
            _configuration = configuration;
            _AppFileInfo = AppFileInfo;
        }


        public async Task<FileContentResult> GetFileById(Guid id)
        {
            var currentPath = Directory.GetCurrentDirectory();
            var fileInfo = await _AppFileInfo.GetById(id);
            if (fileInfo == null)
            {
                return new FileContentResult(new byte[0], "image/jpeg");
            }
            var fullPath = Path.Combine(currentPath, fileInfo.RelativePath);
            if (!System.IO.File.Exists(fullPath))
            {
                return new FileContentResult(new byte[0], "image/jpeg");
            }
            using (var sw = new FileStream(fullPath, FileMode.Open))
            {
                var contenttype = fileInfo.ContentType;
                var bytes = new byte[sw.Length];
                sw.Read(bytes, 0, bytes.Length);
                sw.Close();
                return new FileContentResult(bytes, contenttype);
            }
        }

        public async Task<IEnumerable<string>> UploadFile(IFormCollection files)
        {
            // （通过配置文件获取到的）设置上传文件5
            var uploadFilePath = _configuration.GetSection("UploadFilespath").GetValue<string>("FilePath");
            var allowExtension = _configuration.GetSection("UploadFilespath").GetValue<string>("AllowFileExtension").Split(",");
            var allowMaxSize = _configuration.GetSection("UploadFilespath").GetValue<long>("MaxFileSize");
            // 当前路径（完整路径）
            var currentPath = Directory.GetCurrentDirectory();

            // 用于容纳要返回的相对地址
            var list = new List<string>();

            var fileList = new List<FileInfo>();

            //过滤1 查找不符合指定扩展名的文件 配置文件设置形如：.jpg,.pdf
            var extensionFiles = files.Files.Where(x =>
            {
                var ext = Path.GetExtension(x.FileName).ToLowerInvariant();
                return string.IsNullOrEmpty(ext) || !allowExtension.Contains(ext);
            });

            if (extensionFiles.Count() > 0)
            {
                return list;
            }

            //过滤2 查找大小超过允许最大文件大小的文件，单位字节
            var allowMaxFileSize = files.Files.Where(x =>
            {
                return x.Length > allowMaxSize;
            });

            if (allowMaxFileSize.Count() > 0)
            {
                return list;
            }



            // 遍历上传的文件
            foreach (var formFile in files.Files)
            {

                if (formFile.Length > 0)
                {
                    // 随机文件名称
                    var rndFileName = Guid.NewGuid().ToString("N");

                    // 扩展名
                    var extenName = formFile.FileName.Substring(formFile.FileName.IndexOf("."));

                    // 新的文件名（包含扩展名）
                    var newFileName = string.Format("{0}{1}", rndFileName, extenName);

                    var now = DateTime.Now;

                    // 相对路径（不包含当前路径、不包含新文件路径）
                    var relativePath = Path.Combine(uploadFilePath, now.Year.ToString(), now.Month.ToString(), now.Day.ToString());

                    // 当前完整路径 + 相对路径
                    var preFileName = Path.Combine(currentPath, relativePath);

                    // 路径不存在则创建
                    if (!Directory.Exists(preFileName))
                    {
                        Directory.CreateDirectory(preFileName);
                    }

                    //  完整文件名称（包含路径、扩展名）
                    var fullPath = Path.Combine(preFileName, newFileName);


                    // 写入硬盘
                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var fullRelativePath = Path.Combine(relativePath, newFileName).Replace("\\", "/");
                    // 返回的文件路径（一般为相对路径）
                    list.Add(fullRelativePath);

                    fileList.Add(new FileInfo
                    {
                        OriginName = formFile.FileName,
                        CurrentName = newFileName,
                        RelativePath = fullRelativePath,
                        FileSize = formFile.Length.ToString(),
                        ContentType = formFile.ContentType
                    });
                }
            }


            await _AppFileInfo.AddAsync(fileList);
            return list;
        }

    }
}
