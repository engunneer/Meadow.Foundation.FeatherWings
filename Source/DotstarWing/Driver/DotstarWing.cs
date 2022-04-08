using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using static Meadow.Foundation.Leds.Apa102;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's Dotstar feather wing 12x6
    /// </summary>
    public class DotstarWing : IGraphicsDisplay
    {
        Apa102 ledMatrix;

        /// <summary>
        /// Returns the color mode
        /// </summary>
        public ColorType ColorMode => ColorType.Format12bppRgb444;

        /// <summary>
        /// Returns the width of the RGB LED matrix
        /// </summary>
        public int Width => 12;

        /// <summary>
        /// Returns the height of the RGB LED matrix
        /// </summary>
        public int Height => 6;

        /// <summary>
        /// Get/Sets the RGB LED Matrix brightness
        /// </summary>
        public float Brightness
        {
            get => ledMatrix.Brightness;
            set => ledMatrix.Brightness = value;  
        }

        /// <summary>
        /// Gets/Sets property to ignore boundaries when drawing outside of the LED matrix
        /// </summary>
        public bool IgnoreOutOfBoundsPixels
        {
            get => ledMatrix.IgnoreOutOfBoundsPixels;
            set => ledMatrix.IgnoreOutOfBoundsPixels = value;
        }

        /// <summary>
        /// Creates a DotstarWing driver
        /// </summary>
        /// <param name="spiBus"></param>
        /// <param name="numberOfLeds"></param>
        /// <param name="pixelOrder"></param>
        public DotstarWing(
            ISpiBus spiBus, 
            int numberOfLeds, 
            PixelOrder pixelOrder = PixelOrder.BGR)
        {
            ledMatrix = new Apa102(spiBus, numberOfLeds, pixelOrder);
        }

        /// <summary>
        /// Creates a DotstarWing driver
        /// </summary>
        /// <param name="spiBus"></param>
        public DotstarWing(ISpiBus spiBus) 
            : this(spiBus, 72)
        { }

        /// <summary>
        /// Clear the RGB LED Matrix
        /// </summary>
        /// <param name="updateDisplay"></param>
        public void Clear(bool updateDisplay = false)
        {
            ledMatrix.Clear(updateDisplay);
        }

        /// <summary>
        /// Turn on an RGB LED with the specified color on (x,y) coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawPixel(int x, int y, Color color)
        {
            if (IgnoreOutOfBoundsPixels)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                { return; }
            }

            int minor = x;
            int major = y;
            int majorScale;

            major = Height - 1 - major;
            majorScale = Width;

            int pixelOffset = (major * majorScale) + minor;

            if (pixelOffset >= 0 && pixelOffset < Height * Width)
            {
                ledMatrix.SetLed(pixelOffset, color);
            }
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
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void InvertPixel(int x, int y)
        {
            if (IgnoreOutOfBoundsPixels)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                { return; }
            }

            ledMatrix.InvertPixel(x, y);
        }

        /// <summary>
        /// Draw a buffer to the display
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="displayBuffer"></param>
        public void DrawBuffer(int x, int y, IDisplayBuffer displayBuffer)
        {
            ledMatrix.DrawBuffer(x, y, displayBuffer);
        }

        /// <summary>
        /// Clear the display.
        /// </summary>
        /// <param name="fillColor"></param>
        /// <param name="updateDisplay"></param>
        public void Fill(Color fillColor, bool updateDisplay = false)
        {
            ledMatrix.Fill(fillColor, updateDisplay);
        }

        /// <summary>
        /// Clear a region of the display
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fillColor"></param>
        public void Fill(int x, int y, int width, int height, Color fillColor)
        {
            ledMatrix.Fill(x, y, width, height, fillColor);
        }

        /// <summary>
        /// Show changes on the display
        /// </summary>
        public void Show()
        {
            ledMatrix.Show();
        }

        /// <summary>
        /// Update a region of the display
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public void Show(int left, int top, int right, int bottom)
        {
            ledMatrix.Show(left, top, right, bottom);
        }
    }
}