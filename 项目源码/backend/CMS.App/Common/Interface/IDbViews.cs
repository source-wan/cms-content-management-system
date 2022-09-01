namespace CMS.App.Common.Interface;
public interface IDbViews<TSource, TReturn>
{
    IList<TReturn> Get(Func<TSource, bool> validateFunc, out int count, int pageIndex = 1, int pageSize = 1);
    TReturn Get(Guid id);
}
