using Microsoft.EntityFrameworkCore;
using SharedModels.Dtos.Contacts;
using Shiny.Mediator;
using WebApi.Constants;
using WebApi.Data;
using WebApi.Mediator.Requests.Contacts;

namespace WebApi.Mediator.Handlers.Contacts
{
    [MediatorHttpGroup(GroupConstants.Contact, RequiresAuthorization = true)]
    public class ContactsGroup
        : IRequestHandler<AddContactRequest, ContactDto>,
            IRequestHandler<GetContactRequest, ContactDto?>,
            IRequestHandler<GetAllContactsRequest, IEnumerable<ContactDto>>,
            IRequestHandler<DeleteContactRequest, bool>
    {
        private readonly AppDbContext _db;

        public ContactsGroup(AppDbContext db) => _db = db;

        [MediatorHttpPost($"{GroupConstants.AddPrefix}{GroupConstants.Contact}", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            AddContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var contact = new ContactDto(
                request.WixId,
                request.Name,
                request.Email,
                request.Phone,
                request.Branche
            )
            {
                Id = Guid.NewGuid(),
            };

            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync(ct);
            return contact;
        }

        [MediatorHttpGet($"{GroupConstants.GetPrefix}{GroupConstants.Contact}", GroupConstants.IdTemplate)]
        public async Task<ContactDto?> Handle(
            GetContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            return await _db.Contacts.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        }

        [MediatorHttpGet($"{GroupConstants.GetAllPrefix}{GroupConstants.Contact}", GroupConstants.NoTemplate)]
        public async Task<IEnumerable<ContactDto>> Handle(
            GetAllContactsRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            return await _db.Contacts.ToListAsync(ct);
        }

        [MediatorHttpDelete($"{GroupConstants.DeletePrefix}{GroupConstants.Contact}", GroupConstants.IdTemplate)]
        public async Task<bool> Handle(
            DeleteContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var entity = await _db.Contacts.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (entity != null)
            {
                _db.Contacts.Remove(entity);
                await _db.SaveChangesAsync(ct);
                return true!;
            }
            else
            {
                return false;
            }
        }
    }
}
