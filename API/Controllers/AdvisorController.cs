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
    [Route("Advisor")]
    public class AdvisorController : ControllerBase
    {
        private readonly ILogger<AdvisorController> _logger;
        private EntityDbContext _context;

        /// <summary>
        /// AdvisorController Constructor 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AdvisorController(EntityDbContext context, ILogger<AdvisorController> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        /// <summary>
        /// Get all advisors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Advisors
                                    .Select(n => new Advisor(n))
                                    .ToList());

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get every {nameof(Advisor)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get an advisor with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Models.Data.Advisor a = _context.Advisors.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Advisor)} with id {id} exists.");

                var n = new Advisor(a);

                return Ok(n);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get {nameof(Advisor)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new advisor
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Create([FromBody] Advisor a)
        {
            try
            {
                
                //add new Advisor
                var newentry = new Models.Data.Advisor
                {
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Address = a.Address,
                    PhoneNumber = a.PhoneNumber,
                };

                if (!string.IsNullOrWhiteSpace(a.HealthStatus))
                {
                    if(!Enum.TryParse(typeof(Models.Data.Advisor.Health), a.HealthStatus, true, out object health))
                        throw new InvalidOperationException("Incorrect health status, please choose GREEN or RED");

                    newentry.HealthStatus = (Models.Data.Advisor.Health)health;
                }
                                 
                _context.Advisors.Add(newentry);

                //save database
                _context.SaveChanges();

                return Ok(newentry.ID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to create {nameof(Advisor)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Update an existing advisor
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update([FromBody] Advisor a)
        {
            try
            {
                Models.Data.Advisor item = _context.Advisors.SingleOrDefault(n => n.ID == a.ID);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Advisor)} with id {a.ID} exists.");

                //update Advisor fields
                item.FirstName = a.FirstName;
                item.LastName = a.LastName;
                item.PhoneNumber = a.PhoneNumber;
                item.Address = a.Address;

                //save database
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to update {nameof(Advisor)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Delete an advisor with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Models.Data.Advisor a = _context.Advisors.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Advisor)} with id {id} exists");

                _context.Contracts.RemoveRange(_context.Contracts.Where(n => n.Entity1ID == id || n.Entity2ID == id));
                _context.Advisors.Remove(a);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to delete {nameof(Advisor)}");
                return BadRequest(ex.Message);
            }
        }
    }
}
