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
    [Route("Carrier")]
    public class CarrierController : ControllerBase
    {
        private readonly ILogger<CarrierController> _logger;
        private EntityDbContext _context;

        /// <summary>
        /// CarrierController Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public CarrierController(EntityDbContext context, ILogger<CarrierController> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        /// <summary>
        /// Get all Carriers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Carriers
                                    .Select(n => new Carrier(n))
                                    .ToList());

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get every {nameof(Carrier)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a Carrier with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Models.Data.Carrier a = _context.Carriers.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Carrier)} with id {id} exists.");

                return Ok(new Carrier(a));

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get {nameof(Carrier)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new Carrier
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Create([FromBody] Carrier a)
        {
            try
            {
                //add new Carrier
                var newentry = new Models.Data.Carrier
                {
                    BusinessName = a.BusinessName,
                    BusinessAdress = a.BusinessAdress,
                    BusinessPhoneNumber = a.BusinessPhoneNumber,
                };

                _context.Carriers.Add(newentry);

                //save database
                _context.SaveChanges();

                return Ok(newentry.ID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to create {nameof(Carrier)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Update an existing Carrier
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Update([FromBody] Carrier a)
        {
            try
            {
                Models.Data.Carrier item = _context.Carriers.SingleOrDefault(n => n.ID == a.ID);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Carrier)} with id {a.ID} exists.");

                //update Carrier fields
                item.BusinessName = a.BusinessName;
                item.BusinessAdress = a.BusinessAdress;
                item.BusinessPhoneNumber = a.BusinessPhoneNumber;;

                //save database
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to update {nameof(Carrier)}");
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
                Models.Data.Carrier a = _context.Carriers.SingleOrDefault(n => n.ID == id);

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Carrier)} with id {id} exists");

                _context.Contracts.RemoveRange(_context.Contracts.Where(n => n.Entity1ID == id || n.Entity2ID == id));
                _context.Carriers.Remove(a);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to delete {nameof(Carrier)}");
                return BadRequest(ex.Message);
            }
        }
    }
}
