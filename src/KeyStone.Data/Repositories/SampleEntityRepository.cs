using EntityFrameworkCore.Repository;
using EntityFrameworkCore.Repository.Interfaces;
using KeyStone.Data.Models;
using KeyStone.Data.RepoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data.Repositories
{
    public class SampleEntityRepository : Repository<SampleEntity>,IRepository<SampleEntity>,IAsyncRepository<SampleEntity>, ISampleEntityRepository
    {
        public SampleEntityRepository(KeyStoneDbContext dbContext):base(dbContext)
        {
            
        }
        public async Task<IEnumerable<SampleEntity>> GetAll()
        {
            var query = this.MultipleResultQuery();

            return await this.SearchAsync(query);
        }
    }
}
