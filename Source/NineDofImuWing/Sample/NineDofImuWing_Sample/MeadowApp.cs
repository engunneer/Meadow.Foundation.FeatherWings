using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Hardware;
using Meadow.Units;
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

            // Example that uses an IObservable subscription to only be notified when the filter is satisfied
            var consumer = NineDofImuWing.CreateObserver(handler: result => HandleResult(this, result),
                                                 filter: result => FilterResult(result));

            nineDofImuWing.Subscribe(consumer);

            // classical .NET events can also be used:
            nineDofImuWing.Updated += HandleResult;

            nineDofImuWing.Acceleration3DUpdated += HandleResult;
            nineDofImuWing.AngularVelocity3DUpdated += HandleResult;
            nineDofImuWing.MagneticField3DUpdated += HandleResult;

            nineDofImuWing.StartUpdating(TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        bool FilterResult(
            IChangeResult<(Acceleration3D? Acceleration3D, 
            AngularVelocity3D? AngularVelocity3D, 
            MagneticField3D? MagneticField3D)> result)
        {
            return result.New.Acceleration3D.Value.Z > new Acceleration(0.1, Acceleration.UnitType.Gravity);
        }

        void HandleResult(object sender,
            IChangeResult<(Acceleration3D? Acceleration3D,
            AngularVelocity3D? AngularVelocity3D,
            MagneticField3D? MagneticField3D)> result)
        {
            var accel = result.New.Acceleration3D.Value;
            var gyro = result.New.AngularVelocity3D.Value;
            var mag = result.New.MagneticField3D.Value;

            Resolver.Log.Info($"AccelX={accel.X.Gravity:0.##}g, AccelY={accel.Y.Gravity:0.##}g, AccelZ={accel.Z.Gravity:0.##}g, GyroX={gyro.X.DegreesPerSecond:0.##}°/s, GyroY={gyro.Y.DegreesPerSecond:0.##}°/s, GyroZ={gyro.Z.DegreesPerSecond:0.##}°/s, MagX={mag.X.Gauss:0.##}gauss, MagY={mag.Y.Gauss:0.##}gauss, GyroZ={mag.Z.Gauss:0.##}gauss");
        }

        void HandleResult<UNIT>(object sender, IChangeResult<UNIT> result) 
        where UNIT : struct
        {
            if (result is IChangeResult<Acceleration3D> accel)
            {
                Resolver.Log.Info($"AccelX={accel.New.X.Gravity:0.##}g, AccelY={accel.New.Y.Gravity:0.##}g, AccelZ={accel.New.Z.Gravity:0.##}g");
            }
            else if (result is IChangeResult<AngularVelocity3D> gyro)
            {
                Resolver.Log.Info($"GyroX={gyro.New.X.DegreesPerSecond:0.##}°/s, GyroY={gyro.New.Y.DegreesPerSecond:0.##}°/s, GyroZ={gyro.New.Z.DegreesPerSecond:0.##}°/s");
            }
            else if (result is IChangeResult<MagneticField3D> mag)
            {
                Resolver.Log.Info($"MagX={mag.New.X.Gauss:0.##}gauss, MagY={mag.New.Y.Gauss:0.##}gauss, MagZ={mag.New.Z.Gauss:0.##}gauss");
            }
        }

        //<!=SNOP=>
    }
}