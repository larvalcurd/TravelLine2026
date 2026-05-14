namespace CarFactory.Factories.Compatibility
{
    public record CompatibilityResult( bool IsSupported, string? UnsupportedReason = null )
    {
        public static CompatibilityResult Success() => new( true );
        public static CompatibilityResult Failure( string reason ) => new( false, reason );
    }
}