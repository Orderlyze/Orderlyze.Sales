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
    /// <summary>
    /// Handles all contact-related operations including CRUD and call management.
    /// </summary>
    [MediatorHttpGroup(GroupConstants.Contact, RequiresAuthorization = true)]
    internal sealed class ContactsGroup(AppDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        : IRequestHandler<AddContactRequest, ContactDto>,
            IRequestHandler<GetContactRequest, ContactDto?>,
            IRequestHandler<GetAllContactsRequest, IEnumerable<ContactDto>>,
            IRequestHandler<DeleteContactRequest, bool>,
            IRequestHandler<UpdateCallStatusRequest, ContactDto>,
            IRequestHandler<RescheduleCallRequest, ContactDto>
    {
        /// <summary>
        /// Adds a new contact to the system.
        /// </summary>
        /// <param name="request">The contact creation request.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created contact DTO.</returns>
        [MediatorHttpPost($"{GroupConstants.AddPrefix}{GroupConstants.Contact}", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            AddContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            
            var user = await userManager.FindByNameAsync(userId)
                ?? throw new InvalidOperationException($"User '{userId}' not found.");
            
            var contact = new DbModels.Contact
            {
                WixId = request.WixId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Industry = request.Industry,
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

        /// <summary>
        /// Retrieves a single contact by ID.
        /// </summary>
        /// <param name="request">The contact retrieval request.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The contact DTO if found; otherwise, null.</returns>
        [MediatorHttpGet($"{GroupConstants.GetPrefix}{GroupConstants.Contact}", GroupConstants.IdTemplate)]
        public async Task<ContactDto?> Handle(
            GetContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var contact = await db.Contacts
                .Include(c => c.CallHistory)
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct)
                .ConfigureAwait(false);
            
            return contact != null ? MapToDto(contact) : null;
        }

        /// <summary>
        /// Retrieves all contacts for the authenticated user.
        /// </summary>
        /// <param name="request">The request to get all contacts.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A collection of contact DTOs.</returns>
        [MediatorHttpGet($"{GroupConstants.GetAllPrefix}{GroupConstants.Contact}", GroupConstants.NoTemplate)]
        public async Task<IEnumerable<ContactDto>> Handle(
            GetAllContactsRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            
            var user = await userManager.FindByNameAsync(userId)
                ?? throw new InvalidOperationException($"User '{userId}' not found.");
            
            var contacts = await db.Contacts
                .Where(c => c.UserId == user.Id)
                .Include(c => c.CallHistory)
                .OrderBy(c => c.NextCallDate)
                .ThenBy(c => c.Name)
                .ToListAsync(ct)
                .ConfigureAwait(false);
                
            return contacts.Select(MapToDto);
        }

        /// <summary>
        /// Deletes a contact from the system.
        /// </summary>
        /// <param name="request">The contact deletion request.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the contact was deleted; otherwise, false.</returns>
        [MediatorHttpDelete($"{GroupConstants.DeletePrefix}{GroupConstants.Contact}", GroupConstants.IdTemplate)]
        public async Task<bool> Handle(
            DeleteContactRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var entity = await db.Contacts
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct)
                .ConfigureAwait(false);
                
            if (entity != null)
            {
                db.Contacts.Remove(entity);
                await db.SaveChangesAsync(ct).ConfigureAwait(false);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Updates the call status for a contact and creates a call log entry.
        /// </summary>
        /// <param name="request">The call status update request.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated contact DTO.</returns>
        [MediatorHttpPost("updateCallStatus", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            UpdateCallStatusRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            
            var user = await userManager.FindByNameAsync(userId)
                ?? throw new InvalidOperationException($"User '{userId}' not found.");
            
            var contact = await db.Contacts
                .Include(c => c.CallHistory)
                .FirstOrDefaultAsync(x => x.Id == request.ContactId, ct)
                .ConfigureAwait(false);
            
            if (contact == null)
            {
                throw new InvalidOperationException($"Contact with ID {request.ContactId} was not found.");
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

        /// <summary>
        /// Reschedules a call for a contact.
        /// </summary>
        /// <param name="request">The call rescheduling request.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated contact DTO.</returns>
        [MediatorHttpPost("rescheduleCall", GroupConstants.NoTemplate)]
        public async Task<ContactDto> Handle(
            RescheduleCallRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            
            var user = await userManager.FindByNameAsync(userId)
                ?? throw new InvalidOperationException($"User '{userId}' not found.");
            
            var contact = await db.Contacts
                .Include(c => c.CallHistory)
                .FirstOrDefaultAsync(x => x.Id == request.ContactId, ct)
                .ConfigureAwait(false);
            
            if (contact == null)
            {
                throw new InvalidOperationException($"Contact with ID {request.ContactId} was not found.");
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
                Notes = $"Postponed: {request.Reason ?? "No reason provided"}",
                Status = SharedModels.Dtos.Contacts.CallStatus.Postponed,
                NextCallDate = contact.NextCallDate,
                CreatedAt = DateTime.UtcNow
            };
            
            contact.CallHistory.Add(callLog);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            
            return MapToDto(contact);
        }

        /// <summary>
        /// Maps a Contact entity to a ContactDto.
        /// </summary>
        /// <param name="contact">The contact entity to map.</param>
        /// <returns>The mapped contact DTO.</returns>
        private static ContactDto MapToDto(DbModels.Contact contact)
        {
            return new ContactDto(
                contact.WixId,
                contact.Name,
                contact.Email,
                contact.Phone,
                contact.Industry,
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
                Id = contact.Id,
                CreatedAt = contact.CreatedAt,
                UpdatedAt = contact.UpdatedAt
            };
        }
    }
}
