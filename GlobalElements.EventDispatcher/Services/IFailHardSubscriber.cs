namespace GlobalElements.EventDispatcherLib.Services
{
    /// <summary>
    /// An event subscriber that fails hard (passing through exception and aborting flow)
    /// </summary>
    public interface IFailHardSubscriber : IEventSubscriber
    {
    }
}
