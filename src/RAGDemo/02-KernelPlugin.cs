using Microsoft.SemanticKernel;

namespace RagDemo;

public class KernelPlugin(Kernel kernel) : IChat
{
    public async Task InitChat()
    {
        kernel.ImportPluginFromFunctions("DateTimeHelpers",
        [
            kernel.CreateFunctionFromMethod(() => $"{DateTime.UtcNow:r}",
                functionName: "Now",
                description: "Gets the current date and time")
        ]);

        var kernelFunction = kernel.CreateFunctionFromPrompt("""
                                                             The current date and time is {{ datetimehelpers.now }}.
                                                             {{ $input }}
                                                             """);

        var kernelArguments = new KernelArguments();

        while (true)
        {
            Console.Write("Question: ");
            kernelArguments["input"] = Console.ReadLine();
            Console.WriteLine(await kernelFunction.InvokeAsync(kernel, kernelArguments));
            Console.WriteLine();
        }
        // ReSharper disable once FunctionNeverReturns
    }
}