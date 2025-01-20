using System;

namespace NaLib_Membership_and_Lending_Lib.Common
{
    public class LendingTransactionRequest
    {
        public int MemberId { get; set; }
        public int ResourceId { get; set; }
        public DateOnly CheckoutDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string? Status { get; set; }
    }
}