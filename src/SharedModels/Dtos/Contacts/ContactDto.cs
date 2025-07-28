using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedModels.Dtos.Common;

namespace SharedModels.Dtos.Contacts
{
    /// <summary>
    /// Represents a contact with call tracking information.
    /// </summary>
    /// <param name="WixId">The unique Wix identifier for the contact.</param>
    /// <param name="Name">The full name of the contact.</param>
    /// <param name="Email">The email address of the contact.</param>
    /// <param name="Phone">The phone number of the contact.</param>
    /// <param name="Industry">The industry or business sector of the contact.</param>
    /// <param name="NextCallDate">The scheduled date for the next call. Optional.</param>
    /// <param name="CallNotes">Notes from the most recent call. Optional.</param>
    /// <param name="CallStatus">The current status of the contact in the call workflow. Defaults to New.</param>
    /// <param name="LastCallDate">The date of the last call made to this contact. Optional.</param>
    /// <param name="CallHistory">A list of all previous call log entries. Optional.</param>
    public record ContactDto(
        [Required(ErrorMessage = "WixId is required")]
        string WixId, 
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        string Name, 
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
        string Email, 
        
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        string Phone, 
        
        [Required(ErrorMessage = "Industry is required")]
        [StringLength(100, ErrorMessage = "Industry cannot exceed 100 characters")]
        string Industry,
        
        DateTime? NextCallDate = null,
        
        [StringLength(1000, ErrorMessage = "Call notes cannot exceed 1000 characters")]
        string? CallNotes = null,
        
        CallStatus CallStatus = CallStatus.New,
        
        DateTime? LastCallDate = null,
        
        List<CallLogEntry>? CallHistory = null
    ) : BaseDto;
}
