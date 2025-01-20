using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Dtos
{
    public class RolePermissionDto
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
