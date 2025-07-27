using System.ComponentModel.DataAnnotations;
using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    /// <summary>
    /// Request to add a new contact to the system.
    /// </summary>
    internal class AddContactRequest : IRequest<ContactDto>
    {
        /// <summary>
        /// Gets or sets the unique Wix identifier for the contact.
        /// </summary>
        [Required(ErrorMessage = "WixId is required")]
        [StringLength(100, ErrorMessage = "WixId cannot exceed 100 characters")]
        public string WixId { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's full name.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's phone number.
        /// </summary>
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the contact's industry or business sector.
        /// </summary>
        [Required(ErrorMessage = "Industry is required")]
        [StringLength(100, ErrorMessage = "Industry cannot exceed 100 characters")]
        public string Industry { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets whether to set the initial call date to today.
        /// </summary>
        public bool SetInitialCallDate { get; set; } = true;
    }
}
