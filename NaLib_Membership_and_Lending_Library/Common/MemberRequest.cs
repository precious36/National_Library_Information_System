using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Membership_and_Lending_Lib.Common
{
    public class MemberRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PostalAddress { get; set; }
        public string PhysicalAddress { get; set; }
        [Required]
        public DateTime DateEnrolled { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
