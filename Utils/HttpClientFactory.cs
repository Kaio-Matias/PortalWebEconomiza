namespace PortalWebEconomiza.Utils
{
    public class HttpClientFactory
    {
        private readonly string _appToken;

        public HttpClientFactory(string appToken)
        {
            _appToken = appToken ?? throw new ArgumentNullException(nameof(appToken));
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("AppToken", _appToken);
            return client;
        }
    }
}
