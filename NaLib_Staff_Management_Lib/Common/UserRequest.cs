using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Common
{
    public class UserRequest
    {
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid LibraryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = null!;
    }

}
