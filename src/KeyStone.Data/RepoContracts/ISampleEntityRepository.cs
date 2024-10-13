using KeyStone.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data.RepoContracts
{
    public interface ISampleEntityRepository
    {
        public Task<IEnumerable<SampleEntity>> GetAll();
    }
}
