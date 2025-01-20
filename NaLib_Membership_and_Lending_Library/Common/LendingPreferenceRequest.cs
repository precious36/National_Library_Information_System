using System;

namespace NaLib_Membership_and_Lending_Lib.Common
{
    public class LendingPreferenceRequest
    {
        public int MemberId { get; set; }
        public string ResourceType { get; set; }
        public string Topic { get; set; }
    }
}