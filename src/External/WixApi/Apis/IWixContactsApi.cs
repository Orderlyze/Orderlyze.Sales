using Refit;
using WixApi.Models;

namespace WixApi.Apis
{
    public interface IWixContactsApi
    {
        [Get("/contacts/v4/contacts?sort.fieldName=updatedDate&sort.order=DESC")]
        Task<string> ListContacts([AliasAs("paging.limit")] int limit = 20);

        [Post("/contacts/v4/contacts/{contactId}/labels")]
        Task AddLabelContact(string contactId, [Body] AddLabelPayload labelKeys);

        [Patch("/contacts/v4/contacts/{contactId}")]
        Task UpdateContact(string contactId, [Body] WixContact labelKeys);

        [Delete("/contacts/v4/contacts/{contactId}/labels")]
        Task DeleteLabelContact(string contactId, [Body] AddLabelPayload labelKeys);

        [Post("/contacts/v4/contacts/query")]
        Task<ContactResponse> ContactQuery([Body] ContactQuery contactQuery);
    }
}
