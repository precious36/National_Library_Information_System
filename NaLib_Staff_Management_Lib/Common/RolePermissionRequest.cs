using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Common
{
    public class RolePermissionRequest
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid PermissionId { get; set; }
    }

}
