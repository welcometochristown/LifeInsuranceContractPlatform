using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using Xunit;

namespace XUnitTestProject1
{
    public class EntityTest
    {
        /// <summary>
        /// Test Creating an Advisor
        /// </summary>
        [Fact]
        public void AdvisorCRUD()
        {
            API.Controllers.AdvisorController c = new API.Controllers.AdvisorController(Common.CreateDbContext(), null);
            var model = new API.Models.Pocos.Advisor
            {
                FirstName = "Bob",
                LastName = "Belcher",
                Address = "123 Ocean Avenue",
                HealthStatus = "GREEN",
                PhoneNumber = "133 3133 133"
            };

            var createResult = c.Create(model) as ObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, createResult.StatusCode.GetValueOrDefault());

            var readResult = c.Get((int)createResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, readResult.StatusCode);

            model.ID = (int)createResult.Value;
            model.FirstName = "Linda";

            var updateResult = c.Update(model) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, updateResult.StatusCode);

            var deleteResult = c.Delete((int)createResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, deleteResult.StatusCode);

        }

        /// <summary>
        /// Test Creating an Carrier
        /// </summary>
        [Fact]
        public void CarrierCRUD()
        {
            API.Controllers.CarrierController c = new API.Controllers.CarrierController(Common.CreateDbContext(), null);
            var model = new API.Models.Pocos.Carrier
            {
                BusinessName = "Bobs Burgers",
                BusinessAdress = "123 Ocean Avenue",
                BusinessPhoneNumber = "133 3133 133"
            };

            var createResult = c.Create(model) as ObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, createResult.StatusCode.GetValueOrDefault());

            var readResult = c.Get((int)createResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, readResult.StatusCode);

            model.ID = (int)createResult.Value;
            model.BusinessName = "Bob's Burgers";

            var updateResult = c.Update(model) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, updateResult.StatusCode);

            var deleteResult = c.Delete((int)createResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, deleteResult.StatusCode);

        }

        /// <summary>
        /// Test Creating an MGA
        /// </summary>
        [Fact]
        public void MGACRUD()
        {
            API.Controllers.MGAController c = new API.Controllers.MGAController(Common.CreateDbContext(), null);
            var model = new API.Models.Pocos.MGA
            {
                BusinessName = "Bobs Burgers",
                BusinessAdress = "123 Ocean Avenue",
                BusinessPhoneNumber = "133 3133 133"
            };

            var createResult = c.Create(model) as ObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, createResult.StatusCode.GetValueOrDefault());

            var readResult = c.Get((int)createResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, readResult.StatusCode);

            model.ID = (int)createResult.Value;
            model.BusinessName = "Bob's Burgers";

            var updateResult = c.Update(model) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, updateResult.StatusCode);

            var deleteResult = c.Delete((int)createResult.Value) as IStatusCodeActionResult;

            Assert.Equal((int)HttpStatusCode.OK, deleteResult.StatusCode);

        }

    }
}
