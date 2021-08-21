using System;
using System.ComponentModel.DataAnnotations;

namespace Server
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Coin { get; set; }
    }

    public static class TransactionState
    {
        public const string Pending = "Pending";
        public const string Complete = "Complete";
        public const string Cancelled = "Cancelled"; 
    }
    public static class TransactionType
    {
        public const string Send = "Send";
        public const string Receive = "Receive";
        public const string Swap = "Swap";
    }
}