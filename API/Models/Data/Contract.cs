using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models.Data
{
    public class Contract
    {
        public int Entity1ID { get; set; }
        public int Entity2ID { get; set; }

        [JsonIgnore]
        public virtual Entity Entity1 { get; set; }

        [JsonIgnore]
        public virtual Entity Entity2 { get; set; }
    }
}
