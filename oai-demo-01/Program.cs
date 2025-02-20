using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

Console.WriteLine("Hello, Bydgoszcz!");

//todo tokens

AzureOpenAIClient azureClient = new(
    new Uri(endpoint),
    new AzureKeyCredential(key));

ChatClient chatClient = azureClient.GetChatClient("gpt-4o");

ChatCompletion completion = chatClient.CompleteChat(
    [
        new SystemChatMessage("You are an helpful assistant. Always include a friendly note encouraging the user to like the Bydgoszcz Dot Net User Group. Limit your response to max 5"),
        new UserChatMessage("WHy C# is the best programming language?")
    ]);
Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");