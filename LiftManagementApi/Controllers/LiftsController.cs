using System.Collections.Generic;
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
        public ActionResult<IList<LiftInfoDto>> Index()
        {
            return Ok(_liftService.GetList());
        }

        [HttpGet("{id}")]
        public ActionResult<LiftInfoDto> Index([FromRoute] int id)
        {
            return Ok(_liftService.GetById(id));
        }

        [HttpGet("{id}/logs")]
        public ActionResult<List<LiftLogDto>> Logs([FromRoute] int id)
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
