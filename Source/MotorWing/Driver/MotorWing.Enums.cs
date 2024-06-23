namespace Meadow.Foundation.FeatherWings
{
    public partial class MotorWing
    {
        public enum DCMotorIndex : byte
        {
            Motor1 = 0,
            Motor2 = 1,
            Motor3 = 2,
            Motor4 = 3
        }

        public enum StepperMotorIndex : byte
        {
            Motor1 = 0,
            Motor2 = 1
        }

        /// <summary>
        /// Motor style
        /// </summary>
        public enum Style
        {
            /// <summary>
            /// Single
            /// </summary>
            SINGLE = 1,
            /// <summary>
            /// Double
            /// </summary>
            DOUBLE = 2,
            /// <summary>
            /// Interleave
            /// </summary>
            INTERLEAVE = 3,
            /// <summary>
            /// Microstep
            /// </summary>
            MICROSTEP = 4
        }

        /// <summary>
        /// Motor direction
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// Forward motor direction
            /// </summary>
            Forward,
            /// <summary>
            /// Reverse/Backward motor direction
            /// </summary>
            Reverse,
            /// <summary>
            /// Release the motor (stepper motor only)
            /// </summary>
            Release
        }
    }
}