using System;
using System.Collections.Generic;

namespace WebApi.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string WixId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Branche { get; set; } = "";
        
        // Call tracking fields
        public DateTime? NextCallDate { get; set; }
        public string? CallNotes { get; set; }
        public CallStatus CallStatus { get; set; } = CallStatus.New;
        public DateTime? LastCallDate { get; set; }
        
        // Navigation property
        public List<CallLog> CallHistory { get; set; } = new();
        
        // Tracking
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }

    public enum CallStatus
    {
        New,
        Scheduled,
        Reached,
        NotReached,
        Completed,
        Postponed
    }

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