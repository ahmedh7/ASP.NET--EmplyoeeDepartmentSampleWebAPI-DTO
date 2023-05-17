using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDep.BL.Dtos.Department
{
    public class DepartmentAddDto
    {
        public string Name { get; set; } = string.Empty;
        public double Budget { get; set; }
    }
}
