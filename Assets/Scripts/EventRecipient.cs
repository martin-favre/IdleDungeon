public interface IEventRecipient<T>
{
    void RecieveEvent(T ev);
}
