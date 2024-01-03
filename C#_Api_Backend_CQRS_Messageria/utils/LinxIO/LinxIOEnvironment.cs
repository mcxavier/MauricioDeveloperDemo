namespace LinxIO
{
    public static class LinxIOEnvironment
    {
        public const string BaseAddress = "https://api.linxio.net";
        public const string TokenEndpoint = "/v1/token";

        public static string GetListagemEndpoint(string applicationId)
        {
            return "/v1/applications/" + applicationId + "/queues";
        }

        public static string GetQueueConsumeEndpoint(string queueId)
        {
            return "/v1/queues/" + queueId + "/consume";
        }

        public static string GetSubrscribeTopic(string topicId, string applicationId)
        {
            return "/v1/topics/" + topicId.Trim() + "/applications/" + applicationId + "/subscribe";
        }

        public static string GetPublishEndpoint(string applicationId, string topicId)
        {
            return "v1/topics/" + topicId + "/applications/" + applicationId + "/publish";
        }

        public static string GetSendAckEndpoint(string queueId)
        {
            return "v1/queues/" + queueId + "/receipts/ack";
        }
    }
}