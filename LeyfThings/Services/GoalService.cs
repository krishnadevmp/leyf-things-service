using LeyfThings.DTOs;
using LeyfThings.LeyfThingsDB;
using LeyfThings.Models;
using Microsoft.EntityFrameworkCore;

namespace LeyfThings.Services
{
    // Services/GoalService.cs
    public class GoalService : IGoalService
    {
        private readonly AppDbContext _context;

        public GoalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Goal>> GetGoalsAsync()
        {
            return await _context.Goals
                .Include(g => g.SubGoals)
                .ToListAsync();
        }

        public async Task<Goal?> GetGoalAsync(Guid id)
        {
            return await _context.Goals
                .Include(g => g.SubGoals)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Goal> CreateGoalAsync(GoalDTO dto)
        {
            var goal = new Goal
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                TargetDate = dto.TargetDate,
                Priority = dto.Priority,
                Status = dto.Status,
                SubGoals = dto.SubGoals.Select(s => new SubGoals
                {
                    Id = Guid.NewGuid(),
                    Title = s.Title,
                    IsComplete = s.IsComplete,
                    DueDate = s.DueDate
                }).ToList()
            };

            // Auto-calculate progress
            goal.Progress = goal.SubGoals.Count == 0 ? 0 :
                (int)(goal.SubGoals.Count(s => s.IsComplete) * 100.0 / goal.SubGoals.Count);

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task<bool> UpdateGoalAsync(Guid id, GoalDTO dto)
        {
            var goal = await _context.Goals
                .Include(g => g.SubGoals)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (goal == null) return false;

            goal.Title = dto.Title;
            goal.Description = dto.Description;
            goal.Category = dto.Category;
            goal.TargetDate = dto.TargetDate;
            goal.Priority = dto.Priority;
            goal.Status = dto.Status;

            // Replace subgoals
            _context.SubGoals.RemoveRange(goal.SubGoals);
            goal.SubGoals = dto.SubGoals.Select(s => new SubGoals
            {
                Id = Guid.NewGuid(),
                Title = s.Title,
                IsComplete = s.IsComplete,
                DueDate = s.DueDate
            }).ToList();

            // Recalculate progress
            goal.Progress = goal.SubGoals.Count == 0 ? 0 :
                (int)(goal.SubGoals.Count(s => s.IsComplete) * 100.0 / goal.SubGoals.Count);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGoalAsync(Guid id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null) return false;

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
