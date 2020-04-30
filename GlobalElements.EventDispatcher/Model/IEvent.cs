namespace GlobalElements.EventDispatcherLib.Model
{
    public interface IEvent
    {
        /// <summary>
        /// The name of the event
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// If true, no more listeners will be called
        /// </summary>
        bool IsPropagationStopped { get; set; }

        /// <summary>
        /// Set IsPropagationStopped to true
        /// </summary>
        void StopPropagation();
    }
}