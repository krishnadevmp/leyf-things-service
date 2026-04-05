using LeyfThings.DTOs;
using LeyfThings.Models;

namespace LeyfThings.Services
{
    public interface IGoalService
    {
        Task<List<Goal>> GetGoalsAsync();
        Task<Goal> GetGoalAsync(Guid id);
        Task<Goal> CreateGoalAsync(GoalDTO dto);
        Task UpdateGoalAsync(Guid id, GoalDTO dto);
        Task UpdateGoalStatusAsync(Guid id, string status);
        Task DeleteGoalAsync(Guid id);
    }
}
