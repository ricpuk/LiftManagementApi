using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LiftManagementApi.Controllers
{
    [ApiController]
    public class LiftsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<LiftDto>>> Index()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IList<LiftDto>>> Index([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Call([FromRoute] int id, CallLiftDto callLiftDto)
        {
            return Ok();
        }
    }
}
