namespace JsonUtils
{
    /* EventArgs class for the Updated event. */
    public class UpdatedEventArgs : EventArgs
    {
        // Gets the time when the update occurred.
        public DateTime UpdateTime { get; private set; }

        // Initializes a new instance of the UpdatedEventArgs class with the specified update time.
        public UpdatedEventArgs(DateTime updateTime)
        {
            UpdateTime = updateTime;
        }
    }
}