using EmployeeDep.BL.Dtos.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDep.DAL.Repos.Departments;
using EmployeeDep.DAL.Data.Models;

namespace EmployeeDep.BL.Managers.Departments
{
    public class DepartmentsManager : IDepartmentsManager
    {
        private readonly IDepartmentsRepo _depRepo;

        public DepartmentsManager(IDepartmentsRepo departmentsRepo) { 
            _depRepo = departmentsRepo;
        }
        public int Add(DepartmentAddDto DepartmentDto)
        {
            Department depToAdd = new Department
            {
                Name = DepartmentDto.Name,
                Budget = DepartmentDto.Budget,
            };
            _depRepo.Add(depToAdd);
            _depRepo.SaveChanges();
            //Id Retrieved
            return depToAdd.Id;
        }

        public bool Delete(int id)
        {
            Department? deptFromDb = _depRepo.GetById(id);
            if (deptFromDb == null) return false;

            _depRepo.Delete(deptFromDb);
            _depRepo.SaveChanges();
            return true;
        }

        public List<DepartmentReadDto> GetAll()
        {
            var deptsFromDb = _depRepo.GetAll();
            return deptsFromDb.Select(d => new DepartmentReadDto
            {
                Id = d.Id,  
                Name = d.Name,
                Evaluation = d.Evaluation,
            }).ToList();
        }

        public DepartmentReadDto? GetById(int id)
        {
            Department? deptFromDb = _depRepo.GetById(id);
            if(deptFromDb == null)
                return null;
            return new DepartmentReadDto
            {
                Id = deptFromDb.Id,
                Name = deptFromDb.Name,
                Evaluation = deptFromDb.Evaluation,
            };
        }

        public DepartmentWithEmployeesReadDto? GetWithEmployees(int id)
        {
            Department? department = _depRepo.GetById(id);
            if (department == null) return null;

            return new DepartmentWithEmployeesReadDto
            {
                Id = department.Id,
                Budget = department.Budget,
                Evaluation = department.Evaluation,
                Name = department.Name,

                Employees = department.Employees.Select(e => new EmployeeChildReadDto
                {
                    Id = e.Id,
                    Name = e.Name,
                }).ToList(),
            };


        }
        
        public bool Update(DepartmentUpdateDto DepartmentDto)
        {
            Department? deptFromDb = _depRepo.GetById(DepartmentDto.Id);
            if (deptFromDb == null) return false;


            deptFromDb.Name= DepartmentDto.Name;
            deptFromDb.Evaluation= DepartmentDto.Evaluation;
            deptFromDb.Budget= DepartmentDto.Budget;
            _depRepo.Update(deptFromDb);
            _depRepo.SaveChanges();
            return true;
        }
        

    }
}
