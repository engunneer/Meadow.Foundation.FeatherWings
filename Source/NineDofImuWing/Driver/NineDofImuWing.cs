using Meadow.Peripherals.Sensors.Motion;
using Meadow.Hardware;
using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Units;
using System;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's 9-DOF IMU FeatherWing
    /// </summary>
    public class NineDofImuWing
    {
        public Lis3Mdl Lis3Mdl { get; private set; }
        public Lsm6dsox Lsm6dsox { get; private set; }

        /// <summary>
        /// Represents Adafruit's 9-DOF IMU FeatherWing with an <see cref="Lsm6dsox"/> Accelerometer/Gyroscope and an <see cref="Lis3Mdl"/> Magnetometer.
        /// </summary>
        /// <param name="i2cBus">The I2C bus connected to the device</param>
        /// <param name="alternateAccelAddress">true if the solder jumper for A/G Address is closed on the back of the featherwing, or it's SDO pin is pulled high.</param>
        /// <param name="alternateMagAddress">true if the solder jumper for Mag Address is closed on the back of the featherwing, or it's SDO pin is pulled high.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="i2cBus"/> is <c>null</c></exception>
        public NineDofImuWing(II2cBus i2cBus, bool alternateAccelAddress = false, bool alternateMagAddress = false ) 
        {
            if (i2cBus == null)
            {
                throw new ArgumentNullException(nameof(i2cBus));
            }

            Lsm6dsox = new Lsm6dsox(i2cBus, (byte)(alternateAccelAddress ? Lsm6dsox.Addresses.Address_0x6B : Lsm6dsox.Addresses.Default));
            Lis3Mdl = new Lis3Mdl(i2cBus, (byte)(alternateMagAddress ? Lsm6dsox.Addresses.Address_0x6B : Lsm6dsox.Addresses.Default));
        }

        // TODO: Convenience methods for using these sensors together.
    }
}