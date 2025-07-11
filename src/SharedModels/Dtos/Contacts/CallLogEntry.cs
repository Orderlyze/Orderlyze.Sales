using System;

namespace SharedModels.Dtos.Contacts
{
    public record CallLogEntry(
        DateTime CallDate,
        string Notes,
        CallStatus Status,
        DateTime? NextCallDate = null
    );
}