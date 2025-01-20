using System;

namespace NaLib_Membership_and_Lending_Lib.Dto
{
    public class LendingPreferenceDto
    {
        public int PreferenceId { get; set; }
        public int MemberId { get; set; }
        public string ResourceType { get; set; }
        public string Topic { get; set; }
    }
}