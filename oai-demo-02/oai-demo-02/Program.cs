
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


// Configuration parameters
string endpoint = "https://api.openai.com/v1/engines/davinci-codex/completions";
string apiKey = "YOUR_API_KEY";
string prompt = "Once upon a time";
double temperature = 0.8;

// Create HTTP client
using (HttpClient client = new HttpClient())
{
    // Set authorization header
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

    // Create request payload
    var requestPayload = new
    {
        prompt = prompt,
        temperature = temperature
    };
    var jsonPayload = JsonSerializer.Serialize(requestPayload);
    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    // Send POST request to OpenAI service
    var response = await client.PostAsync(endpoint, content);

    // Handle response
    if (response.IsSuccessStatusCode)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<dynamic>(responseContent);
        string generatedText = responseObject.choices[0].text;
        Console.WriteLine(generatedText);
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
}
