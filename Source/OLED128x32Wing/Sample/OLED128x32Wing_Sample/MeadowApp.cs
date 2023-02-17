using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace FeatherWings.OLED128x32_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        OLED128x32Wing oledWing;
        MicroGraphics graphics;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing ...");
            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);

            oledWing = new OLED128x32Wing(i2cBus, Device.Pins.D11, Device.Pins.D10, Device.Pins.D09);

            graphics = new MicroGraphics(oledWing.Display);
            graphics.CurrentFont = new Font12x16();

            oledWing.ButtonA.Clicked += (sender, e) => UpdateDisplay("A pressed");
            oledWing.ButtonB.Clicked += (sender, e) => UpdateDisplay("B pressed");
            oledWing.ButtonC.Clicked += (sender, e) => UpdateDisplay("C pressed");

            return Task.CompletedTask;
        }

        void UpdateDisplay(string message)
        {
            graphics.Clear();
            graphics.DrawText(0, 8, message);
            graphics.Show();
        }

        //<!=SNOP=>
    }
}