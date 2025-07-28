using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels.Dtos.Contacts;

namespace WebApi.Data.Models
{
    /// <summary>
    /// Represents a contact entity in the database.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the primary key identifier.
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the unique Wix identifier for the contact.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string WixId { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's full name.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's email address.
        /// </summary>
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's phone number.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's industry or business sector.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Industry { get; set; } = string.Empty;
        
        // Call tracking fields
        
        /// <summary>
        /// Gets or sets the scheduled date for the next call.
        /// </summary>
        public DateTime? NextCallDate { get; set; }
        
        /// <summary>
        /// Gets or sets notes from the most recent call.
        /// </summary>
        [StringLength(1000)]
        public string? CallNotes { get; set; }
        
        /// <summary>
        /// Gets or sets the current status in the call workflow.
        /// </summary>
        public CallStatus CallStatus { get; set; } = CallStatus.New;
        
        /// <summary>
        /// Gets or sets the date of the last call.
        /// </summary>
        public DateTime? LastCallDate { get; set; }
        
        // Navigation properties
        
        /// <summary>
        /// Gets or sets the collection of call history entries.
        /// </summary>
        public virtual ICollection<CallLog> CallHistory { get; set; } = new List<CallLog>();
        
        // Tracking and relationships
        
        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the last update timestamp.
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the foreign key to the user who owns this contact.
        /// </summary>
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the navigation property to the user.
        /// </summary>
        public virtual AppUser? User { get; set; }
    }
}