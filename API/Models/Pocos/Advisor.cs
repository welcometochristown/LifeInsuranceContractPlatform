using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pocos
{
    public class Advisor : Abstract.Person
    {
        public Advisor()
            :base(null)
        {

        }

        public Advisor(Models.Data.Advisor a)
            :base(a)
        {
            this.HealthStatus = a.HealthStatus.ToString();
        }

        public string HealthStatus { get; set; }
    }
}