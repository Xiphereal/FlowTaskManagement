using System;
using System.Diagnostics;
using System.IO;

namespace Desktop.Tests.e2e.Utils;

public class BackendLauncher : IDisposable
{
    private Process process;

    /// <remarks>
    ///     This is a tentative approach. It is error-prone, fragile and couples both
    ///     the location of the Backend and its execution to the same machine that the
    ///     Desktop app is running for the test.
    ///     <para>
    ///         Ideally, the desired version of the Backend to test will be deployed in
    ///         a Docker container by the CI/CD pipeline, running health checks and
    ///         ensuring it is reachable for this e2e tests.
    ///     </para>
    /// </remarks>
    public void Launch()
    {
        var pathToBackend = Path.GetFullPath(
            Path.Combine(
                Environment.CurrentDirectory,
                "..",
                "..",
                "..",
                "..",
                "Backend",
                "Backend",
                "Backend.csproj"));

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments =
                $"run --project {pathToBackend} --applicationUrl http://localhost:5045",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process = Process.Start(startInfo);
    }

    public void Dispose()
    {
        process.Kill();
    }
}