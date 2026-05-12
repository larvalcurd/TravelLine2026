using CarFactory.Domain;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Services
{
    public class CarPerformanceCalculator
    {
        private const double BaseSpeed = 100.0;
        private const double HorsePowerWeight = 0.35;
        private const double GearSpeedBonus = 3.0;
        private const double PercentageDivisor = 100.0;

        public CarPerformance Calculate( IEngine engine, ITransmission transmission, IBody body )
        {
            double rawSpeed = BaseSpeed
                + engine.BaseMaxSpeedBonus
                + ( engine.HorsePower * HorsePowerWeight )
                + ( transmission.GearCount * GearSpeedBonus );

            int maxSpeed = ( int )Math.Round( rawSpeed * body.AerodynamicsFactor / PercentageDivisor );

            return new CarPerformance( maxSpeed, transmission.GearCount );
        }
    }
}