using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Data
{
    public class Advisor : Abstract.Person
    {
        public enum Health
        {
            RED = 0, GREEN = 1
        }

        public Advisor(Health healthStatus)
        {
            this.HealthStatus = healthStatus;
        }

        public Advisor()
            :this((Health)(new Random().Next(0, 2)))
        {
        }

        public Health HealthStatus { get; set; }
    }
}