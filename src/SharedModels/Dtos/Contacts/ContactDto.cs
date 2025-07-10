using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Dtos.Common;

namespace SharedModels.Dtos.Contacts
{
    public record ContactDto(
        string WixId, 
        string Name, 
        string Email, 
        string Phone, 
        string Branche,
        DateTime? NextCallDate = null,
        string? CallNotes = null,
        CallStatus CallStatus = CallStatus.New,
        DateTime? LastCallDate = null,
        List<CallLogEntry>? CallHistory = null
    ) : BaseDto;

    public enum CallStatus
    {
        New,
        Scheduled,
        Reached,
        NotReached,
        Completed,
        Postponed
    }

    public record CallLogEntry(
        DateTime CallDate,
        string Notes,
        CallStatus Status,
        DateTime? NextCallDate = null
    );
}
