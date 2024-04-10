using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Orchestration;
using System.ComponentModel;

namespace VectorSearchAiAssistant.SemanticKernel.Plugins.Core
{
    public class PolicyPlugin2
    {
        private readonly ISKFunction _translateConversation;
        private readonly IKernel _kernel;

        private static string promptTemplate = "";

        public PolicyPlugin2(
            int maxTokens,
            IKernel kernel)
        {
            _kernel = kernel;
            _translateConversation = kernel.CreateSemanticFunction(
                promptTemplate,
                pluginName: nameof(PolicyPlugin2),
                description: "Given a text, return the text translated into a PolicyPlugin2an Poem using Iambic Pentameter",
                requestSettings: new OpenAIRequestSettings
                {
                    MaxTokens = maxTokens,
                    Temperature = 0.1,
                    TopP = 0.5
                });
        }

        [SKFunction, Description("Get information about product policies.")]
        public async Task<string> GetCompanyPolicyAsync(
            string text)
        {
            var result = await _kernel.RunAsync(text, _translateConversation);
            return result.GetValue<string>() ?? string.Empty;
        }
    }
}
