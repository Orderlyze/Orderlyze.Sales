namespace WebApi.Mediator.Requests.WixContacts;

/// <summary>
/// Simplified DTO for Wix contact data.
/// </summary>
public class WixContactDto
{
    /// <summary>
    /// Gets or sets the contact ID.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the primary email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the primary phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Gets or sets the contact labels.
    /// </summary>
    public List<string> Labels { get; set; } = new();

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
}