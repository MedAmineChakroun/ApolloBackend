namespace ApolloBackend.Services
{
    public class JsonResponseData
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = "";
        public object? Data { get; set; } = null;
    }
}
