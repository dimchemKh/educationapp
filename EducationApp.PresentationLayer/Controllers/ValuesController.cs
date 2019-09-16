using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Initialization;
using EducationApp.PresentationLayer.Filter;
using Microsoft.AspNetCore.Mvc;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ValuesController : ControllerBase
    {
        private RoleInitialization _roleInitialization;
        public ValuesController(RoleInitialization initializer)
        {
            _roleInitialization = initializer;
            //InitAsync();
            //_roleInitialization.InitializeAsync();
        }
        [HttpPost("init")]
        public async Task<ActionResult> InitAsync()
        {
           await _roleInitialization.InitializeAsync();

            return Ok("yeah");
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
