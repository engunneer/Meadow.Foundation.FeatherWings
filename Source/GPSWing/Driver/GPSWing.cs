using Meadow.Foundation.Sensors.Gnss;
using Meadow.Hardware;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents a featherwing GPSWing
    /// </summary>
    public class GPSWing : Mt3339
    {
        /// <summary>
        /// Creates a GPSWing driver
        /// </summary>
        /// <param name="serialMessagePort"></param>
        public GPSWing(ISerialMessagePort serialMessagePort)
            : base(serialMessagePort)
        { }
    }
}