using Microsoft.EntityFrameworkCore;
using PhoenixAuth.BusinessLogic.BusinessEntities.Models.Model;

namespace PhoenixAuth.BusinessLogic.BusinessEntities
{
    public class PhoenixAuthDbContext : DbContext
    {
        public PhoenixAuthDbContext(DbContextOptions<PhoenixAuthDbContext> options) : base(options)
        {
            //var kls = new DbContextOptionsBuilder(options).EnableSensitiveDataLogging();
        }

        public virtual DbSet<PartnerAuth> PartnerAuths { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(PartnerAuth).Assembly); // Here UseConfiguration is any IEntityTypeConfiguration
            //builder.Entity<PpAgent>().ToTable("PpAgent", t => t.ExcludeFromMigrations());
        }
    }
}
