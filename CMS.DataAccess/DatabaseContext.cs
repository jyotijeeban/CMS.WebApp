using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.DataAccess
{
    public class DatabaseContext:IdentityDbContext<User,Role,int>
    {
        //only for migration
        public DatabaseContext()
        {

        }
        //setting form startup.cs file
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=CSMBHUL323\\SQL2012; initial catalog=CMS.ASPCORE;persist security info=True;user id=sa;password=csmpl@123;");
            }
            base.OnConfiguring(optionsBuilder);
        }

    }
}
