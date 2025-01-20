using System;

namespace NaLib_Membership_and_Lending_Lib.Dto
{
    public class DamageAndLossDto
    {
        public int DamageId { get; set; }
        public int TransactionId { get; set; }
        public string DamageType { get; set; }
        public decimal ReplacementCost { get; set; }
    }
}