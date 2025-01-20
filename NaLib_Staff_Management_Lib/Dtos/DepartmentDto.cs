using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Dtos
{
    public class DepartmentDto
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; } = null!;
    }

}
