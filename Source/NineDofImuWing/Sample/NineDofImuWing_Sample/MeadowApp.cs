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

            nineDofImuWing = new NineDofImuWing(i2cBus);

            nineDofImuWing.Lsm6dsox.StartUpdating();
            nineDofImuWing.Lis3Mdl.StartUpdating();

            nineDofImuWing.Lsm6dsox.Updated += (sender, eventArgs) => { };
            nineDofImuWing.Lis3Mdl.Updated += (sender, eventArgs) => { };

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            // TODO: Follow example of Lsm6dsox sample, but for more sensors.

            return new Task(() => { });
        }

        //<!=SNOP=>
    }
}