using EmployeeDep.BL.Dtos.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDep.BL.Managers.Departments
{
    public  interface IDepartmentsManager
    {
        List<DepartmentReadDto> GetAll();
        DepartmentReadDto? GetById(int id);
        int Add(DepartmentAddDto DepartmentDto);
        bool Update(DepartmentUpdateDto DepartmentDto);
        bool Delete(int id);
        DepartmentWithEmployeesReadDto? GetWithEmployees(int id);
    }
}
