using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using API.Models;
using API.Models.Pocos;
using API.Models.Data.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Contract")]
    public class ContractController : ControllerBase
    {
        private readonly ILogger<ContractController> _logger;
        private EntityDbContext _context;

        /// <summary>
        /// ContractController Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public ContractController(EntityDbContext context, ILogger<ContractController> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        /// <summary>
        /// Get all Contracts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Contracts.Select(n => new Contract(n)).ToList());
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get every {nameof(Contract)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a Contract between {entity1Id} and {entity2Id}
        /// </summary>
        /// <param name="entity1Id"></param>
        /// <param name="entity2Id"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("/api/Contract/{entity1Id}/{entity2Id}")]
        public IActionResult Get(int entity1Id, int entity2Id)
        {
            try
            {
                Models.Data.Contract a = _context.Contracts.SingleOrDefault(n => (n.Entity1ID == entity1Id && n.Entity2ID == entity2Id) ||
                                                                                (n.Entity2ID == entity1Id && n.Entity1ID == entity2Id));

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Contract)} between {entity1Id} and {entity2Id}.");

                return Ok(new Contract(a));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to get {nameof(Contract)}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a Contract between {entity1Id} and {entity2Id}
        /// </summary>
        /// <param name="entity1Id"></param>
        /// <param name="entity2Id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/Contract/Establish/{entity1Id}/{entity2Id}")]
        public IActionResult Create(int entity1Id, int entity2Id)
        {
            try
            {
                if (entity1Id == entity2Id)
                    throw new InvalidOperationException($"You can't create a contact with between the same entity.");

                Models.Data.Contract a = _context.Contracts.SingleOrDefault(n => (n.Entity1ID == entity1Id && n.Entity2ID == entity2Id) ||
                                                                                (n.Entity2ID == entity1Id && n.Entity1ID == entity2Id));

                if (a != null)
                    throw new InvalidOperationException($"You can't create a contact when one already exists between {entity1Id} and {entity2Id}.");

                //add new Contract
                _context.Contracts.Add(new Models.Data.Contract { Entity1ID = entity1Id, Entity2ID = entity2Id });

                //save database
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to create {nameof(Contract)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Terminate a Contract between {entity1Id} and {entity2Id}
        /// </summary>
        /// <param name="entity1Id"></param>
        /// <param name="entity2Id"></param>
        /// <returns></returns>
        [HttpDelete()]
        [Route("/api/Contract/Terminate/{entity1Id}/{entity2Id}")]
        public IActionResult Delete(int entity1Id, int entity2Id)
        {
            try
            {
                Models.Data.Contract a = _context.Contracts.SingleOrDefault(n => (n.Entity1ID == entity1Id && n.Entity2ID == entity2Id) ||
                                                                                (n.Entity2ID == entity1Id && n.Entity1ID == entity2Id));

                if (a == null)
                    throw new InvalidOperationException($"No {nameof(Contract)} exists between {entity1Id} and {entity2Id}");

                _context.Contracts.Remove(a);
                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to terminate {nameof(Contract)}");
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Finds the smallest Contract chain between {entity1Id} and {entity2Id}
        /// </summary>
        /// <param name="entity1Id"></param>
        /// <param name="entity2Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Contract/SmallestChain/{entity1Id}/{entity2Id}")]
        public IActionResult SmallestChain(int entity1Id, int entity2Id)
        {
            try
            {
                Models.Data.Entity e1 = _context.Entities.SingleOrDefault(n => n.ID == entity1Id);
                Models.Data.Entity e2 = _context.Entities.SingleOrDefault(n => n.ID == entity2Id);

                if (e1 == null)
                    throw new InvalidOperationException($"No entity exists with id {entity1Id}");

                if (e2 == null)
                    throw new InvalidOperationException($"No entity exists with id {entity2Id}");

                //check for a direct connection between e1 and e2, if so we dont need to search
                if (_context.Contracts.Any(n => (n.Entity1 == e1 && n.Entity2 == e2) || (n.Entity1 == e2 && n.Entity2 == e1)))
                {
                    return Ok(new[] {
                        new EntitySimple() {  ID = e1.ID, Name = e1.ToString() },
                        new EntitySimple() {  ID = e2.ID, Name = e2.ToString() }
                    });
                }

                Dictionary<Models.Data.Entity, Tuple<Models.Data.Entity, int>> nodemap = new Dictionary<Models.Data.Entity, Tuple<Models.Data.Entity, int>>(); // this will keep track of all nodes, their parent and how long it took to get there

                Models.Data.Entity last = null;
                Queue<Models.Data.Entity> toVisit = new Queue<Models.Data.Entity>();

                //first node
                nodemap.Add(e1, new Tuple<Models.Data.Entity, int>(null, 0));
                toVisit.Enqueue(e1);

                while (toVisit.Count > 0)
                {
                    //pop entity
                    var e = toVisit.Dequeue();

                    //find the current distance of this node from [e1]
                    int distance = nodemap[e].Item2;

                    //find all the neighbours (other contracts spawning from this one)
                    var neighbours = e.Contracts1.Select(n => e == n.Entity1 ? n.Entity2 : n.Entity1).Union(e.Contracts2.Select(n => e == n.Entity1 ? n.Entity2 : n.Entity1));

                    //iterate over each neighbour
                    foreach (var n in neighbours)
                    {
                        //ignore the previous entity
                        if (last == n)
                            continue;

                        //if the nodemap doesnt contain this node, add it
                        if (!nodemap.ContainsKey(n))
                        {
                            //add node with parent and distance
                            nodemap.Add(n, new Tuple<Models.Data.Entity, int>(e, distance + 1));
                            toVisit.Enqueue(n);
                        }
                        else
                        {
                            Tuple<Models.Data.Entity, int> t = nodemap[n];

                            //update if a shorter distance was found
                            if (distance + 1 < t.Item2)
                                nodemap[e] = new Tuple<Models.Data.Entity, int>(n, distance + 1);
                        }
                    }

                    last = e;
                }

                List<Models.Data.Entity> results = new List<Models.Data.Entity>();

                //iterate back through the nodes of the best solution
                for (var node = e2; node != null;)
                {
                    if (!nodemap.ContainsKey(node))
                        return NotFound($"No {nameof(Contract)} chain between {e1.ToString()} and {e2.ToString()}");

                    //reverse nodes
                    results.Insert(0, node);
                    node = nodemap[node].Item1;
                }

                return Ok(results.Select(n => new EntitySimple { ID = n.ID, Name = n.ToString() })) ;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed finding the shortest {nameof(Contract)} chain found");
                return BadRequest(ex.Message);
            }
            
        }

    }
}
