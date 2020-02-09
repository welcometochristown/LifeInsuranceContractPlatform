using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Data.Abstract
{
    public abstract class Business : Entity
    {
        public string BusinessName { get; set; }
        public string BusinessAdress { get; set; }
        public string BusinessPhoneNumber { get; set; }

        public override string ToString()
        {
            return BusinessName;
        }
    }
}