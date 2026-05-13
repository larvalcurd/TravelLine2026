using CarFactory.Domain;

namespace CarFactory.Factories.Compatibility
{
    public interface IBrandCompatibilityPolicy
    {
        CompatibilityResult CheckCompatibility( CarConfiguration configuration );
    }
}