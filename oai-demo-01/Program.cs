using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using static System.Environment;

Console.WriteLine("Hello, Bydgoszcz!");

string endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
string key = GetEnvironmentVariable("AZURE_OPENAI_API_KEY");

AzureOpenAIClient azureClient = new(
    new Uri(endpoint),
    new AzureKeyCredential(key));

// This must match the custom deployment name you chose for your model
ChatClient chatClient = azureClient.GetChatClient("gpt-4o");

ChatCompletion completion = chatClient.CompleteChat(
    [
        new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
        new UserChatMessage("Do other Azure AI services support this too?")
    ]);

Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");