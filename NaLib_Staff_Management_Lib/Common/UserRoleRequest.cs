using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Common
{
    public class UserRoleRequest
    {

        
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        
        public string Description { get; set; }
    }
}
