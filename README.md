# RagDemo

This repository contains an implementation of the [`Demystifying Retrieval Augmented Generation with .NET`](https://devblogs.microsoft.com/dotnet/demystifying-retrieval-augmented-generation-with-dotnet/). Each C# file represents a specific component or module of the project. 

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Azure OpenAI Service](https://azure.microsoft.com/en-gb/services/cognitive-services/openai-service/)

### Files

Below is a brief description of each file:

- `01-BasicChat.cs`: This file contains the implementation of the [`BasicChat`](src/RAGDemo/01-BasicChat.cs) class, which handles basic chat functionality using the Semantic Kernel.

- `02-KernelPlugin.cs`: The [`KernelPlugin`](src/RAGDemo/02-KernelPlugin.cs) class in this file is responsible for demonstrating how to use plugins with the Semantic Kernel.

- `03-History.cs`: In this file, you will find the [`History`](src/RAGDemo/03-History.cs) class that deals with maintaining chat history and using it to generate responses.

- `04-HistoryStream.cs`: This file contains the implementation of the `HistoryStream` class, which extends the chat history functionality with streaming capabilities.

- `05-BasicRag.cs`: The [`BasicRag`](src/RAGDemo/05-BasicRag.cs) class in this file demonstrates the use of Retrieval-Augmented Generation (RAG) techniques.

- `06-Embeddings.cs`: This file contains the implementation of the `Embeddings` class, which handles embedding generation and usage.

- `IChat.cs`: This file defines the [`IChat`](src/RAGDemo/IChat.cs) interface, which is implemented by various chat classes.

- `Program.cs`: The entry point of the application. This file contains the [`Program`](src/RAGDemo/Program.cs) class, which sets up the kernel and runs the chat application.

You can switch between different chat implementations by uncommenting the respective line in the Program.cs file.

### License
This project is licensed under the MIT License. See the LICENSE file for details.