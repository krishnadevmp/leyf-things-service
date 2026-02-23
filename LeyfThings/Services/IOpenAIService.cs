using LeyfThings.DTOs;

namespace LeyfThings.Services
{

    /// <summary>
    /// Interface for Azure OpenAI Service operations.
    /// Enables Dependency Injection and easy unit testing/mocking.
    /// </summary>
    public interface IOpenAIService
    {
        /// <summary>
        /// Extracts structured Goal and Milestone data from a natural language message.
        /// </summary>
        /// <param name="userMessage">The natural language input from the user</param>
        /// <returns>A GoalData object containing the extracted Goal and Milestones</returns>
        Task<GoalDTO> ExtractGoalDataAsync(string userMessage);



    }
}
