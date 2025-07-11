using System;
using System.Collections.Generic;
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
}
