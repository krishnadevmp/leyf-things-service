using LeyfThings.DTOs;
using LeyfThings.Models;

namespace LeyfThings.Services
{
    public interface IGoalService
    {
        Task<List<Goal>> GetGoalsAsync();
        Task<Goal?> GetGoalAsync(Guid id);
        Task<Goal> CreateGoalAsync(GoalDTO dto);
        Task<bool> UpdateGoalAsync(Guid id, GoalDTO dto);
        Task<bool> UpdateGoalStatusAsync(Guid id, string status);
        Task<bool> DeleteGoalAsync(Guid id);
    }
}
