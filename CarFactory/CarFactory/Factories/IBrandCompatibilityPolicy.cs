using CarFactory.Domain;

namespace CarFactory.Factories
{
    public interface IBrandCompatibilityPolicy
    {
        bool IsSupported( CarConfiguration configuration );
        string? GetUnsupportedReason( CarConfiguration configuration );
    }
}