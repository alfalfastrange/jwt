using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Jwt.Entity.Entities;

namespace Jwt.Repository.Contexts
{
    public class ModelContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Profile>()
                .Ignore(p => p.CreatedBy);
        }

        private void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}