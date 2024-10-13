using EntityFrameworkCore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data
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
