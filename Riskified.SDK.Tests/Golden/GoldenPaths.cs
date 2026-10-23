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

        /// <summary>
        /// What 5.7.x sent, frozen when 5.8.0 started changing shapes. GOLDEN_UPDATE never rewrites these.
        /// </summary>
        public static string Snapshots5_7 => Path.Combine(Snapshots, "5.7");

        public static string Schema => Path.Combine(Root, "Schema");
    }
}
