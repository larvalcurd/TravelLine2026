using CarFactory.Domain;

namespace CarFactory.Factories
{
    public interface IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration );
        public string? GetUnsupportedReason( CarConfiguration configuration );
    }
}