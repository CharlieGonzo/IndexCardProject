using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndexCardBackendApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace IndexCardBackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users
            .Include(p => p.Decks)
            .ThenInclude(c => c.Cards)
            .Select(x => UserToDTO(x))
            .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
           var user =  _context.Users.Include(p => p.Decks).ThenInclude(c => c.Cards).FirstOrDefault(u => u.Id == id);
            
            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO{Id = user.Id,Username = user.Username,Decks = user.Decks};
        }

        //Put api/Users/add/{name}
        [HttpPut("/add/{Id}/{name}")]
        public async Task<IActionResult> addCardDeck(String name,long Id){
            var user = _context.Users.Include(p => p.Decks).ThenInclude(c => c.Cards).FirstOrDefault(u => u.Id == Id);
            if (user == null)
            {
                return BadRequest();
            }
            user.AddDeck(name);
            UserDTO use =  new UserDTO{
                Id = user.Id,
                Username = user.Username,
                Decks = user.Decks

            };
            await PutUser(Id,use);
            return NoContent();
        }



        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserDTO user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            
            var UpdatedUser = await _context.Users.FindAsync(id);
            UpdatedUser.Id = user.Id;
            UpdatedUser.Username = user.Username;
            UpdatedUser.Decks = user.Decks;

            _context.Entry(UpdatedUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(LoginInfo user)
        {   
            if(user.username.IsNullOrEmpty() || user.password.IsNullOrEmpty()){
                return BadRequest();
            }
            var use = await _context.Users.FirstOrDefaultAsync(p => p.Username == user.username && p.Password == user.password);
            if(use != null){
                return UserToDTO(use);
            }
            var NewUser = new User(user.username,user.password);
            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = NewUser.Id }, UserToDTO(NewUser));
        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private static UserDTO UserToDTO(User user) =>
            new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Decks = user.Decks
            };   
}
    }

