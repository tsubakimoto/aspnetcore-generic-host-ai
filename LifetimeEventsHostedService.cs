using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetcore_generic_host_ai
{
    class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger<LifetimeEventsHostedService> logger;
        private readonly IHostApplicationLifetime appLifeTime;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IHostApplicationLifetime appLifeTime)
        {
            this.logger = logger;
            this.appLifeTime = appLifeTime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            appLifeTime.ApplicationStarted.Register(OnStarted);
            appLifeTime.ApplicationStopping.Register(OnStopping);
            appLifeTime.ApplicationStopped.Register(OnStopped);

            appLifeTime.StopApplication();
            return Task.CompletedTask;
        }

        private void OnStarted() => logger.LogInformation($"{nameof(OnStarted)} : {DateTime.Now}");

        private void OnStopping() => logger.LogInformation($"{nameof(OnStopping)} : {DateTime.Now}");

        private void OnStopped() => logger.LogInformation($"{nameof(OnStopped)} : {DateTime.Now}");

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}