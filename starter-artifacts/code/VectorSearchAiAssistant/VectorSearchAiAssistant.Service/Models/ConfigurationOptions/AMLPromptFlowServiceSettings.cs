namespace VectorSearchAiAssistant.Service.Models.ConfigurationOptions;

public record AMLPromptFlowServiceSettings
{
    public required string Endpoint { get; init; }
    public required string Key { get; init; }
    public required string AzureMLModelDeployment { get; init; }
}
