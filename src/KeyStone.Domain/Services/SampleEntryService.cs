using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentResults;
using KeyStone.Concerns.Domain;
using KeyStone.Core.Contracts;
using KeyStone.Data;
using KeyStone.Data.Models;
using KeyStone.Data.RepoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Domain.Services
{
    public class SampleEntityService : ISampleContract
    {
        // implement the interface here
        IUnitOfWork _unitOfWork;
        ISampleEntityRepository _sampleEntityRepository;
        IRepository<SampleEntity> _sampleEntityRepo;
        public SampleEntityService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            _sampleEntityRepo = this._unitOfWork.Repository<SampleEntity>();
            _sampleEntityRepository = this._unitOfWork.CustomRepository<ISampleEntityRepository>();
        }
        public async Task<Result<IEnumerable<SampleConcern>>> GetAll()
        {

            var result = await _sampleEntityRepository.GetAll();
            var sampleConcerns = result.Select(x => new SampleConcern()
            {
                Id = x.Id,
                Name = x.Name
            });
            return Result.Ok(sampleConcerns);
        }
        public async Task<Result<SampleConcern>> GetById(int id)
        {
            var result = await _sampleEntityRepo.SingleOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return Result.Fail<SampleConcern>($"No record found with id {id}");
            }
            var sampleConcern = new SampleConcern()
            {
                Id = result.Id,
                Name = result.Name
            };
            return Result.Ok(sampleConcern);
        }
        public async Task<Result<SampleConcern>> Add(SampleConcern concern)
        {
            var entity = new SampleEntity()
            {
                Name = concern.Name
            };
            _sampleEntityRepo.Add(entity);
            var changesCount = _unitOfWork.SaveChanges();
            if (changesCount == 0)
            {
                return Result.Fail<SampleConcern>("Failed to save record");
            }
            concern.Id = entity.Id;
            return Result.Ok(concern);
        }
        public async Task<Result<SampleConcern>> Update(int id, SampleConcern concern)
        {
            var entity = await _sampleEntityRepo.SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return Result.Fail<SampleConcern>($"No record found with id {concern.Id}");
            }
            entity.Name = concern.Name;
            //await _sampleEntityRepo.UpdateAsync(_ => _.Id == id, _ => _.SetProperty(p => p.Name, v => v.Name));
            _sampleEntityRepo.Update(entity);
            var changesCount = await _unitOfWork.SaveChangesAsync();
            if (changesCount == 0)
            {
                return Result.Fail<SampleConcern>("Failed to save record");
            }
            return Result.Ok(concern);
        }
        public async Task<Result> Delete(int id)
        {
            var entity = await _sampleEntityRepo.SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return Result.Fail($"No record found with id {id}");
            }
            await _sampleEntityRepo.RemoveAsync(_ => _.Id == id);
            var changesCount = _unitOfWork.SaveChanges();
            if (changesCount == 0)
            {
                return Result.Fail("Failed to delete record");
            }
            return Result.Ok();
        }

    }
}
