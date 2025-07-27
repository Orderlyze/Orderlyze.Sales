using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SharedModels.Dtos.Contacts;

namespace WebApi.Data.Models
{
    /// <summary>
    /// Represents a call log entry in the database.
    /// </summary>
    [Index(nameof(ContactId), nameof(CallDate))]
    [Index(nameof(CallDate))]
    public class CallLog
    {
        /// <summary>
        /// Gets or sets the primary key identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the foreign key to the associated contact.
        /// </summary>
        [Required]
        [ForeignKey(nameof(Contact))]
        public int ContactId { get; set; }
        
        /// <summary>
        /// Gets or sets the navigation property to the contact.
        /// </summary>
        public virtual Contact Contact { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the date and time when the call was made.
        /// </summary>
        [Required]
        public DateTime CallDate { get; set; }
        
        /// <summary>
        /// Gets or sets notes or comments about the call.
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the outcome status of the call.
        /// </summary>
        [Required]
        public CallStatus Status { get; set; }
        
        /// <summary>
        /// Gets or sets the next scheduled call date, if applicable.
        /// </summary>
        public DateTime? NextCallDate { get; set; }
        
        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}