using System;
using SharedModels.Dtos.Contacts;

namespace WebApi.Data.Models
{
    public class CallLog
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
        
        public DateTime CallDate { get; set; }
        public string Notes { get; set; } = "";
        public CallStatus Status { get; set; }
        public DateTime? NextCallDate { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}