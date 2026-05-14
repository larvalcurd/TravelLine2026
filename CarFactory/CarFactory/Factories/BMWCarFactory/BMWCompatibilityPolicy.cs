using CarFactory.Domain;
using CarFactory.Factories.Compatibility;

namespace CarFactory.Factories.BMWCarFactory
{
    public class BMWCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public CompatibilityResult CheckCompatibility( CarConfiguration configuration )
        {
            return CompatibilityResult.Success();
        }
    }
}