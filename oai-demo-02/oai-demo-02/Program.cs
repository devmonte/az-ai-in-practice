using System.Text;
using System.Text.Json;

// Configuration parameters
string oaiEndpoint = "https://oai-demo-02.openai.azure.com/openai/deployments/gpt-4/chat/completions?api-version=2024-08-01-preview";
double temperature = 0.8;

// Create HTTP client
using HttpClient client = new HttpClient();

// Get bearer token from Microsoft
var tokenEndpoint = "https://login.microsoftonline.com/f5bdc02d-36c8-4c9c-96b9-791245d763f9/oauth2/v2.0/token";
//client id and secret
var tokenRequestContent = new FormUrlEncodedContent(new[]
{
    new KeyValuePair<string, string>("grant_type", "client_credentials"),
    new KeyValuePair<string, string>("client_id", clientId),
    new KeyValuePair<string, string>("client_secret", clientSecret),
    new KeyValuePair<string, string>("scope", "https://cognitiveservices.azure.com/.default")
});

var tokenResponse = await client.PostAsync(tokenEndpoint, tokenRequestContent);
var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
var tokenObject = JsonSerializer.Deserialize<JsonElement>(tokenResponseContent);
var bearerToken = tokenObject.GetProperty("access_token").GetString();

// Set authorization header
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

var messages = new List<object>
            {
                new {
                    role = "system",
                    content = new object[]
                    {
                        new {
                            type = "text",
                            text = "You are an AI assistant that helps people find information."
                        }
                    }
                },
                new {
                    role = "assistant",
                    content = new object[]
                    {
                        new {
                            type = "text",
                            text = "Hello! How can I assist you today?"
                        }
                    }
                }
            };

// Create request payload
var requestPayload = new
{
    messages = messages.ToArray(),
    temperature = temperature
};
var jsonPayload = JsonSerializer.Serialize(requestPayload);
var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

// Send POST request to OpenAI service
var response = await client.PostAsync(oaiEndpoint, content);

// Handle response
if (response.IsSuccessStatusCode)
{
    var responseContent = await response.Content.ReadAsStringAsync();
    var responseObject = JsonSerializer.Deserialize<dynamic>(responseContent);
    //string generatedText = responseObject.GetProperty("choices")[0].GetProperty("text").GetString();
    Console.WriteLine(JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true }));
}
else
{
    Console.WriteLine($"Error: {response.StatusCode}");
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}

