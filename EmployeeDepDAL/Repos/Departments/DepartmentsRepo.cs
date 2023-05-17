using EmployeeDep.DAL.Data.Context;
using EmployeeDep.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDep.DAL.Repos.Departments
{
    public class DepartmentsRepo : IDepartmentsRepo
    {
        private CompanyContext _context;

        public DepartmentsRepo(CompanyContext context) {
            _context = context;
        }
        public void Add(Department entity)
        {
            _context.Set<Department>().Add(entity);
        }

        public void Delete(Department entity)
        {
            _context.Set<Department>().Remove(entity);
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Set<Department>();
        }

        public Department? GetById(int id)
        {
            return _context.Set<Department>().Find(id);
        }

        public Department? GetByIdWithEmployees(int id)
        {
            return (_context.Set<Department>()
                .Include(d => d.Employees).FirstOrDefault(d => d.Id == id));
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Update(Department entity)
        {
            _context.Set<Department>().Update(entity);
        }
    }
}
