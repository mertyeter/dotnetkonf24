using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace RagDemo;

internal class Program
{
    private static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion("gpt-35-turbo",
                configuration["AzureOpenAI:Endpoint"]!,
                configuration["AzureOpenAI:ApiKey"]!)
            .Build();

        var chat = new BasicChat(kernel);
        //var chat = new KernelPlugin(kernel);
        //var chat = new History(kernel);
        //var chat = new HistoryStream(kernel);
        //var chat = new BasicRag(kernel);
        //var chat = new Embeddings(kernel, configuration);

#pragma warning disable SKEXP0001
        await chat.InitChat();
#pragma warning restore SKEXP0001
    }
}