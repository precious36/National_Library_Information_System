using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Common
{
    public class UserDetailRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Qualification { get; set; } = null!;

        [Required]
        public long Experience { get; set; }

        [Required]
        public string Skills { get; set; } = null!;

        [Required]
        public DateOnly EmploymentDate { get; set; }
    }

}
