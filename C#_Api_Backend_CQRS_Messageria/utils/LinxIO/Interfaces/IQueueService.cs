namespace LinxIO.Interfaces
{
    public interface IQueueService
    {
        void SendMessage(string queueName, string message);
    }
}