using BootStrap.Models;
using Microsoft.EntityFrameworkCore;

namespace Bootstrap_3.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Project>Projects { get; set; }
    }
}
