using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pocos
{
    public class Carrier : Abstract.Business
    {
        public Carrier()
            :base(null)
        { 
        }

        public Carrier(Models.Data.Carrier a)
            :base(a)
        { 
        }
    }
}