using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace XUnitTestProject1
{
    public class ContractTest
    {
        /// <summary>
        /// Test Creating an Advisor
        /// </summary>
        [Fact]
        public void ContractCreate()
        {
            var context = Common.CreateDbContext();

            API.Controllers.AdvisorController a = new API.Controllers.AdvisorController(context, null);
            API.Controllers.MGAController m = new API.Controllers.MGAController(context, null);
            API.Controllers.ContractController c = new API.Controllers.ContractController(context, null);

            var modelA = new API.Models.Pocos.Advisor
            {
                FirstName = "Bob",
                LastName = "Belcher",
                Address = "123 Ocean Avenue",
                HealthStatus = "GREEN",
                PhoneNumber = "133 3133 133"
            };

            var modelB = new API.Models.Pocos.MGA
            {
                BusinessName = "Bobs Burgers",
                BusinessAdress = "123 Ocean Avenue",
                BusinessPhoneNumber = "133 3133 133"
            };

            var createAResult = a.Create(modelA) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createAResult.StatusCode.GetValueOrDefault());

            var createBResult = m.Create(modelB) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createBResult.StatusCode.GetValueOrDefault());

            var createCResult = c.Create((int)createAResult.Value, (int)createBResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, createCResult.StatusCode);

        }

        [Fact]
        public void ContractQuery()
        {
            var context = Common.CreateDbContext();

            API.Controllers.AdvisorController a = new API.Controllers.AdvisorController(context, null);
            API.Controllers.MGAController m = new API.Controllers.MGAController(context, null);
            API.Controllers.ContractController c = new API.Controllers.ContractController(context, null);

            var modelA = new API.Models.Pocos.Advisor
            {
                FirstName = "Bob",
                LastName = "Belcher",
                Address = "123 Ocean Avenue",
                HealthStatus = "GREEN",
                PhoneNumber = "133 3133 133"
            };

            var modelB = new API.Models.Pocos.MGA
            {
                BusinessName = "Bobs Burgers",
                BusinessAdress = "123 Ocean Avenue",
                BusinessPhoneNumber = "133 3133 133"
            };

            var createAResult = a.Create(modelA) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createAResult.StatusCode.GetValueOrDefault());

            var createBResult = m.Create(modelB) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createBResult.StatusCode.GetValueOrDefault());

            var createCResult = c.Create((int)createAResult.Value, (int)createBResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, createCResult.StatusCode);

            var queryCResult = c.Get((int)createAResult.Value, (int)createBResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, queryCResult.StatusCode);

        }

        [Fact]
        public void ContractTerminate()
        {
            var context = Common.CreateDbContext();

            API.Controllers.AdvisorController a = new API.Controllers.AdvisorController(context, null);
            API.Controllers.MGAController m = new API.Controllers.MGAController(context, null);
            API.Controllers.ContractController c = new API.Controllers.ContractController(context, null);

            var modelA = new API.Models.Pocos.Advisor
            {
                FirstName = "Bob",
                LastName = "Belcher",
                Address = "123 Ocean Avenue",
                HealthStatus = "GREEN",
                PhoneNumber = "133 3133 133"
            };

            var modelB = new API.Models.Pocos.MGA
            {
                BusinessName = "Bobs Burgers",
                BusinessAdress = "123 Ocean Avenue",
                BusinessPhoneNumber = "133 3133 133"
            };

            var createAResult = a.Create(modelA) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createAResult.StatusCode.GetValueOrDefault());

            var createBResult = m.Create(modelB) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createBResult.StatusCode.GetValueOrDefault());

            var createCResult = c.Create((int)createAResult.Value, (int)createBResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, createCResult.StatusCode);

            var deleteCResult = c.Delete((int)createAResult.Value, (int)createBResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, deleteCResult.StatusCode);

        }

        [Fact]
        public void ContractShortestPath()
        {
            //   (A)----(B)----(C)
            //    '-------------'         

            var context = Common.CreateDbContext();

            API.Controllers.AdvisorController a = new API.Controllers.AdvisorController(context, null);
            API.Controllers.CarrierController r = new API.Controllers.CarrierController(context, null);
            API.Controllers.MGAController m = new API.Controllers.MGAController(context, null);
            API.Controllers.ContractController c = new API.Controllers.ContractController(context, null);

            var modelA = new API.Models.Pocos.Advisor
            {
                FirstName = "A"
            };

            var modelB = new API.Models.Pocos.MGA
            {
                BusinessName = "B",

            };

            var modelC = new API.Models.Pocos.Carrier
            {
                BusinessName = "C",

            };

            var createAResult = a.Create(modelA) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createAResult.StatusCode.GetValueOrDefault());

            var createBResult = m.Create(modelB) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createBResult.StatusCode.GetValueOrDefault());

            var createCResult = r.Create(modelC) as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, createCResult.StatusCode.GetValueOrDefault());

            //A-B Contract
            var createContractAResult = c.Create((int)createAResult.Value, (int)createBResult.Value) as IStatusCodeActionResult;
            Assert.Equal((int)HttpStatusCode.OK, createContractAResult.StatusCode);

            //B-C Contract
            var createContractBResult = c.Create((int)createBResult.Value, (int)createCResult.Value) as IStatusCodeActionResult;
            Assert.Equal((int)HttpStatusCode.OK, createContractBResult.StatusCode);

            //A-C Contract
            var createContractCResult = c.Create((int)createAResult.Value, (int)createCResult.Value) as IStatusCodeActionResult;
            Assert.Equal((int)HttpStatusCode.OK, createContractCResult.StatusCode);

            var smallestChainCResult = c.SmallestChain((int)createAResult.Value, (int)createCResult.Value) as ObjectResult;
            var items = (IEnumerable<API.Models.Pocos.EntitySimple>)smallestChainCResult.Value;

            //should be A-C
            Assert.Equal(new[] { "A", "C" }, items.Select(n => n.Name).ToArray());

        }

    }
}
