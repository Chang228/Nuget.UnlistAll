﻿using System;
using System.IO;
using Nuget.UnlistAll.Models;
using System.Diagnostics;
using Nuget.UnlistAll.Configuration;
using Nuget.UnlistAll.Resources;
using Shuhari.Framework.Utils;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Nuget.UnlistAll.Tasks
{
    public class UnlistTask : WorkerTask
    {
        public UnlistTask(IWorkerUi ui, AppConfig config, PackageVersion[] versions) 
            : base(ui, config)
        {
            Expect.IsNotNull(versions, nameof(versions));

            _versions = versions;
        }

        private readonly PackageVersion[] _versions;

        protected override object ExecuteCore()
        {
            // Inn developing mode, the .exe layout in bin\[Debug|Release], while 
            // when packaged by nuget the .exe are copied together with nuget.exe.
            // The task should support either case.
            //var nugetPath = FindNugetPaths(@".\nuget.exe",
            //    @"..\..\..\packages\NuGet.CommandLine.4.1.0\tools\nuget.exe");
            //if (nugetPath == null)
            //    throw new FileNotFoundException(Strings.NugetNotFound);
            //NotifyLog(true, Strings.NugetFound, nugetPath);
            Parallel.ForEach(_versions, new ParallelOptions { MaxDegreeOfParallelism = 10 }, version =>
            {
                var src = this.Config.Source;
                if (string.IsNullOrEmpty(src))
                {
                    src = "https://api.nuget.org/";
                }
                var cmdArgs = $"delete {version.PackageId} {version.Version} -ApiKey {Config.ApiKey} -Source {src} -NonInteractive";
                var psi = new ProcessStartInfo("nuget", cmdArgs)
                {
                    RedirectStandardOutput = true,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                var process = Process.Start(psi);
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                NotifyLog(true, Strings.UnlistResult,
                    version.PackageId, version.Version, output);
            });
             
            return null;
        }

        private string FindNugetPaths(params string[] relativePaths)
        {
            foreach (var relativePath in relativePaths)
            {
                var fullPath = FindNugetPath(relativePath);
                if (fullPath != null)
                    return fullPath;
            }
            return null;
        }

        private string FindNugetPath(string relativePath)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
            if (File.Exists(fullPath))
                return fullPath;
            return null;
        }
    }
}
