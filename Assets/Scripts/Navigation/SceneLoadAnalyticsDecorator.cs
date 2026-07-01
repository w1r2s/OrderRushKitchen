using Cysharp.Threading.Tasks;
using OrderRushKitchen.Sdk;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;

namespace OrderRushKitchen.Navigation
{
    public sealed class SceneLoadAnalyticsDecorator : ISceneLoader
    {
        private readonly ISceneLoader _inner;
        private readonly IAnalyticsService _analyticsService;
        private readonly ICrashReportingService _crashReportingService;

        public SceneLoadAnalyticsDecorator(ISceneLoader inner, IAnalyticsService analyticsService, ICrashReportingService crashReportingService)
        {
            _inner = inner;
            _analyticsService = analyticsService;
            _crashReportingService = crashReportingService;
        }

        public async UniTask LoadAsync(GameSceneId sceneId, CancellationToken cancellationToken)
        {
            try
            {
                await _inner.LoadAsync(sceneId, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                string sourceScene = SceneManager.GetActiveScene().name;
                string targetScene = sceneId.ToString();

                var analyticsParameters = new Dictionary<string, object>
                {
                    [AnalyticsParameters.SourceScene] = sourceScene,
                    [AnalyticsParameters.TargetScene] = targetScene,
                    [AnalyticsParameters.Reason] = exception.GetType().Name,
                    [AnalyticsParameters.ErrorCode] = exception.Message
                };

                _analyticsService.LogEvent(AnalyticsEvents.SceneLoadFailed, analyticsParameters);

                var crashContext = new Dictionary<string, object>
                {
                    [CrashReportKeys.SourceScene] = sourceScene,
                    [CrashReportKeys.TargetScene] = targetScene,
                    [CrashReportKeys.ErrorStage] = "scene_load"
                };

                _crashReportingService.ReportException(exception, crashContext);

                throw;
            }
        }
    }
}