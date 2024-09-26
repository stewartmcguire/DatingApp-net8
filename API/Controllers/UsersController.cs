using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(DataContext context) : ControllerBase
    {
/* [NOTE] C# 11 (and older) way of doing dependency injection
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
*/

        // Synchronous way of getting users
/*
        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = context.Users.ToList();

            return Ok(users);
        }

        [HttpGet("{id:int}")]   // api/users/3
        public ActionResult<AppUser> GetUser(int id)
        {
            var user = context.Users.Find(id);

            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }
*/
        // Asynchronous way of getting users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id:int}")]   // api/users/3
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }

    }
}
