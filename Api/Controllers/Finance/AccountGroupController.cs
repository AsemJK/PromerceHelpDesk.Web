using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Models.ModelsViews;
using PromerceCRM.API.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PromerceCRM.API.Web.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountGroupController : ControllerBase
    {
        private readonly IAccountGroupService _accountGroupService;

        public AccountGroupController(IAccountGroupService accountGroupService)
        {
            _accountGroupService = accountGroupService;
        }
        // GET: api/<AccountGroupController>
        [HttpGet]
        public IEnumerable<AccountGroupDto> Get()
        {
            return _accountGroupService.GetAll();
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountGroupController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountGroupController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
