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
            string error = string.Empty;

            if ( configuration.EngineType != EngineType.Electric )
                error += $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported. ";

            if ( configuration.TransmissionType != TransmissionType.Automatic )
                error += $"Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.";

            if ( string.IsNullOrEmpty( error ) )
                return CompatibilityResult.Success();

            return CompatibilityResult.Failure( error.Trim() );
        }
    }
}