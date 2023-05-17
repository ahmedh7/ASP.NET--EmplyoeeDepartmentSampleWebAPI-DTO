using EmployeeDep.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDep.DAL.Data.Context
{
    public class CompanyContext : IdentityDbContext<Employee>
    {
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();

        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Base Implementation must exist for IdentityDBContext inheritance
            // (Check OnModelCreating method definiton)
            base.OnModelCreating(modelBuilder);

            var departments = new List<Department>()
            {
                new Department{Id = 1 , Budget = 50000 , Name = "IT" ,  Evaluation = 5},
                new Department{Id = 2 , Budget = 50000 , Name = "IT2" ,  Evaluation = 4},
                new Department{Id = 3 , Budget = 50000 , Name = "IT3" ,  Evaluation = 3},
            };

            var employees = new List<Employee>()
            {
                new Employee{Id = 1 , Name = "Ahmed" , DepartmentId = 1},
                new Employee{Id = 2 , Name = "Mohamed" , DepartmentId = 2},
                new Employee{Id = 3 , Name = "Hamada" , DepartmentId = 3},
            };

            modelBuilder.Entity<Department>().HasData(departments);
            modelBuilder.Entity<Employee>().HasData(employees);

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("EmployeeClaims");
        }
    }
}
