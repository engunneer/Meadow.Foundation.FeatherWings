using Meadow.Foundation.ICs.IOExpanders;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents a generic motor
    /// </summary>
    public abstract class Motor
    {
        protected readonly Pca9685 _pca9685;

        /// <summary>
        /// Creates a Motor driver
        /// </summary>
        /// <param name="pca9685"></param>
        public Motor(Pca9685 pca9685)
        {
            _pca9685 = pca9685;
        }

        /// <summary>
        /// Set speed
        /// </summary>
        /// <param name="speed"></param>
        public abstract void SetSpeed(short speed);
    }
}