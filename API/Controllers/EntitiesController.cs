using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.Models;
using API.Models.Data.Context;
using API.Models.Pocos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Entities")]
    public class EntitiesController : ControllerBase
    {
        private readonly ILogger<AdvisorController> _logger;
        private EntityDbContext _context;

        /// <summary>
        /// EntitiesController Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public EntitiesController(EntityDbContext context, ILogger<AdvisorController> logger)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Dictionary<string, object> entities = new Dictionary<string, object>();
                entities.Add(nameof(Advisor), _context.Advisors.Select(n => new Entity(n)).ToList());
                entities.Add(nameof(Carrier), _context.Carriers.Select(n => new Entity(n)).ToList());
                entities.Add(nameof(MGA), _context.MGAs.Select(n => new Entity(n)).ToList());
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get every {nameof(Entity)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Gets an entity with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Models.Data.Entity a = _context.Entities.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No entity exists with id {id} exists");

                return Ok(new Entity(a));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get {nameof(Entity)}");
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Delete an entity with {id}
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Models.Data.Entity a = _context.Entities.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No entity exists with id {id} exists");

                _context.Contracts.RemoveRange(_context.Contracts.Where(n => n.Entity1ID == id || n.Entity2ID == id));
                _context.Entities.Remove(a);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to delete {nameof(Entity)}");
                return BadRequest(ex.Message);
            }
            
        }

    }
}
