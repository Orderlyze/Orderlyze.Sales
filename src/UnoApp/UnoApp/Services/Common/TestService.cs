using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Services.Common;
[Service(ServiceLifetime.Singleton)]
internal class TestService : ITestService
{
}
internal interface ITestService;
