using FluentResults;
using KeyStone.Concerns.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Core.Contracts
{
    public interface ISampleContract
    {
        public Task<Result<SampleConcern>> GetById(int id);
        public Task<Result<IEnumerable<SampleConcern>>> GetAll();
        public Task<Result<SampleConcern>> Add(SampleConcern concern);
        public Task<Result<SampleConcern>> Update(int id,SampleConcern concern);
        public Task<Result> Delete(int id);

    }
}
