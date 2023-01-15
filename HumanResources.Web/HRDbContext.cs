

using HumanResources.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using HumanResources.Web.Models;

namespace HumanResources.Web.Models
{
    public class HRDbContext:DbContext
    {
        //public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations{ get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\ mssqllocaldb;Initial Catalog = HumanResources; Integrated Security = True");
        }
      

    }
}
