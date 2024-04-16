using System.Net.Http.Headers;

namespace CallRequestResponseService;

class Program
{
    static void Main(string[] args)
    {
        InvokeRequestResponseService().Wait();
    }

    static async Task InvokeRequestResponseService()
    {
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };
        using var client = new HttpClient(handler);
        string requestBody =
            $@"{{
                ""chat_input"": ""List stores""
            }}";

        const string apiKey = "";
        if (string.IsNullOrEmpty(apiKey))
            throw new Exception("A key should be provided to invoke the endpoint");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.BaseAddress = new Uri("https://h7zebqy4uyivrw-ml-ithdf.eastus.inference.ml.azure.com/score");

        var content = new StringContent(requestBody);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        content.Headers.Add("azureml-model-deployment", "h7zebqy4uyivrw-ml-ithdf-1");

        HttpResponseMessage response = await client.PostAsync("", content);//.ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
    }
}