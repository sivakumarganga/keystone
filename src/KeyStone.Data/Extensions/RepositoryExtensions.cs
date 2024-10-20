using System.Linq.Expressions;
using EntityFrameworkCore.Repository.Interfaces;

namespace KeyStone.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<T> SingleOrDefaultAsync<T>(this IRepository<T> repository,Expression<Func<T,bool>> predicate) where T : class
        {
            var query = repository.SingleResultQuery()
                          .AndFilter(predicate);
           return await repository.SingleOrDefaultAsync(query);
        }
    }
}
