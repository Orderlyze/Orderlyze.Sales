namespace SharedModels.Dtos.Wix
{
    public record WixContactDto(
        string Id,
        string? FirstName,
        string? LastName,
        string? Email,
        string? Phone,
        string? Address,
        string? Company,
        string[] LabelKeys,
        DateTime? CreatedDate = null,
        DateTime? UpdatedDate = null
    );
}