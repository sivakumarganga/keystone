using KeyStone.Concerns.Domain;
using KeyStone.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyStone.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/SampleEntity")]
    public class SampleEntityController : ControllerBase
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
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _sampleEntityService.GetById(id);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }
        [HttpPost]
        public async Task<IActionResult> Add(SampleConcern concern)
        {
            var result = await _sampleEntityService.Add(concern);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SampleConcern concern)
        {
            var result = await _sampleEntityService.Update(id, concern);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sampleEntityService.Delete(id);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.IsSuccess);
        }



    }
}
