using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestProject1
{
    public static class Common
    {
        public static API.Models.Data.Context.EntityDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<API.Models.Data.Context.EntityDbContext> optionsBuilder = new DbContextOptionsBuilder<API.Models.Data.Context.EntityDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "Entities");

           return new API.Models.Data.Context.EntityDbContext(optionsBuilder.Options);
        }

    }
}
