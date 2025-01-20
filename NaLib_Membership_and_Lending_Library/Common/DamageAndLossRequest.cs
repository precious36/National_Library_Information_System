using System;

namespace NaLib_Membership_and_Lending_Lib.Common
{
    public class DamageAndLossRequest
    {
        public int TransactionId { get; set; }
        public string DamageType { get; set; }
        public decimal ReplacementCost { get; set; }
    }
}