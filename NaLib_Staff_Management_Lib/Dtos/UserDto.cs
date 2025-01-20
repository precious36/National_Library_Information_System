using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public Guid RoleId { get; set; }
        public Guid LibraryId { get; set; }
    }

}
