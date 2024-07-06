using System.Linq.Expressions;

namespace WalletAPI.Helper
{
    public interface IRepository<T>
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);
    }
}
