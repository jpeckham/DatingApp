using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserAsync([FromRoute]int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AppUser>> DeleteUserAsync([FromRoute]int id)
        {
            var entity = await _context.Users.FindAsync(id);
            
            if(entity == null)
                return NotFound();
                
            _context.Users.Remove(entity);
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> PostUserAsync(AppUser user)
        {
             // assume Entity base class have an Id property for all items
            if(user.Id == 0)//insert
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Created($"api/User/{user.Id}",user);
            }
            else 
            return BadRequest("No id allowed");
        }
        
        [HttpPut]
        public async Task<ActionResult<AppUser>> PutUserAsync(AppUser user)
        {
            if(user.Id <= 0)
            {
                return BadRequest("Valid Id value is required");
            }
            var entity = await _context.Users.FindAsync(user.Id);
            if (entity == null)
            {
                return BadRequest($"User with Id {user.Id} did not exist. Cannot update.");
            }
            _context.Entry(entity).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();
             return Created($"api/User/{user.Id}",user);
        }
    }
}