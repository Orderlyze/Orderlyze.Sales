using Aspire.Hosting.ApplicationModel;

internal static partial class Projects
{
    internal sealed class WebApi : IProjectMetadata
    {
        public string ProjectPath => "../WebApi/WebApi.csproj";
    }

    internal sealed class UnoApp : IProjectMetadata
    {
        public string ProjectPath => "../UnoApp/UnoApp/UnoApp.csproj";
    }
}
