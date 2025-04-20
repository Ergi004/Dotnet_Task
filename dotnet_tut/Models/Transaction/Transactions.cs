using System;

namespace dotnet_tut.Models
{


    public class Transactions
    {
        public int Id { get; set; }
        public int Amount {get; set;}
        public string Description { get; set; } 
        public string CustomerFullName { get; set; }
        public string CustomerMainPhoneNumber { get; set; }
        public TransactionTypeEnum TransactionType { get; set; } 
        public TransactionStatus Status { get; set; } 
        public string CustomerMainEmailAddress { get; set; }
        public string CustomerMainAddress { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}