namespace GlobalElements.EventDispatcherLib.Model
{
    /// <summary>
    /// The event priority. Lower value means higher priority
    /// </summary>
    public class EventPriority
    {
        /// <summary>
        /// The value of "highest priority"
        /// </summary>
        public const short Max = short.MinValue;

        /// <summary>
        /// The default priority
        /// </summary>
        public const short Default = 0;

        /// <summary>
        /// The value of "lowest priority"
        /// </summary>
        public const short Min = short.MaxValue;
    }
}