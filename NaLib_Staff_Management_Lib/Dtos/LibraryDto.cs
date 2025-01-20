using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Staff_Management_Lib.Dtos
{
    public class LibraryDto
    {
        public Guid LibraryId { get; set; }
        public string Name { get; set; } = null!;
        public Guid LocationId { get; set; }
    }
}
