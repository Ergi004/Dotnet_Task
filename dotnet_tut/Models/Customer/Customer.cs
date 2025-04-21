using System;
using System.Collections.Generic;

namespace dotnet_tut.Models
{


    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string MainPhoneNumber { get; set; }
        public string MainEmailAddress { get; set; }
        public string MainAddress { get; set; } 
        public DateTime CreatedAt { get; set; }
        public ICollection<Transactions> Transactions { get; set; } 

    }
}