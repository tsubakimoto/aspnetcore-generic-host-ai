using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetcore_generic_host_ai
{
    class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger<LifetimeEventsHostedService> logger;
        private readonly IHostApplicationLifetime appLifeTime;
        private readonly TelemetryClient telemetryClient;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IHostApplicationLifetime appLifeTime,
            TelemetryClient telemetryClient)
        {
            this.logger = logger;
            this.appLifeTime = appLifeTime;
            this.telemetryClient = telemetryClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (telemetryClient.StartOperation<RequestTelemetry>(nameof(LifetimeEventsHostedService)))
            {
                appLifeTime.ApplicationStarted.Register(OnStarted);
                logger.LogInformation($"{nameof(StartAsync)} registered OnStarted : {DateTime.Now}");

                appLifeTime.ApplicationStopping.Register(OnStopping);
                logger.LogInformation($"{nameof(StartAsync)} registered OnStopping : {DateTime.Now}");

                appLifeTime.ApplicationStopped.Register(OnStopped);
                logger.LogInformation($"{nameof(StartAsync)} registered OnStopped : {DateTime.Now}");
            }

            appLifeTime.StopApplication();
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            logger.LogInformation($"{nameof(OnStarted)} : {DateTime.Now}");
            telemetryClient.TrackEvent(nameof(OnStarted), new Dictionary<string, string> { { "now", DateTime.Now.ToShortTimeString() } });
        }

        private void OnStopping()
        {
            logger.LogInformation($"{nameof(OnStopping)} : {DateTime.Now}");
            telemetryClient.TrackEvent(nameof(OnStopping), new Dictionary<string, string> { { "now", DateTime.Now.ToShortTimeString() } });
        }

        private void OnStopped()
        {
            logger.LogInformation($"{nameof(OnStopped)} : {DateTime.Now}");
            telemetryClient.TrackEvent(nameof(OnStopped), new Dictionary<string, string> { { "now", DateTime.Now.ToShortTimeString() } });
            telemetryClient.Flush();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}