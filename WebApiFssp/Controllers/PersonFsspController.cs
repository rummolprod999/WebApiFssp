using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiFssp.Models;

namespace WebApiFssp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonFsspController : ControllerBase

    {
        private readonly ILogger<FsspPerson> _logger;

        public PersonFsspController(ILogger<FsspPerson> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Get([FromQuery]FsspPerson person)
        {
            if (person==null)
            {
                ModelState.AddModelError("", "Bad data");
                return BadRequest(ModelState);
            }

            return Ok(person);
        }
        
        
    }
}