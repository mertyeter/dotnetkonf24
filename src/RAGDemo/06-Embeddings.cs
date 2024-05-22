using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Text;

namespace RagDemo;

public class Embeddings(Kernel kernel, IConfiguration configuration) : IChat
{
    [Experimental("SKEXP0001")]
    public async Task InitChat()
    {
        var semanticTextMemory = new MemoryBuilder()
            .WithMemoryStore(new QdrantMemoryStore("http://localhost:6333/", 1536))
            .WithAzureOpenAITextEmbeddingGeneration("text-embedding-ada-002",
                configuration["AzureOpenAI:Endpoint"]!,
                configuration["AzureOpenAI:ApiKey"]!)
            .Build();

        var collections = await semanticTextMemory.GetCollectionsAsync();
        const string collectionName = "dotnetsupportdates";
        if (collections.Contains(collectionName))
        {
            Console.WriteLine("Found database");
        }
        else
        {
            using HttpClient client = new();
            var str =
                await client.GetStringAsync(
                    "https://learn.microsoft.com/en-us/lifecycle/products/microsoft-net-and-net-core");

            var paragraphs =
                TextChunker.SplitPlainTextParagraphs(
                    TextChunker.SplitPlainTextLines(
                        WebUtility.HtmlDecode(Regex.Replace(str, @"<[^>]+>|&nbsp;", string.Empty)),
                        128),
                    1024);

            for (var i = 0; i < paragraphs.Count; i++)
                await semanticTextMemory.SaveInformationAsync(collectionName, paragraphs[i], $"paragraph{i}");

            Console.WriteLine("Generated database");
        }

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatHistory = new("You are an AI assistant that helps people find information.");
        StringBuilder builder = new();

        while (true)
        {
            Console.Write("Question: ");
            var question = Console.ReadLine()!;

            builder.Clear();
            await foreach (var result in semanticTextMemory.SearchAsync(collectionName, question, 3))
                builder.AppendLine(result.Metadata.Text);

            var contextToRemove = -1;
            if (builder.Length != 0)
            {
                builder.Insert(0, "Here's some additional information: ");
                contextToRemove = chatHistory.Count;
                chatHistory.AddUserMessage(builder.ToString());
            }

            chatHistory.AddUserMessage(question);

            builder.Clear();
            await foreach (var message in chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory))
            {
                Console.Write(message);
                builder.Append(message.Content);
            }

            Console.WriteLine();
            chatHistory.AddAssistantMessage(builder.ToString());

            if (contextToRemove >= 0) chatHistory.RemoveAt(contextToRemove);
            Console.WriteLine();
        }
        // ReSharper disable once FunctionNeverReturns
    }
}