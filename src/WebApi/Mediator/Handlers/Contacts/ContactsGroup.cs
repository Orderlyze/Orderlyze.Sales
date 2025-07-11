using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModels.Dtos.Contacts;
using Shiny.Mediator;
using WebApi.Constants;
using WebApi.Data;
using WebApi.Mediator.Requests.Contacts;
using DbModels = WebApi.Data.Models;

namespace WebApi.Mediator.Handlers.Contacts
{
    [MediatorHttpGroup(GroupConstants.Contact, RequiresAuthorization = true)]
    internal class ContactsGroup(AppDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        : IRequestHandler<AddContactRequest, ContactDto>,
            IRequestHandler<GetContactRequest, ContactDto?>,
            IRequestHandler<GetAllContactsRequest, IEnumerable<ContactDto>>,
            IRequestHandler<DeleteContactRequest, bool>,
            IRequestHandler<UpdateCallStatusRequest, ContactDto>,
            IRequestHandler<RescheduleCallRequest, ContactDto>
    {
        [MediatorHttpPost($"{GroupConstants.AddPrefix}{GroupConstants.Contact}", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            AddContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = userId != null ? await userManager.FindByNameAsync(userId) : null;
            
            var contact = new DbModels.Contact
            {
                WixId = request.WixId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Branche = request.Branche,
                UserId = user?.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CallStatus = SharedModels.Dtos.Contacts.CallStatus.New,
                NextCallDate = request.SetInitialCallDate ? DateTime.Today : null
            };

            db.Contacts.Add(contact);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return MapToDto(contact);
        }

        [MediatorHttpGet($"{GroupConstants.GetPrefix}{GroupConstants.Contact}", GroupConstants.IdTemplate)]
        public async Task<ContactDto?> Handle(
            GetContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            // For now, use a simple ID mapping until we fix the ID type issue
            var contacts = await db.Contacts
                .Include(c => c.CallHistory)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            
            var contact = contacts.FirstOrDefault(c => c.Id.ToString() == request.Id.ToString());
            return contact != null ? MapToDto(contact) : null;
        }

        [MediatorHttpGet($"{GroupConstants.GetAllPrefix}{GroupConstants.Contact}", GroupConstants.NoTemplate)]
        public async Task<IEnumerable<ContactDto>> Handle(
            GetAllContactsRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = userId != null ? await userManager.FindByNameAsync(userId) : null;
            
            var contacts = await db.Contacts
                .Where(c => c.UserId == user!.Id)
                .Include(c => c.CallHistory)
                .OrderBy(c => c.NextCallDate)
                .ThenBy(c => c.Name)
                .ToListAsync(ct)
                .ConfigureAwait(false);
                
            return contacts.Select(MapToDto);
        }

        [MediatorHttpDelete($"{GroupConstants.DeletePrefix}{GroupConstants.Contact}", GroupConstants.IdTemplate)]
        public async Task<bool> Handle(
            DeleteContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var contacts = await db.Contacts.ToListAsync(ct).ConfigureAwait(false);
            var entity = contacts.FirstOrDefault(c => c.Id.ToString() == request.Id.ToString());
                
            if (entity != null)
            {
                db.Contacts.Remove(entity);
                await db.SaveChangesAsync(ct).ConfigureAwait(false);
                return true;
            }
            
            return false;
        }

        [MediatorHttpPost("updateCallStatus", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            UpdateCallStatusRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = userId != null ? await userManager.FindByNameAsync(userId) : null;
            
            var contact = await db.Contacts
                .Include(c => c.CallHistory)
                .FirstOrDefaultAsync(x => x.Id == request.ContactId, ct)
                .ConfigureAwait(false);
            
            if (contact == null)
            {
                throw new InvalidOperationException("Contact not found");
            }

            // Create call log entry
            var callLog = new DbModels.CallLog
            {
                ContactId = contact.Id,
                CallDate = DateTime.UtcNow,
                Notes = request.Notes,
                Status = (SharedModels.Dtos.Contacts.CallStatus)request.Status,
                NextCallDate = request.NextCallDate,
                CreatedAt = DateTime.UtcNow
            };
            
            contact.CallHistory.Add(callLog);
            contact.LastCallDate = DateTime.UtcNow;
            contact.CallStatus = (SharedModels.Dtos.Contacts.CallStatus)request.Status;
            contact.CallNotes = request.Notes;
            
            if (request.RescheduleCall && request.NextCallDate.HasValue)
            {
                contact.NextCallDate = request.NextCallDate.Value;
            }
            else if (request.Status == CallStatus.Completed)
            {
                contact.NextCallDate = null;
            }
            else if (request.Status == CallStatus.NotReached)
            {
                // Use user's default callback days
                contact.NextCallDate = DateTime.Today.AddDays(user?.DefaultCallbackDays ?? 3);
            }
            
            contact.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            
            return MapToDto(contact);
        }

        [MediatorHttpPost("rescheduleCall", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            RescheduleCallRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = userId != null ? await userManager.FindByNameAsync(userId) : null;
            
            var contact = await db.Contacts
                .Include(c => c.CallHistory)
                .FirstOrDefaultAsync(x => x.Id == request.ContactId, ct)
                .ConfigureAwait(false);
            
            if (contact == null)
            {
                throw new InvalidOperationException("Contact not found");
            }

            // Set new call date
            if (request.NewCallDate.HasValue)
            {
                contact.NextCallDate = request.NewCallDate.Value;
            }
            else
            {
                // Use user's default callback days
                contact.NextCallDate = DateTime.Today.AddDays(user?.DefaultCallbackDays ?? 3);
            }
            
            contact.CallStatus = SharedModels.Dtos.Contacts.CallStatus.Postponed;
            contact.UpdatedAt = DateTime.UtcNow;
            
            // Add to call history
            var callLog = new DbModels.CallLog
            {
                ContactId = contact.Id,
                CallDate = DateTime.UtcNow,
                Notes = $"Verschoben: {request.Reason ?? "Kein Grund angegeben"}",
                Status = SharedModels.Dtos.Contacts.CallStatus.Postponed,
                NextCallDate = contact.NextCallDate,
                CreatedAt = DateTime.UtcNow
            };
            
            contact.CallHistory.Add(callLog);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            
            return MapToDto(contact);
        }

        private ContactDto MapToDto(DbModels.Contact contact)
        {
            return new ContactDto(
                contact.WixId,
                contact.Name,
                contact.Email,
                contact.Phone,
                contact.Branche,
                contact.NextCallDate,
                contact.CallNotes,
                (CallStatus)contact.CallStatus,
                contact.LastCallDate,
                contact.CallHistory?.Select(h => new CallLogEntry(
                    h.CallDate,
                    h.Notes,
                    (CallStatus)h.Status,
                    h.NextCallDate
                )).ToList()
            )
            {
                Id = Guid.NewGuid(),
                CreatedAt = contact.CreatedAt,
                UpdatedAt = contact.UpdatedAt
            };
        }
    }
}
