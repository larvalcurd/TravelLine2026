using CarFactory.Components.Engine;
using CarFactory.Components.Transmission;
using CarFactory.Domain;

namespace CarFactory.Factories.ToyotaCarFactory
{
    public class ToyotaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration ) =>
            GetUnsupportedReason( configuration ) == null;

        public string? GetUnsupportedReason( CarConfiguration configuration )
        {
            return (configuration.EngineType, configuration.TransmissionType) switch
            {
                (EngineType.Electric, TransmissionType.Manual ) =>
                    "Toyota does not support Manual transmission for Electric vehicles.",
                _ => null
            };
        }
    }
}