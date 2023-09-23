using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.FeatherWings;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace NineDofImuWing_Sample
{
    public class MeadowApp : App<F7FeatherV1>
    {
        //<!=SNIP=>

        NineDofImuWing nineDofImuWing;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing ...");

            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);


            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            return new Task(() => { });
        }

        //<!=SNOP=>
    }
}