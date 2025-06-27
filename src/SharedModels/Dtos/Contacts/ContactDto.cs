using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Dtos.Common;

namespace SharedModels.Dtos.Contacts
{
    public record ContactDto(string WixId, string Name, string Email, string Phone, string Branche)
        : BaseDto;
}
