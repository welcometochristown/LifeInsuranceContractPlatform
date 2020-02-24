using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.Models;
using API.Models.Pocos;
using API.Models.Data.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/MGA")]
    public class MGAController : ControllerBase
    {
        private readonly ILogger<MGAController> _logger;
        private EntityDbContext _context;

        /// <summary>
        /// MGAController Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public MGAController(EntityDbContext context, ILogger<MGAController> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        /// <summary>
        /// Get all MGAs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.MGAs
                                    .Select(n => new MGA(n))
                                    .ToList());

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get every {nameof(MGA)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets an MGA with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Models.Data.MGA a = _context.MGAs.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(MGA)} with id {id} exists.");

                return Ok(new MGA(a));

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get {nameof(MGA)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new MGA
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Create([FromBody] MGA a)
        {
            try
            {
                //add new MGA
                var newentry = new Models.Data.MGA
                {
                    BusinessName = a.BusinessName,
                    BusinessAddress = a.BusinessAddress,
                    BusinessPhoneNumber = a.BusinessPhoneNumber,
                };

                _context.MGAs.Add(newentry);

                //save database
                _context.SaveChanges();

                return Ok(newentry.ID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to create {nameof(MGA)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Update an existing MGA
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update([FromBody] MGA a)
        {
            try
            {
                Models.Data.MGA item = _context.MGAs.SingleOrDefault(n => n.ID == a.ID);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(MGA)} with id {a.ID} exists.");

                //update MGA fields
                item.BusinessName = a.BusinessName;
                item.BusinessAddress = a.BusinessAddress;
                item.BusinessPhoneNumber = a.BusinessPhoneNumber; ;

                //save database
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to update {nameof(MGA)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Delete an MGA with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Models.Data.MGA a = _context.MGAs.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(MGA)} with id {id} exists");

                _context.Contracts.RemoveRange(_context.Contracts.Where(n => n.Entity1ID == id || n.Entity2ID == id));
                _context.MGAs.Remove(a);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to delete {nameof(MGA)}");
                return BadRequest(ex.Message);
            }
        }
    }
}
