namespace CMS.App.Common.Interface
{
    
    public interface IQueryHelper<T>
    {
        /// <summary>
        /// 校验前端传入的东西在数据库中是否合法，   
        /// 
        /// 举个例子：
        ///     Data["Username"] == Request.Query["Username"] 则说明该数据通过了校验
        /// </summary>
        /// <param name="data">需要被校验的数据（条）</param>
        /// <returns>校验的结果</returns>
        bool ValidateData(T data);

        /// <summary>
        /// 从当前请求中获取指定的参数的值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <returns>参数的值</returns>
        string GetRequestQueryParam(string paramName);
    }
}