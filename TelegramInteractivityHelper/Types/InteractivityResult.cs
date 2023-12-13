namespace Interactivity.Types
{
    public class InteractivityResult<T>
    {
        /// <summary>
        /// The actual result.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Indicates whether the process has timed out.
        /// </summary>
        public bool IsTimedOut { get; set; }

        /// <summary>
        /// Indicates whether the process was Interrupted.
        /// </summary>
        public bool IsInterrupted { get; set; }

        /// <summary>
        /// Indicates whether or not an error has occured.
        /// </summary>
        public bool IsError { get => Value == null && !IsTimedOut; }

        /// <summary>
        /// Create a new InteractivityResult
        /// </summary>
        /// <param name="value">The value of the result (nullable)</param>
        /// <param name="isTimedOut">Has the process timed out?</param>
        public InteractivityResult(T value, bool isTimedOut, bool isInterrupted)
        {
            this.Value = value;
            this.IsTimedOut = isTimedOut;
            this.IsInterrupted = isInterrupted;
        }

    }
}
