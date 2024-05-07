using ManageInformation.Model;
using Microsoft.AspNetCore.Mvc;
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
