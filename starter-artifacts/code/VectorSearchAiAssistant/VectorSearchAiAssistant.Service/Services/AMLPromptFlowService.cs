using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VectorSearchAiAssistant.SemanticKernel.Plugins.Memory;
using VectorSearchAiAssistant.Service.Interfaces;
using VectorSearchAiAssistant.Service.Models.Chat;
using VectorSearchAiAssistant.Service.Models.ConfigurationOptions;

namespace VectorSearchAiAssistant.Service.Services;

public class AMLPromptFlowService : IRAGService
{
    readonly VectorMemoryStore _longTermMemory;
    readonly VectorMemoryStore _shortTermMemory;
    private readonly AMLPromptFlowServiceSettings _settings;
    bool _serviceInitialized = false;
    public bool IsInitialized => _serviceInitialized;

    public AMLPromptFlowService(IOptions<AMLPromptFlowServiceSettings> options)
    {
        _settings = options.Value;
        _serviceInitialized = true;
    }

    public async Task AddMemory(object item, string itemName)
    {
        await _longTermMemory.AddMemory(item, itemName);
    }

    public async Task<(string Completion, string UserPrompt, int UserPromptTokens, int ResponseTokens, float[]? UserPromptEmbedding)> GetResponse(string userPrompt, List<Message> messageHistory)
    {
        var response = await SendRequest(userPrompt, messageHistory);
        // TODO: Figure out how to make prompt flow return tokens and embeddings
        return new (response, userPrompt, 0, 0, null);
    }

    public async Task RemoveMemory(object item)
    {
        await _longTermMemory.RemoveMemory(item);
    }

    public Task<string> Summarize(string sessionId, string userPrompt)
    {
        throw new NotImplementedException();
    }

    private async Task<string> SendRequest(string userPrompt, List<Message> messageHistory)
    {
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };
        using var client = new HttpClient(handler);

        var chatHistory = messageHistory.OrderBy(m => m.TimeStamp)
            .Select((m, i) => new { m, i })
            .GroupBy(x => x.i / 2)
            .Select(g => new
            {
                inputs = new
                {
                    chat_input = g.First().m.Text
                },
                outputs = new
                {
                    output = g.Last().m.Text
                }
            });
        var requestBody = new
        {
            chat_input = userPrompt,
            chat_history = chatHistory
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Key);
        client.BaseAddress = new Uri(_settings.Endpoint);

        var content = new StringContent(JsonConvert.SerializeObject(requestBody));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        content.Headers.Add("azureml-model-deployment", _settings.AzureMLModelDeployment);

        var response = await client.PostAsync("", content);//.ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
