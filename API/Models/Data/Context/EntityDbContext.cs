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

                for (int i = 0; i < 10; i++)
                {
                    var advisor = new Advisor() { FirstName = "AdvisorZ"+i.ToString(), LastName = "", Address = "133 Orange Lane", PhoneNumber = "(647) 5555 4425" };
                    var carrier = new Carrier() { BusinessName = "CarrierX" + i.ToString(), BusinessAdress = "123 Purple Street", BusinessPhoneNumber = "(647) 5555 1234" };
                    var mga = new MGA() { BusinessName = "MGAY" + i.ToString(), BusinessAdress = "789 Green Road", BusinessPhoneNumber = "(647) 5555 5556" };

                    //add entities
                    context.Advisors.Add(advisor);
                    context.Carriers.Add(carrier);
                    context.MGAs.Add(mga);

                    //add contracts
                    context.Contracts.Add(new Contract { Entity1 = carrier, Entity2 = mga }); // CarrierX-MGAY
                    context.Contracts.Add(new Contract { Entity1 = mga, Entity2 = advisor }); // MGAY-AdvisorZ
                }

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
