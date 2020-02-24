using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pocos.Abstract
{
    public abstract class Business : Entity
    {
        public Business(Models.Data.Abstract.Business b)
            :base(b)
        {
            if (b == null)
                return;
            
            this.BusinessName = b.BusinessName;
            this.BusinessAddress = b.BusinessAddress;
            this.BusinessPhoneNumber = b.BusinessPhoneNumber;
        }

        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessPhoneNumber { get; set; }

        public override string ToString()
        {
            return BusinessName;
        }
    }
}