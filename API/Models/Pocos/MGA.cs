using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pocos
{
    public class MGA : Abstract.Business
    {
        public MGA()
            :base(null)
        {

        }

        public MGA(Models.Data.MGA a)
           : base(a)
        {
        }
    }
}