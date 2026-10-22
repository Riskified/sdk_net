using System.IO;
using System.Linq;
using System.Reflection;

namespace Riskified.SDK.Tests.Golden
{
    /// <summary>
    /// The Golden folder in the source tree, not the build output, so GOLDEN_UPDATE writes straight
    /// into the repository. The test project records it at build time (AssemblyMetadata in the
    /// csproj); [CallerFilePath] isn't used because deterministic CI builds rewrite it to "/_/".
    /// </summary>
    internal static class GoldenPaths
    {
        public static string Root { get; } = typeof(GoldenPaths).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(attribute => attribute.Key == "GoldenDirectory")
            .Value!;

        public static string Snapshots => Path.Combine(Root, "Snapshots");

        public static string Schema => Path.Combine(Root, "Schema");
    }
}
