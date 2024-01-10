using Microsoft.ApplicationInsights.Channel;

namespace Presentation.Telemetry
{
    public interface ITelemetryInitializer
    {
        void Initialize(ITelemetry telemetry);
    }
}