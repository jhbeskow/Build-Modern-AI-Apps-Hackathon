using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Orchestration;
using System.ComponentModel;

namespace VectorSearchAiAssistant.SemanticKernel.Plugins.Core
{
    public class ShakespearePlugin
    {
        private readonly ISKFunction _translateConversation;
        private readonly IKernel _kernel;

        public ShakespearePlugin(
            string promptTemplate,
            int maxTokens,
            IKernel kernel)
        {
            _kernel = kernel;
            _translateConversation = kernel.CreateSemanticFunction(
                promptTemplate,
                pluginName: nameof(ShakespearePlugin),
                description: "Given a text, return the text translated into a Shakespearean Poem using Iambic Pentameter",
                requestSettings: new OpenAIRequestSettings
                {
                    MaxTokens = maxTokens,
                    Temperature = 0.1,
                    TopP = 0.5
                });
        }

        [SKFunction]
        public async Task<string> SummarizeTextAsync(
            string text)
        {
            var result = await _kernel.RunAsync(text, _translateConversation);
            return result.GetValue<string>() ?? string.Empty;
        }
    }
}
