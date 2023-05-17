
using EmployeeDep.BL.Dtos.Department;
using EmployeeDep.BL.Managers.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmplyoeeDep.API_day2.Controllers
{
    // ClaimsPrincipal can be accessed from any authorized method
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsManager _departmentsManager;

        public DepartmentsController(IDepartmentsManager departmentsManager) { 
            _departmentsManager = departmentsManager;
        }


        [HttpGet]
        public ActionResult<List<DepartmentReadDto>> GetAll()
        {
            return _departmentsManager.GetAll();
        }

        [HttpPut]
        public ActionResult Update(DepartmentUpdateDto DepartmentDto)
        {
            bool isFound = _departmentsManager.Update(DepartmentDto);
            if (!isFound)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "ManagersOnly")]
        public ActionResult Delete(int id)
        {
            bool isFound = _departmentsManager.Delete(id);
            if (!isFound)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet]
        [Route("WithEmployees/{id}")]
        [Authorize(Policy = "ManagersOnly")]
        public ActionResult<DepartmentWithEmployeesReadDto> GetDepartmentByIdWithEmployees(int id) {
            DepartmentWithEmployeesReadDto? dept = _departmentsManager.GetWithEmployees(id); 
            if (dept == null)   return  NotFound();
            return dept;
        }
    }
}
