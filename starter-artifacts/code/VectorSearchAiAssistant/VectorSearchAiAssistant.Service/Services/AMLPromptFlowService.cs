using VectorSearchAiAssistant.SemanticKernel.Plugins.Memory;
using VectorSearchAiAssistant.Service.Interfaces;
using VectorSearchAiAssistant.Service.Models.Chat;

namespace VectorSearchAiAssistant.Service.Services;

public class AMLPromptFlowService : IRAGService
{
    readonly VectorMemoryStore _longTermMemory;
    readonly VectorMemoryStore _shortTermMemory;
    bool _serviceInitialized = false;
    public bool IsInitialized => _serviceInitialized;

    public AMLPromptFlowService()
    {
        _serviceInitialized = true;
    }

    public async Task AddMemory(object item, string itemName)
    {
        await _longTermMemory.AddMemory(item, itemName);
    }

    public Task<(string Completion, string UserPrompt, int UserPromptTokens, int ResponseTokens, float[]? UserPromptEmbedding)> GetResponse(string userPrompt, List<Message> messageHistory)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveMemory(object item)
    {
        await _longTermMemory.RemoveMemory(item);
    }

    public Task<string> Summarize(string sessionId, string userPrompt)
    {
        throw new NotImplementedException();
    }
}
