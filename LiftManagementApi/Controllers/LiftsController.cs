using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiftManagementApi.Controllers
{
    [ApiController]
    public class LiftsController : ApiControllerBase
    {
        private readonly ILiftService _liftService;

        public LiftsController(ILiftService liftService)
        {
            _liftService = liftService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<LiftInfoDto>>> Index()
        {
            return Ok(_liftService.GetList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<LiftLogDto>>> Index([FromRoute] int id)
        {
            return Ok(_liftService.GetLiftLogs(id));
        }

        [HttpPost("{id}")]
        public ActionResult Call([FromRoute] int id, CallLiftDto callLiftDto)
        {
            var result = _liftService.CallLift(id, callLiftDto);
            if (!result)
            {
                return BadRequest("Bad request");
            }
            return Ok();
        }
    }
}
