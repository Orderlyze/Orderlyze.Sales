using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Dtos.Contacts
{
    /// <summary>
    /// Represents a single entry in a contact's call history.
    /// </summary>
    /// <param name="CallDate">The date and time when the call was made.</param>
    /// <param name="Notes">Notes or comments about the call.</param>
    /// <param name="Status">The outcome status of the call.</param>
    /// <param name="NextCallDate">The next scheduled call date, if applicable. Optional.</param>
    public record CallLogEntry(
        [Required(ErrorMessage = "Call date is required")]
        DateTime CallDate,
        
        [Required(ErrorMessage = "Notes are required")]
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        string Notes,
        
        [Required(ErrorMessage = "Status is required")]
        CallStatus Status,
        
        DateTime? NextCallDate = null
    );
}