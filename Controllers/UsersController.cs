using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndexCardBackendApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;


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
            //returns list of users with all of their children as well
            return await _context.Users
            .Include(p => p.Decks)
            .ThenInclude(c => c.Cards)
            .Select(x => UserToDTO(x))
            .ToListAsync();
        }

        

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody]LoginInfo info){
          
            User user = await _context.Users.Where(x => x.Username == info.username).Include(p => p.Decks)
            .ThenInclude(c => c.Cards).SingleOrDefaultAsync();
            
            //if user is null return badRequest
            if(user == null){
                return BadRequest("invalid username or password");
            }

            if(user.Password != info.password){
                return BadRequest("invalid password");
            }

            return Ok(UserToDTO(user));
        }

        
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long Id)
        {
            //retrieve the user from database
            var user = await _context.Users
                    .Include(u => u.Decks)
                    .ThenInclude(d => d.Cards)
                    .FirstOrDefaultAsync(u => u.Id == Id);
            
            //check if user is null, if null, return notFound()
            if (user == null)
            {
                return NotFound();
            }

            //return DTO of User
            return new UserDTO{Id = user.Id,Username = user.Username,Decks = user.Decks};
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
        

        
         [HttpPatch("{id}")]
        public async Task<ActionResult<HttpStatusCode>> AddDeck(long id,[FromBody]String name)
        {
            if(!UserExists(id)){
                return BadRequest();
            }

            var UpdatedUser = await _context.Users.FindAsync(id);

            if(UpdatedUser == null){
                return HttpStatusCode.InternalServerError;
            }

            UpdatedUser.AddDeck(name);

            //update the user in the database after changes
            _context.Entry(UpdatedUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return HttpStatusCode.NotFound;
                }
                else
                {
                    throw;
                }
            }

            return HttpStatusCode.NoContent;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser([FromBody]LoginInfo user)
        {   
            if(user.username.IsNullOrEmpty() || user.password.IsNullOrEmpty()){
                return BadRequest();
            }
            var use = await _context.Users.FirstOrDefaultAsync(p => p.Username == user.username && p.Password == user.password);
            if(use != null){
                return BadRequest("user already exist");
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

