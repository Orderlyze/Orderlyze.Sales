using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Presentation.Views.WixContacts;

internal partial record WixContactsListModel(
    string Id,
    string Name,
    string Email,
    string? Branche,
    string? Company,
    IReadOnlyCollection<string> Labels
);
