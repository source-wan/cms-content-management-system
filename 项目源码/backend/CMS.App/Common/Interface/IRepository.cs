namespace CMS.App.Common.Interface
{
    public interface IRepository<T>
    {
        /// <summary>
        /// 用于查找的数据表
        /// </summary>
        /// <value></value>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 根据 Id 获取对应表的数据实例
        /// </summary>
        /// <param name="id">需要获取到的实例的Id</param>
        /// <returns>获取到的实例, 可能为空</returns>
        Task<T?> GetById(Guid? id);

        /// <summary>
        /// 获取指定页面的数据
        /// </summary>
        /// <param name="pageIndex">需要获取的页面的索引</param>
        /// <param name="pageSize">自定义页长</param>
        /// <returns></returns>
        IEnumerable<T> GetByPageIndex(int pageIndex = 1, int pageSize = 5);
        /// <summary>
        /// 从指定的数据源获取指定页面的数据，
        /// 之所以把这玩意做成一个重载， 是因为可选参数只能是编译时常量
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="pageIndex">需要获取的页面的索引</param>
        /// <param name="pageSize">自定义页长</param>
        /// <returns></returns>
        IEnumerable<T> GetByPageIndex(IQueryable<T> dataSource, int pageIndex, int pageSize);

        /// <summary>
        /// 向表中插入数据， 该方法具有一个重载，支持通过列表批量插入数据
        /// </summary>
        /// <param name="entity">需要被插入的数据的实例</param>
        /// <returns>插入后的实例</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 向表中批量插入数据
        /// </summary>
        /// <param name="entities">需要被批量插入的数据的实例</param>
        /// <returns>被批量插入的数据的实例</returns>
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities);

        /// <summary>
        /// 更新实体到数据库， 同样具有一个重载用于批量更新
        /// </summary>
        /// <param name="entity">需要更新的实例</param>
        /// <returns>被更新后的数据（实例）</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// 批量更新实例到数据库
        /// </summary>
        /// <param name="entities">需要被批量更新的数据（实例）</param>
        /// <returns>被批量更新后的数据（实例）</returns>
        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// 删除数据， 该方法依旧具有一个重载用于批量删除数据
        /// </summary>
        /// <param name="entity">需要被删除的数据（实例）</param>
        /// <param name="shoudHardDel">是否硬删除， 默认为 false</param>
        /// <returns>删除数据的结果</returns>
        Task<T?> DeleteAsync(T entity, bool shoudHardDel = false);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities">需要被批量删除的数据（实例）</param>
        /// <param name="shoudHardDel">是否硬删除， 默认为 false</param>
        /// <returns>删除数据的结果</returns>
        Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities, bool shoudHardDel = false);
    }
}