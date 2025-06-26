using WixApi.Models;

namespace WixApi.Repositories
{
    public interface IWixContactsRepository
    {
        Task<List<WixContact>> GetContactsAsync(int limit = 20);
        Task<WixContact> GetContactsByEmailAsync(string email);
        Task UpdateContact(string contactId, WixContact info);
        Task AddLabelContactAsync(string contactId, string[] labelKeys);
        Task DeleteLabelContactAsync(string contactId, string[] labelKeys);
    }
}
