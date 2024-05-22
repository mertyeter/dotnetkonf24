using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RagDemo;

public class BasicRag(Kernel kernel) : IChat
{
    public async Task InitChat()
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatHistory = new("You are an AI assistant that helps people find information.");
        StringBuilder builder = new();

        using (HttpClient client = new())
        {
            var str = await client.GetStringAsync(
                "https://learn.microsoft.com/en-us/lifecycle/products/microsoft-net-and-net-core");

            str = WebUtility.HtmlDecode(Regex.Replace(str,
                @"<[^>]+>|&nbsp;",
                string.Empty));

            chatHistory.AddUserMessage("Here's some additional information: " + str);
        }

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