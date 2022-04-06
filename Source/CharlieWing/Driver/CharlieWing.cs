using System;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;

namespace Meadow.Foundation.FeatherWing
{
    /// <summary>
    /// Represents an Adafruit CharliePlex 15x7 feather wing
    /// </summary>
    public class CharlieWing : IGraphicsDisplay
    {
        protected readonly Is31fl3731 iS31FL3731;

        /// <summary>
        /// Color mode of display
        /// </summary>
        public ColorType ColorMode => ColorType.Format8bppGray;

        /// <summary>
        /// Width of display in pixels
        /// </summary>
        public int Width => 15;

        /// <summary>
        /// Height of display in pixels
        /// </summary>
        public int Height => 7;

        public byte Frame { get; set; }

        /// <summary>
        /// Gets/Sets property to ignore boundaries when drawing outside of the LED matrix
        /// </summary>
        public bool IgnoreOutOfBoundsPixels { get; set; }

        /// <summary>
        /// Creates a CharlieWing driver
        /// </summary>
        /// <param name="i2cBus"></param>
        /// <param name="address"></param>
        public CharlieWing(II2cBus i2cBus, byte address = (byte)Is31fl3731.Addresses.Default)
        {
            iS31FL3731 = new Is31fl3731(i2cBus, address);
            iS31FL3731.Initialize();

            for (byte i = 0; i <= 7; i++)
            {
                iS31FL3731.SetLedState(i, true);
                iS31FL3731.Clear(i);
            }
        }

        /// <summary>
        /// Clear display
        /// </summary>
        /// <param name="updateDisplay"></param>
        public void Clear(bool updateDisplay = false)
        {
            iS31FL3731.Clear(Frame);
        }

        /// <summary>
        /// Turn on an RGB LED with the specified color on (x,y) coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawPixel(int x, int y, Color color)
        {
            DrawPixel(x, y, color.Color8bppGray);
        }

        /// <summary>
        /// Turn on a LED on (x,y) coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colored"></param>
        public void DrawPixel(int x, int y, bool colored)
        {
            DrawPixel(x, y, colored ? Color.White : Color.Black);
        }

        /// <summary>
        /// Turn on LED with the specified brightness on (x,y) coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="brightness"></param>
        public void DrawPixel(int x, int y, byte brightness)
        {
            if (IgnoreOutOfBoundsPixels)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                { return; }
            }

            if (x > 7)
            {
                x = 15 - x;
                y += 8;
            }
            else
            {
                y = 7 - y;
            }

            //Swap
            var temp = x;
            x = y;
            y = temp;
      
            iS31FL3731.SetLedPwm(Frame, (byte)(x + y * 16), brightness);
        }

        /// <summary>
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void InvertPixel(int x, int y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draw a buffer to the display
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="displayBuffer"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DrawBuffer(int x, int y, IDisplayBuffer displayBuffer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clear the display.
        /// </summary>
        /// <param name="clearColor"></param>
        /// <param name="updateDisplay"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Fill(Color clearColor, bool updateDisplay = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clear the display.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fillColor"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Fill(int x, int y, int width, int height, Color fillColor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show changes on the display
        /// </summary>
        public void Show()
        {
            iS31FL3731.DisplayFrame(Frame);
        }

        /// <summary>
        /// Show changes on the display
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public void Show(int left, int top, int right, int bottom)
        {
            Show();
        }

        /// <summary>
        /// Show changes on the display
        /// </summary>
        /// <param name="frame"></param>
        public void Show(byte frame)
        {   
            // TODO
            iS31FL3731.DisplayFrame(Frame);
        }
    }
}