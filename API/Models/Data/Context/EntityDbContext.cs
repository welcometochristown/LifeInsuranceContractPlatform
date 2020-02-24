using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Data.Context
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options)
        : base(options)
        {

        }

        /// <summary>
        /// Initialise with test data
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EntityDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<EntityDbContext>>()))
            {
                var advisors = new[]
                {
                    new Advisor() { FirstName = "John", LastName = "Smith", Address = "133 Orange Lane", PhoneNumber = "(647) 555 4425" },
                    new Advisor() { FirstName = "David", LastName = "Peterson", Address = "155 Yellow Road", PhoneNumber = "(647) 555 4425" },
                    new Advisor() { FirstName = "John", LastName = "Smith", Address = "33 Maroon Marsh", PhoneNumber = "(647) 555 4425" }
                };

                var carriers = new[]
                {
                    new Carrier() { BusinessName = "State Farm", BusinessAddress = "1 Purple Street", BusinessPhoneNumber = "(647) 555 1234" },
                    new Carrier() { BusinessName = "GEICO", BusinessAddress = "99 Red Lane", BusinessPhoneNumber = "(647) 555 1234" },
                    new Carrier() { BusinessName = "AlState", BusinessAddress = "5534 Blue Bay", BusinessPhoneNumber = "(647) 555 1234" }
                };

                var mgas = new[]
                {
                    new MGA() { BusinessName = "Techno Insurance", BusinessAddress = "87 Mauve  Street", BusinessPhoneNumber = "(647) 555 1234" },
                    new MGA() { BusinessName = "Big Insure", BusinessAddress = "1139 Brown Bay", BusinessPhoneNumber = "(647) 555 1234" },
                    new MGA() { BusinessName = "Special Insurance Co.", BusinessAddress = "13 Green Grove", BusinessPhoneNumber = "(647) 555 1234" }
                };

                context.Advisors.AddRange(advisors);
                context.Carriers.AddRange(carriers);
                context.MGAs.AddRange(mgas);

                context.Contracts.Add(new Contract { Entity1 = advisors[0], Entity2 = carriers[0] }); 
                context.Contracts.Add(new Contract { Entity1 = carriers[1], Entity2 = mgas[1] });
                context.Contracts.Add(new Contract { Entity1 = carriers[2], Entity2 = advisors[2] });

                //save changes
                context.SaveChanges();
            }
        }

        public DbSet<Models.Data.Entity> Entities { get; set; }
        public DbSet<Models.Data.Advisor> Advisors { get; set; }
        public DbSet<Models.Data.Carrier> Carriers { get; set; }
        public DbSet<Models.Data.MGA> MGAs { get; set; }
        public DbSet<Models.Data.Contract> Contracts { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        /// <summary>
        /// Configure dbContext
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        /// <summary>
        /// Create dbContext model entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Models.Data.Entity>()
                .HasKey(n => n.ID);

            modelBuilder.Entity<Models.Data.Contract>()
               .HasKey(n => new { n.Entity1ID, n.Entity2ID });

            modelBuilder.Entity<Models.Data.Contract>()
                .HasOne(n => n.Entity1)
                .WithMany(n => n.Contracts1)
                .IsRequired()
                .HasForeignKey(n => n.Entity1ID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Data.Contract>()
                .HasOne(n => n.Entity2)
                .WithMany(n => n.Contracts2)
                .IsRequired()
                .HasForeignKey(n => n.Entity2ID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
