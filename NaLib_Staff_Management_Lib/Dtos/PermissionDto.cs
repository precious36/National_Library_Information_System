using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Dtos
{
    public class PermissionDto
    {
        public Guid PermissionId { get; set; }
        public string Resource { get; set; } = null!;
        
    }
}
