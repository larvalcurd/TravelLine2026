using CarFactory.Domain;

namespace CarFactory.Factories
{
    public class BMWCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration ) => true;

        public string? GetUnsupportedReason( CarConfiguration configuration ) => null;
    }
}