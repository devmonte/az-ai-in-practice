using System.Text;
using System.Text.Json;

using HttpClient client = new HttpClient();

// Get bearer token from Microsoft
//todo tokens
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
// Configuration parameters
string oaiEndpoint = "https://oai-demo-02.openai.azure.com/openai/deployments/gpt-4/chat/completions?api-version=2024-08-01-preview";
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

var messages = new List<object>
            {
                new {
                    role = "system",
                    content = new object[]
                    {
                        new {
                            type = "text",
                            text = "You are an helpful assistant. Limit your response to max 5 sentences."
                        }
                    }
                },
                new {
                    role = "user",
                    content = new object[]
                    {
                        new {
                            type = "text",
                            text = "Why C# is the best?"
                        }
                    }
                }
            };

double temperature = 0.8;
double topP = 0.9;
var requestPayload = new
{
    messages = messages.ToArray(),
    temperature,
    top_p = topP,
    max_tokens = 100
};
var jsonPayload = JsonSerializer.Serialize(requestPayload);
var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

var response = await client.PostAsync(oaiEndpoint, content);

if (response.IsSuccessStatusCode)
{
    var responseContent = await response.Content.ReadAsStringAsync();
    var responseObject = JsonSerializer.Deserialize<dynamic>(responseContent);
    Console.WriteLine(JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true }));
}
else
{
    Console.WriteLine($"Error: {response.StatusCode}");
    Console.WriteLine(await response.Content.ReadAsStringAsync());
}
