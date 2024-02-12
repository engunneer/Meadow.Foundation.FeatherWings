﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FeatherWings.LedMatrix8x16_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        LedMatrix8x16Wing ledMatrixWing;
        MicroGraphics graphics;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing ..");

            ledMatrixWing = new LedMatrix8x16Wing(Device.CreateI2cBus());
            ledMatrixWing.Clear();

            graphics = new MicroGraphics(ledMatrixWing)
            {
                Rotation = RotationType._90Degrees,
                CurrentFont = new Font4x8()
            };

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            graphics.Clear();

            graphics.DrawText(0, 0, "M F7");

            graphics.Show();

            return Task.CompletedTask;
        }

        //<!=SNOP=>

        void PixelWalk()
        {
            for (byte j = 0; j < 16; j++)
            {
                for (byte i = 0; i < 8; i++)
                {
                    ledMatrixWing.Clear();
                    ledMatrixWing.DrawPixel(i, j, true);
                    ledMatrixWing.Show();
                    Thread.Sleep(50);
                }
            }
        }

        void FourCorners()
        {
            ledMatrixWing.Clear();
            ledMatrixWing.DrawPixel(0, 0, true);
            ledMatrixWing.DrawPixel(7, 0, true);
            ledMatrixWing.DrawPixel(0, 7, true);
            ledMatrixWing.DrawPixel(7, 7, true);
            ledMatrixWing.Show();
        }
    }
}