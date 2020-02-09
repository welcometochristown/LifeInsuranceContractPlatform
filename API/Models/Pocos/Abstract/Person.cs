using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pocos.Abstract
{
    public abstract class Person : Entity
    {
        public Person(Models.Data.Abstract.Person a)
            :base(a)
        {
            if (a == null)
                return;

            this.FirstName = a.FirstName;
            this.LastName = a.LastName;
            this.Address = a.Address;
            this.PhoneNumber = a.PhoneNumber;
        }

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