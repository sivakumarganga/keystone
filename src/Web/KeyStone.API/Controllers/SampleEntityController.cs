using Asp.Versioning;
using KeyStone.Concerns.Domain;
using KeyStone.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyStone.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/SampleEntity")]
    public class SampleEntityController : BaseController
    {
        private ISampleContract _sampleEntityService;
        public SampleEntityController(ISampleContract sampleEntityService)
        {
            _sampleEntityService = sampleEntityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sampleEntityService.GetAll();
            return  ResultResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _sampleEntityService.GetById(id);
            return ResultResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(SampleConcern concern)
        {
            var result = await _sampleEntityService.Add(concern);
            return ResultResponse(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SampleConcern concern)
        {
            var result = await _sampleEntityService.Update(id, concern);
            return ResultResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sampleEntityService.Delete(id);
            return ResultResponse(result);
        }
    }
}
