using CarFactory.Components.Engine;
using CarFactory.Components.Transmission;
using CarFactory.Domain;
using CarFactory.Factories.Compatibility;

namespace CarFactory.Factories.ToyotaCarFactory
{
    public class ToyotaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public CompatibilityResult CheckCompatibility( CarConfiguration configuration )
        {
            string? reason = (configuration.EngineType, configuration.TransmissionType) switch
            {
                (EngineType.Electric, TransmissionType.Manual ) =>
                    "Toyota does not support Manual transmission for Electric vehicles.",
                _ => null
            };

            return reason is null
                ? CompatibilityResult.Success()
                : CompatibilityResult.Failure( reason );
        }
    }
}