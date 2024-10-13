using EntityFrameworkCore.UnitOfWork.Interfaces;
using KeyStone.Core.Services;
using KeyStone.Data.Models;
using KeyStone.Data.RepoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data
{
    public class BasicDataSeeder : IDataSeedService
    {
        IUnitOfWork _unitOfWork;
        public BasicDataSeeder(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task Seed()
        {

            var sampleEntityRepo = this._unitOfWork.Repository<SampleEntity>();
            var customSampleRepo = this._unitOfWork.CustomRepository<ISampleEntityRepository>();
            var result = await customSampleRepo.GetAll();

            sampleEntityRepo.Add(new SampleEntity() { Name = "Sample Record" });
            var changesCount = _unitOfWork.SaveChanges();
        }
    }
}
