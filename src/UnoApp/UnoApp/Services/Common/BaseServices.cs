using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Extensions.DependencyInjection;

namespace UnoApp.Services.Common;

[Service(ServiceLifetime.Scoped)]
public record BaseServices(INavigator Navigator);
