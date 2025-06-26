using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using WixApi.Constants;
using WixApi.Models;

namespace WixApi.Apis
{
    [Headers(
        $"Authorization: {WixConstants.ApiKey}",
        $"wix-account-id: {WixConstants.AccountId}",
        $"wix-site-id: {WixConstants.SiteId}"
    )]
    public interface IContactsApi
    {
        [Get("/contacts/v4/contacts?sort.fieldName=updatedDate&sort.order=DESC&paging.Limit=200")]
        Task<string> ListContacts();

        [Post("/contacts/v4/contacts/{contactId}/labels")]
        Task LabelContact(string contactId, [Body] AddLabelPayload labelKeys);
    }
}
