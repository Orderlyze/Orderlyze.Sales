using WixApi.Apis;
using WixApi.Models;

namespace WixApi.Repositories
{
    public class WixContactsRepository : BaseWixRepository<IWixContactsApi>, IWixContactsRepository
    {
        public WixContactsRepository(IHttpClientFactory clientFactory)
            : base(clientFactory) { }

        public async Task<List<WixContact>> GetContactsAsync(int limit = 20)
        {
            var serverResult = await this.TryRequest(
                async () => await RepositoryApi.ListContacts(limit).ConfigureAwait(false)
            );
            var result = System.Text.Json.JsonSerializer.Deserialize<ContactResponse>(serverResult);

            return result.contacts;
        }

        public async Task<WixContact> GetContactsByEmailAsync(string email)
        {
            var result = await this.TryRequest(
                async () =>
                    await RepositoryApi.ContactQuery(
                        new ContactQuery()
                        {
                            Query = new Query()
                            {
                                Filter = new Filter() { InfoEmailsEmail = email },
                            },
                        }
                    ).ConfigureAwait(false)
            );

            return result.contacts.FirstOrDefault();
        }

        public async Task AddLabelContactAsync(string contactId, string[] labelKeys)
        {
            await this.RepositoryApi.AddLabelContact(
                contactId,
                new AddLabelPayload { labelKeys = labelKeys }
            ).ConfigureAwait(false);
        }

        public async Task UpdateContact(string contactId, WixContact info)
        {
            await this.RepositoryApi.UpdateContact(contactId, info).ConfigureAwait(false);
        }

        public async Task DeleteLabelContactAsync(string contactId, string[] labelKeys)
        {
            await this.RepositoryApi.DeleteLabelContact(
                contactId,
                new AddLabelPayload { labelKeys = labelKeys }
            ).ConfigureAwait(false);
        }
    }
}
