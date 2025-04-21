using System;
using System.Text.Json.Serialization;

namespace dotnet_tut.Models
{


    public class Transactions
    {
        public int Id { get; set; }
        public int Amount {get; set;}
        public string Description { get; set; } 
        public TransactionTypeEnum TransactionType { get; set; } 
        public TransactionStatus Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }  

        [JsonIgnore]
        public Customer Customer { get; set; }

    }
}