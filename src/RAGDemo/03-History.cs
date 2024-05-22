using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RagDemo;

public class History(Kernel kernel) : IChat
{
    public async Task InitChat()
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatHistory = new("You are an AI assistant that helps people find information.");

        while (true)
        {
            Console.Write("Question: ");
            chatHistory.AddUserMessage(Console.ReadLine()!);

            var answer = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
            chatHistory.AddAssistantMessage(answer.Content!);
            Console.WriteLine(answer);

            Console.WriteLine();
        }
        // ReSharper disable once FunctionNeverReturns
    }
}