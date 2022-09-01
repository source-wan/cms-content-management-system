namespace CMS.App.Common.Interface
{
    public interface IUserSession
    {
        /// <summary>
        /// 用户Id, 从 Session 中获取
        /// </summary>
        /// <value></value>
        Guid? Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        /// <value></value>
        string? Name { get; }
    }
}