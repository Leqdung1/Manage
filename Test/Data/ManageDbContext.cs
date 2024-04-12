using ManageInformation.Model;
using Microsoft.EntityFrameworkCore;

namespace ManageInformation.Data
{
    public class ManageDbContext : DbContext
    {
        public ManageDbContext(DbContextOptions<ManageDbContext> context) : base(context) 
        {
           
        }

        public DbSet<Manage> Information { get; set; }
    }
}
