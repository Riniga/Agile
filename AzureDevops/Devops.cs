using Agile.Library.Teams;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevops
{
    internal abstract class Devops : IAzureDevopsService
    {

        protected readonly ILogger<Devops> logger;
        protected readonly IConfiguration configuration;

        protected Devops(ILogger<Devops> log, IConfiguration config)
        {
            logger = log;
            configuration = config;
            Settings.CacheEnabled = configuration.GetValue<string>("CacheEnabled").ToLower() == "true";
        }

        public abstract void Run();

    }
}
