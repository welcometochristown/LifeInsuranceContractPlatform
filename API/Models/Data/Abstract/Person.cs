using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Data.Abstract
{
    public abstract class Person : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return string.Join(" ", new[] { FirstName, LastName }.Where(n => !string.IsNullOrWhiteSpace(n)));
        }
    }
}