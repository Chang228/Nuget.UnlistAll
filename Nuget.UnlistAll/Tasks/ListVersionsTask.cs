﻿using System;
using System.Linq;
using Nuget.UnlistAll.Api;
using Nuget.UnlistAll.Configuration;
using Nuget.UnlistAll.Models;
using Nuget.UnlistAll.Resources;

namespace Nuget.UnlistAll.Tasks
{
    /// <summary>
    /// List versions for package
    /// </summary>
    public class ListVersionsTask : WorkerTask
    {
        public ListVersionsTask(IWorkerUi ui, AppConfig config) 
            : base(ui, config)
        {
        }

        protected override object ExecuteCore()
        {
            string packageId = Config.PackageId;

            NotifyLog(true, Strings.ListVersionsBegin, packageId);
            var response = new NugetApi(Config).GetIndex();
            var result = response.SelectMany(x => x.Versions.Where(c=> c.Version.Contains( Config.PackageVersion)).Select(v=> new PackageVersion(x.Id, v.Version))).Distinct().ToArray();

            NotifyLog(true, Strings.ListVersionsFinished, packageId, result.Length);
            return result;
        }
    }
}
