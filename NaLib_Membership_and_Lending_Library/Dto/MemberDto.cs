using System;

namespace NaLib_Membership_and_Lending_Lib.Dto
{
    public class MemberDto
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PostalAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public DateOnly DateEnrolled { get; set; }
        public string Status { get; set; }
    }
}