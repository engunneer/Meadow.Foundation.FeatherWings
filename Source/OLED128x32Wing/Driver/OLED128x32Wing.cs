using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruits OLED Feather Wing
    /// </summary>
    public class OLED128x32Wing
    {
        /// <summary>
        /// Returns Ssd1306 object
        /// </summary>
        public Ssd1306 Display { get; protected set; }

        /// <summary>
        /// Returns button A
        /// </summary>
        public PushButton ButtonA { get; protected set; }

        /// <summary>
        /// Returns button B
        /// </summary>
        public PushButton ButtonB { get; protected set; }

        /// <summary>
        /// Returns button C
        /// </summary>
        public PushButton ButtonC { get; protected set; }

        /// <summary>
        /// Creates a OLED128x32Wing driver
        /// </summary>
        /// <param name="i2cBus"></param>
        /// <param name="portA"></param>
        /// <param name="portB"></param>
        /// <param name="portC"></param>
        public OLED128x32Wing(II2cBus i2cBus, IDigitalInputPort portA, IDigitalInputPort portB, IDigitalInputPort portC)
        {
            Display = new Ssd1306(i2cBus, (byte)Ssd1306.Addresses.Default, Ssd1306.DisplayType.OLED128x32);

            ButtonA = new PushButton(portA);
            ButtonB = new PushButton(portB);
            ButtonC = new PushButton(portC);
        }

        /// <summary>
        /// Creates a OLED128x32Wing driver
        /// </summary>
        /// <param name="i2cBus"></param>
        /// <param name="device"></param>
        /// <param name="pinA"></param>
        /// <param name="pinB"></param>
        /// <param name="pinC"></param>
        public OLED128x32Wing(II2cBus i2cBus, IDigitalInputController device, IPin pinA, IPin pinB, IPin pinC) : 
            this(i2cBus, 
                device.CreateDigitalInputPort(pinA, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp),
                device.CreateDigitalInputPort(pinB, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp),
                device.CreateDigitalInputPort(pinC, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp))
        { }
    }
}