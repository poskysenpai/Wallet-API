using System.Linq.Expressions;
using WalletAPI.Data;

namespace WalletAPI.Helper
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly AppDbContext _context; 
        public Repository( AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindByCondition(Expression<Func <T, bool>> condition)
        {
            return _context.Set<T>().Where(condition);
        }
    }
}
