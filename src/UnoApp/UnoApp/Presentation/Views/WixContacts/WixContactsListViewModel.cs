using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Presentation.Common;
using UnoApp.Services.Common;
using WixApi.Models;

namespace UnoApp.Presentation.Views.WixContacts;

internal partial class WixContactsListViewModel(BaseServices Services, List<WixContact> wixContacts)
    : BaseItemViewModel<List<WixContact>>(Services, wixContacts) { }
