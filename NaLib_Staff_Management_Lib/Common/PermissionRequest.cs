using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Common
{
    public class PermissionRequest
    {
        [Required]
        [StringLength(200)]
        public string Resource { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Action { get; set; } = null!;
    }

}
