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
            string error = string.Empty;

            if ( configuration.EngineType == EngineType.Electric &&
                configuration.TransmissionType == TransmissionType.Manual )
            {
                error += "Toyota does not support Manual transmission for Electric vehicles.";
            }

            if ( string.IsNullOrEmpty( error ) )
                return CompatibilityResult.Success();

            return CompatibilityResult.Failure( error.Trim() );
        }
    }
}