using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RagDemo;

public class HistoryStream(Kernel kernel) : IChat
{
    public async Task InitChat()
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatHistory = new("You are an AI assistant that helps people find information.");
        StringBuilder builder = new();

        while (true)
        {
            Console.Write("Question: ");
            chatHistory.AddUserMessage(Console.ReadLine()!);

            builder.Clear();
            await foreach (var message in chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory))
            {
                Console.Write(message);
                builder.Append(message.Content);
            }

            Console.WriteLine();
            chatHistory.AddAssistantMessage(builder.ToString());

            Console.WriteLine();
        }
        // ReSharper disable once FunctionNeverReturns
    }
}