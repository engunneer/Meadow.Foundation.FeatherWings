using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using System;
using System.Threading;
using static Meadow.Foundation.FeatherWings.MotorWing;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents a Stepper Motor
    /// </summary>
    public class StepperMotor
    {
        private readonly IPwmPort portA;
        private readonly IPwmPort portB;

        int currentStep;
        double rpmDelay;
        readonly int motorSteps;

        private readonly IPwmPort pwmPortA;
        private readonly IDigitalOutputPort in1A;
        private readonly IDigitalOutputPort in2A;

        private readonly IPwmPort pwmPortB;
        private readonly IDigitalOutputPort in1B;
        private readonly IDigitalOutputPort in2B;

        const short MICROSTEPS = 8;
        readonly byte[] microStepCurve = { 0, 50, 98, 142, 180, 212, 236, 250, 255 };

        /// <summary>
        ///  a Stepper motor object
        /// </summary>
        /// <param name="steps">The number of steps per revolution</param>
        /// <param name="num">The Stepper motor port</param>
        /// <param name="pca9685">The PCS9685 diver object</param>
        public StepperMotor(int steps, StepperMotorIndex motorIndex, Pca9685 pca9685)
        {
            if (motorIndex == StepperMotorIndex.Motor1)
            {
                pwmPortA = pca9685.Pins.LED8.CreatePwmPort(pca9685.Frequency);
                in1A = pca9685.Pins.LED10.CreateDigitalOutputPort();
                in2A = pca9685.Pins.LED9.CreateDigitalOutputPort();

                pwmPortB = pca9685.Pins.LED13.CreatePwmPort(pca9685.Frequency);
                in1B = pca9685.Pins.LED11.CreateDigitalOutputPort();
                in2B = pca9685.Pins.LED12.CreateDigitalOutputPort();
            }
            else if (motorIndex == StepperMotorIndex.Motor2)
            {
                pwmPortA = pca9685.Pins.LED2.CreatePwmPort(pca9685.Frequency);
                in1A = pca9685.Pins.LED4.CreateDigitalOutputPort();
                in2A = pca9685.Pins.LED3.CreateDigitalOutputPort();

                pwmPortB = pca9685.Pins.LED7.CreatePwmPort(pca9685.Frequency);
                in1B = pca9685.Pins.LED5.CreateDigitalOutputPort();
                in2B = pca9685.Pins.LED6.CreateDigitalOutputPort();
            }
            else
            {
                throw new ArgumentException("Stepper num must be 0 or 1");
            }

            motorSteps = steps;
            SetSpeed(15);
            currentStep = 0;
        }

        /// <summary>
        /// Set the delay for the Stepper Motor speed in RPM
        /// </summary>
        /// <param name="rpm">The desired RPM</param>
        public void SetSpeed(short rpm)
        {
            rpmDelay = 60000.0 / (motorSteps * rpm);
        }

        /// <summary>
        /// Move the stepper with the given RPM
        /// </summary>
        /// <param name="steps">The number of steps to move. Negative number moves the stepper backwards</param>
        /// <param name="style">How to perform the step</param>
        public virtual void Step(int steps = 1, Style style = Style.SINGLE)
        {
            if (steps > 0)
            {
                Step(steps, Direction.Forward, style);
            }
            else
            {
                Step(Math.Abs(steps), Direction.Reverse, style);
            }
        }

        /// <summary>
        /// Move the stepper with the given RPM
        /// </summary>
        /// <param name="steps">The number of steps to move</param>
        /// <param name="direction">The direction to go</param>
        /// <param name="style">How to perform the step</param>
        protected virtual void Step(int steps, Direction direction, Style style)
        {
            int delay = (int)rpmDelay;
            if (style == Style.INTERLEAVE)
            {
                delay /= 2;
            }
            else if (style == Style.MICROSTEP)
            {
                delay /= MICROSTEPS;
                steps *= MICROSTEPS;
            }

            while (steps >= 0)
            {
                Step(direction, style);
                Thread.Sleep(delay);
                steps--;
            }
        }

        /// <summary>
        /// Move the stepper one step only
        /// </summary>
        /// <param name="direction">The direction to go</param>
        /// <param name="style">How to perform the step</param>
        /// <returns>The current location</returns>
        protected virtual int Step(Direction direction, Style style)
        {
            int ocrb, ocra;
            ocra = ocrb = 255;

            if (style == Style.SINGLE)
            {
                if (currentStep / (MICROSTEPS / 2) % 2 != 0) // we're at an odd step, weird
                {
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS / 2;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS / 2;
                    }
                }
                else
                { // go to the next even step
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS;
                    }
                }
            }
            else if (style == Style.DOUBLE)
            {
                if ((currentStep / (MICROSTEPS / 2) % 2) != 0)
                { // we're at an even step, weird
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS / 2;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS / 2;
                    }
                }
                else
                { // go to the next odd step
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS;
                    }
                }
            }
            else if (style == Style.INTERLEAVE)
            {
                if (direction == Direction.Forward)
                {
                    currentStep += MICROSTEPS / 2;
                }
                else
                {
                    currentStep -= MICROSTEPS / 2;
                }
            }
            else if (style == Style.MICROSTEP)
            {
                if (direction == Direction.Forward)
                {
                    currentStep++;
                }
                else
                {
                    currentStep--;
                }

                currentStep += MICROSTEPS * 4;
                currentStep %= MICROSTEPS * 4;
                ocra = ocrb = 0;

                if ((currentStep >= 0) && (currentStep < MICROSTEPS))
                {
                    ocra = microStepCurve[MICROSTEPS - currentStep];
                    ocrb = microStepCurve[currentStep];
                }
                else if ((currentStep >= MICROSTEPS) && (currentStep < MICROSTEPS * 2))
                {
                    ocra = microStepCurve[currentStep - MICROSTEPS];
                    ocrb = microStepCurve[MICROSTEPS * 2 - currentStep];
                }
                else if ((currentStep >= MICROSTEPS * 2) &&
                         (currentStep < MICROSTEPS * 3))
                {
                    ocra = microStepCurve[MICROSTEPS * 3 - currentStep];
                    ocrb = microStepCurve[currentStep - MICROSTEPS * 2];
                }
                else if ((currentStep >= MICROSTEPS * 3) &&
                         (currentStep < MICROSTEPS * 4))
                {
                    ocra = microStepCurve[currentStep - MICROSTEPS * 3];
                    ocrb = microStepCurve[MICROSTEPS * 4 - currentStep];
                }
            }

            currentStep += MICROSTEPS * 4;
            currentStep %= MICROSTEPS * 4;

            pwmPortA.DutyCycle = ocra / 255.0;
            pwmPortB.DutyCycle = ocrb / 255.0;

            // release all
            int latch_state = 0; // all motor pins to 0

            // Serial.println(step, DEC);
            if (style == Style.MICROSTEP)
            {
                if ((currentStep >= 0) && (currentStep < MICROSTEPS))
                    latch_state |= 0x03;
                if ((currentStep >= MICROSTEPS) && (currentStep < MICROSTEPS * 2))
                    latch_state |= 0x06;
                if ((currentStep >= MICROSTEPS * 2) && (currentStep < MICROSTEPS * 3))
                    latch_state |= 0x0C;
                if ((currentStep >= MICROSTEPS * 3) && (currentStep < MICROSTEPS * 4))
                    latch_state |= 0x09;
            }
            else
            {
                switch (currentStep / (MICROSTEPS / 2))
                {
                    case 0:
                        latch_state |= 0x1; // energize coil 1 only
                        break;
                    case 1:
                        latch_state |= 0x3; // energize coil 1+2
                        break;
                    case 2:
                        latch_state |= 0x2; // energize coil 2 only
                        break;
                    case 3:
                        latch_state |= 0x6; // energize coil 2+3
                        break;
                    case 4:
                        latch_state |= 0x4; // energize coil 3 only
                        break;
                    case 5:
                        latch_state |= 0xC; // energize coil 3+4
                        break;
                    case 6:
                        latch_state |= 0x8; // energize coil 4 only
                        break;
                    case 7:
                        latch_state |= 0x9; // energize coil 1+4
                        break;
                }
            }

            if ((latch_state & 0x1) == 0x1)
            {
                in1A.State = true;
            }
            else
            {
                in1A.State = false;
            }

            if ((latch_state & 0x2) == 0x2)
            {
                in1B.State = true;
            }
            else
            {
                in1B.State = false;
            }

            if ((latch_state & 0x4) == 0x4)
            {
                in2A.State = true;
            }
            else
            {
                in2A.State = false;
            }

            if ((latch_state & 0x8) == 0x8)
            {
                in2B.State = true;
            }
            else
            {
                in2B.State = false;
            }

            return currentStep;
        }
    }
}