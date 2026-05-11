using CarFactory.Domain;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Services
{
    public class CarPerformanceCalculator
    {
        public CarPerformance Calculate( IEngine engine, ITransmission transmission, IBody body )
        {
            double rawSpeed =
                100
                + engine.BaseMaxSpeedBonus
                + engine.HorsePower * 0.35
                + transmission.GearCount * 3;

            int maxSpeed = ( int )Math.Round( rawSpeed * body.AerodynamicsFactor / 100 );

            return new CarPerformance( maxSpeed, transmission.GearCount );
        }
    }
}