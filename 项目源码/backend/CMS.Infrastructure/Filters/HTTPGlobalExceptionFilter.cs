using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.App.Common.Interface;
using CMS.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS.Infrastructure.Filters
{
    public class HTTPGlobalExceptionFilter:IAsyncExceptionFilter
    {
        private readonly IRepository<AppException> _appException;

        public HTTPGlobalExceptionFilter(IRepository<AppException> AppException)
        {
            _appException=AppException;
        }
        
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            //捕获全局异常，记入数据库日志
            await _appException.AddAsync(new AppException
            {
                ShortMessage = context.Exception.Message,
                FullMessage = context.Exception.ToString()
            });
            context.ExceptionHandled = true;
        }
    }
}