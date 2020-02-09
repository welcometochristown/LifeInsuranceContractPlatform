using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models.Pocos
{
    public class Contract
    {
        public struct ContractEntity
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public Contract()
        {

        }

        public Contract(Models.Data.Contract c)
        {
            this.Entity1 = new ContractEntity { ID = c.Entity1.ID, Name = c.Entity1.ToString() };
            this.Entity2 = new ContractEntity { ID = c.Entity2.ID, Name = c.Entity2.ToString() };
        }

        public ContractEntity Entity1 { get; set; }
        public ContractEntity Entity2 { get; set; }
    }
}
