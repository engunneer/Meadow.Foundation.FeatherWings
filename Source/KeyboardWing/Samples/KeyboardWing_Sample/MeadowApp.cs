﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace FeatherWings.KeyboardWing_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        KeyboardWing keyboardWing;
        MicroGraphics graphics;

        string lastKeyPress;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing ...");

            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
            var spiBus = Device.CreateSpiBus(new Meadow.Units.Frequency(48000, Meadow.Units.Frequency.UnitType.Kilohertz));

            keyboardWing = new KeyboardWing(
                spiBus: spiBus,
                i2cBus: i2cBus,
                keyboardPin: Device.Pins.D10,
                displayChipSelectPin: Device.Pins.D11,
                displayDcPin: Device.Pins.D12,
                lightSensorPin: Device.Pins.A05);

            keyboardWing.TouchScreen.Rotation = RotationType._90Degrees;

            graphics = new MicroGraphics(keyboardWing.Display)
            {
                Rotation = RotationType._90Degrees,
                CurrentFont = new Font12x16()
            };

            keyboardWing.Keyboard.OnKeyEvent += Keyboard_OnKeyEvent;

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            graphics.Clear(true);

            keyboardWing.LightSensor.StartUpdating(new TimeSpan(0, 0, 30));

            return Task.CompletedTask;
        }

        private void Keyboard_OnKeyEvent(object sender, Meadow.Foundation.Sensors.Hid.BBQ10Keyboard.KeyEvent e)
        {
            if (e.KeyState == BBQ10Keyboard.KeyState.StatePress)
            {
                Console.WriteLine($"OnKeyEvent ASCII value: {(byte)e.AsciiValue}");

                lastKeyPress = (byte)e.AsciiValue switch
                {
                    (byte)ButtonType._5WayUp => "5-way up",
                    (byte)ButtonType._5WayDown => "5-way down",
                    (byte)ButtonType._5WayLeft => "5-way left",
                    (byte)ButtonType._5WayRight => "5-way right",
                    (byte)ButtonType._5WayCenter => "5-way center",
                    (byte)ButtonType.Button1 => "Button 1",
                    (byte)ButtonType.Button2 => "Button 2",
                    (byte)ButtonType.Button3 => "Button 3",
                    (byte)ButtonType.Button4 => "Button 4",
                    _ => e.AsciiValue.ToString()
                };
            }

            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            graphics.Clear();

            graphics.DrawText(0, 0, $"Last pressed: {lastKeyPress}");
            graphics.DrawText(0, 16, $"Luminance: {keyboardWing.LightSensor.Illuminance.Value.Lux}");

            graphics.Show();
        }

        //<!=SNOP=>
    }
}