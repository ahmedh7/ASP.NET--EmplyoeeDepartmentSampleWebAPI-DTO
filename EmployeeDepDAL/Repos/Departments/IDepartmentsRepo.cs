using EmployeeDep.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDep.DAL.Repos.Departments
{
    public interface IDepartmentsRepo
    {
        IEnumerable<Department> GetAll();
        Department? GetById(int id);
        void Add(Department entity);
        void Update(Department entity);
        void Delete(Department entity);
        int SaveChanges();
        Department? GetByIdWithEmployees(int id);
    }
}
