using System;
using System.Collections.Generic;
using SharedModels.Dtos.Contacts;

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
}