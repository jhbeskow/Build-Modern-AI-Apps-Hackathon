using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using System.ComponentModel;
using VectorSearchAiAssistant.SemanticKernel.Plugins.Memory;
using VectorSearchAiAssistant.SemanticKernel.Chat;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;

namespace VectorSearchAiAssistant.SemanticKernel.Plugins.Fun
{
    public class RetailAssistancePlugin
    {
        private const double DefaultRelevance = 0.7;
        private const int DefaultLimit = 1;

        private readonly IChatCompletion _chat;
        private readonly ILogger _logger;
        private readonly ChatHistory _chatHistory;

        /// <summary>
        /// Creates a new instance of the TextEmbeddingMemorySkill
        /// </summary>
        public RetailAssistancePlugin(
            IChatCompletion chat,
            ChatHistory chatHistory,
            ILogger logger)
        {
            _chat = chat;
            _chatHistory = chatHistory;
            _logger = logger;
        }

        [SKFunction, Description("Get chat completion")]
        public async Task<string> GetChatCompletionAsync(
            [Description("The input text from the user")] string userPrompt,
            [Description("The relevance score, from 0.0 to 1.0, where 1.0 means perfect match"), DefaultValue(DefaultRelevance)] double? relevance,
            [Description("The maximum number of relevant memories to recall"), DefaultValue(DefaultLimit)] int? limit)
        {
            //ArgumentException.ThrowIfNullOrEmpty(collection, nameof(collection));
            relevance ??= DefaultRelevance;
            limit ??= DefaultLimit;

            _chatHistory.AddUserMessage(userPrompt);
            var completionResults = await _chat.GetChatCompletionsAsync(_chatHistory);
            var reply = await completionResults[0].GetChatMessageAsync();
            var rawResult = (completionResults[0] as ITextResult).ModelResult.GetOpenAIChatResult();
            return rawResult.Choice.Message.Content;
        }
    }
}