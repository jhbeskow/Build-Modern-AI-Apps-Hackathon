from promptflow import tool, output_metrics, promptflow

@tool
def output_metrics(chat_with_context_output: str) -> str:
    promptflow.runs.get_metrics()
    return metric
# def output_metrics(chat_with_context_output: Dict[str, int]) -> Dict[str, int]:
#     tokens = chat_with_context_output[0]['system_metrics']['completion_tokens']
#     return {"completion_tokens": tokens}