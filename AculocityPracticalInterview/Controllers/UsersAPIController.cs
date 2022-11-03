using Library.Data;
using Library.Data.Users;
using Microsoft.AspNetCore.Mvc;

namespace AculocityPracticalInterview.Controllers
{
    /// <summary>
    /// Controller for required api interface
    /// </summary>
    [Route("api/Users")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        private readonly IDataRepository<UsersDto> _usersDataRepository;
        public UsersAPIController(IDataRepository<UsersDto> usersDataRepository)
        {
            _usersDataRepository = usersDataRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetUsers()
        {
            var users = await _usersDataRepository.GetAllAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsersDto>> GetUser([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = (await _usersDataRepository.GetAllAsync()).Where(model => model.Id == id);
            return user != null && user.Count() == 1 ? user.First() : NotFound();
        }

        [HttpPost]
        public async Task InsertUser([FromBody] UsersDto usersDto)
        {
            await _usersDataRepository.InsertAsync(usersDto);
        }
    }
}
