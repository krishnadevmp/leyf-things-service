using Azure.AI.OpenAI;
using LeyfThings.DTOs;
using LeyfThings.Exceptions;
using OpenAI.Chat;
using System.Text.Json;
using System.ClientModel;

namespace LeyfThings.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentName;

        public OpenAIService(IConfiguration configuration)
        {
            var endpoint = configuration["AzureOpenAI:Endpoint"];
            var apiKey = configuration["AzureOpenAI:ApiKey"];
            _deploymentName = configuration["AzureOpenAI:DeploymentName"];

            _client = new AzureOpenAIClient(
                new Uri(endpoint),
                new ApiKeyCredential(apiKey)
            );
        }

        public async Task<GoalDTO> ExtractGoalDataAsync(string userMessage)
        {
            var systemPrompt = @"
                You are a helpful assistant that extracts goal and milestone
                information from natural language.

                Always respond ONLY with a valid JSON object in this exact format:
                {
                    ""title"": ""Learn docker"",
                    ""description"": ""Learn docker and deploy an app"",
                    ""category"": null,
                    ""targetDate"": ""2026-02-03"",
                    ""priority"": ""medium"",
                    ""status"": ""incomplete"",
                    ""mileStones"": [
                        {
                            ""title"": ""Complete tutorial"",
                            ""dueDate"": ""2026-02-16"",
                            ""status"": ""incomplete"",
                        },
                        {
                            ""title"": ""Complete handson"",
                            ""status"": ""incomplete""
                        }
                    ]
                }
                If any information is missing, use null for that field.
                Never add extra text outside the JSON.";

            ChatClient chatClient = _client.GetChatClient(_deploymentName);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userMessage)
            };

            var options = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 500,
                Temperature = 0.1f
            };

            ChatCompletion completion;
            try
            {
                completion = await chatClient.CompleteChatAsync(messages, options);
            }
            catch (Exception ex)
            {
                throw new ExternalServiceException("Azure OpenAI", "Failed to communicate with Azure OpenAI.", ex);
            }

            var aiReply = completion.Content[0].Text;

            GoalDTO? goalData;
            try
            {
                goalData = JsonSerializer.Deserialize<GoalDTO>(aiReply, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new ExternalServiceException("Azure OpenAI", "Received an invalid response from Azure OpenAI.", ex);
            }

            if (goalData == null)
                throw new ExternalServiceException("Azure OpenAI", "Azure OpenAI returned an empty response.");

            return goalData;
        }
    }
}