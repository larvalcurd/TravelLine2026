using CarFactory.Components.Engine;
using CarFactory.Components.Transmission;
using CarFactory.Domain;
using CarFactory.Factories.Compatibility;

namespace CarFactory.Factories.TeslaCarFactory
{
    public class TeslaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public CompatibilityResult CheckCompatibility( CarConfiguration configuration )
        {
            string? reason = (configuration.EngineType, configuration.TransmissionType) switch
            {
                (not EngineType.Electric, not TransmissionType.Automatic ) =>
                    $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported. " +
                    $"Additionally, Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.",
                (not EngineType.Electric, _ ) =>
                    $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported.",
                (_, not TransmissionType.Automatic ) =>
                    $"Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.",
                _ => null
            };

            return reason is null
                ? CompatibilityResult.Success()
                : CompatibilityResult.Failure( reason );
        }
    }
}