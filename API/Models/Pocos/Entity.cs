using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models.Pocos
{
    public class Entity
    {
        public int ID { get; set; }

        public string Name { get; }

        public Entity()
        {

        }

        public Entity(Models.Data.Entity e)
        {
            if (e== null)
                return;

            this.ID = e.ID;
            this.Name = e.ToString();
            this.Contracts = e.Contracts1.Union(e.Contracts2).Select(n => new Contract(n));
        }

        public IEnumerable<Contract> Contracts { get; set; } = new List<Contract>();
       
    }
}
