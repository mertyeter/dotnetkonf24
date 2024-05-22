using Microsoft.SemanticKernel;

namespace RagDemo;

public class BasicChat(Kernel kernel) : IChat
{
    public async Task InitChat()
    {
        while (true)
        {
            Console.Write("Question: ");
            Console.WriteLine(await kernel.InvokePromptAsync(Console.ReadLine()!));
            Console.WriteLine();
        }
        // ReSharper disable once FunctionNeverReturns
    }
}