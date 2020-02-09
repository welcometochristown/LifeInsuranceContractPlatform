using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models.Data
{
    public abstract class Entity
    {
        public int ID { get; set; }

        [JsonIgnore]
        public virtual ICollection<Contract> Contracts1 { get; set; } = new List<Contract>();

        [JsonIgnore]
        public virtual ICollection<Contract> Contracts2 { get; set; } = new List<Contract>();

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
