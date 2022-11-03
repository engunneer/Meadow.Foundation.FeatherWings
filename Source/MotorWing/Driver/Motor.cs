using Meadow.Foundation.ICs.IOExpanders;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents a generic motor
    /// </summary>
    public abstract class Motor
    {
        /// <summary>
        /// The pca9685 instance used to control the motor
        /// </summary>
        protected readonly Pca9685 pca9685;

        /// <summary>
        /// Creates a Motor driver
        /// </summary>
        /// <param name="pca9685">A pca9685 instance</param>
        public Motor(Pca9685 pca9685)
        {
            this.pca9685 = pca9685;
        }

        /// <summary>
        /// Set the motor speed
        /// </summary>
        /// <param name="speed">The motor speed</param>
        public abstract void SetSpeed(short speed);
    }
}