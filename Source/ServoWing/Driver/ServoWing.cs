using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using System;

namespace Meadow.Foundation.FeatherWing
{
    /// <summary>
    /// Represents Adafruit's Feather Servo Wing and 16-Channel 12-bit PWM/Servo Shield
    /// </summary>
    /// <remarks>All PWM channels run at the same Frequency</remarks>
    public class ServoWing 
    {
        Pca9685 pca9685;

        readonly short portCount;

        /// <summary>
        /// Creates a ServoWing driver
        /// </summary>
        /// <param name="i2cBus"></param>
        /// <param name="address"></param>
        /// <param name="frequency"></param>
        /// <param name="portCount"></param>
        public ServoWing(
            II2cBus i2cBus, 
            byte address = (byte)Pca9685.Addresses.Default, 
            int frequency = 50, 
            short portCount = 8)
        {
            if (portCount != 8 && portCount != 16)
            {
                throw new ArgumentException("Channels need to be 8 or 16", "ports");
            }

            this.portCount = portCount;
            pca9685 = new Pca9685(i2cBus, address, frequency);
            pca9685.Initialize();
        }

        /// <summary>
        /// Returns the specified servo
        /// </summary>
        /// <param name="portIndex"></param>
        /// <param name="servoConfig"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Servo GetServo(byte portIndex, ServoConfig servoConfig)
        {
            if ((portIndex < 0) || (portIndex > portCount))
            {
                throw new ArgumentException($"Servo num must be between 1 and {portCount}", "num");
            }

            var pwm = pca9685.CreatePwmPort(portIndex);
            var servo = new Servo(pwm, servoConfig);

            return servo;
        }

        /// <summary>
        /// Returns the specified continues rotation servo
        /// </summary>
        /// <param name="portIndex"></param>
        /// <param name="servoConfig"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IContinuousRotationServo GetContinuousRotatioServo(byte portIndex, ServoConfig servoConfig)
        {
            if ((portIndex < 0) || (portIndex > portCount))
            {
                throw new ArgumentException($"Continuous Rotatio Servo num must be between 1 and {portCount}", "num");
            }

            var pwm = pca9685.CreatePwmPort(portIndex);
            var servo = new ContinuousRotationServo(pwm, servoConfig);

            return servo;
        }
    }
}