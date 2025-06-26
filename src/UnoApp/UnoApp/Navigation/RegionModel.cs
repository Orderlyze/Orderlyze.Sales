using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Navigation;

public record RegionModel(string Region, string View, string? PreRoute = null, object? data = null);
