using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using VectorSearchAiAssistant.SemanticKernel.Plugins.Memory;

namespace VectorSearchAiAssistant.SemanticKernel.Plugins.Core
{
    public class PolicyPlugin
    {
        private const double DefaultRelevance = 0.7;
        private const int DefaultLimit = 1;

        private readonly VectorMemoryStore _policyMemory;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance of the TextEmbeddingMemorySkill
        /// </summary>
        public PolicyPlugin(
            VectorMemoryStore policyMemory,
            ILogger logger)
        {
            _policyMemory = policyMemory;
            _logger = logger;
        }

        [SKFunction, Description("Get information about product policies.")]
        public async Task<string> RecallPolicyAsync(
            [Description("The input text to find related memories for")] string text,
            [Description("The relevance score, from 0.0 to 1.0, where 1.0 means perfect match"), DefaultValue(DefaultRelevance)] double? relevance,
            [Description("The maximum number of relevant memories to recall"), DefaultValue(DefaultLimit)] int? limit)
        {
            //ArgumentException.ThrowIfNullOrEmpty(collection, nameof(collection));
            relevance ??= DefaultRelevance;
            limit ??= DefaultLimit;

            var policyMemories = await _policyMemory
                .GetNearestMatches(text, limit.Value, relevance.Value)
                .ToListAsync()
                .ConfigureAwait(false);

            if (policyMemories.Count == 0)
            {
                _logger.LogWarning("no policy memories found");
                return string.Empty;
            }

            var result = policyMemories.Select(m => m.Metadata.Text).FirstOrDefault();
            return result;
        }
    }
}