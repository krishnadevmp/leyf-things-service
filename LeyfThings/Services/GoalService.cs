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
                .Include(g => g.MileStones)
                .ToListAsync();
        }

        public async Task<Goal?> GetGoalAsync(Guid id)
        {
            return await _context.Goals
                .Include(g => g.MileStones)
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
                MileStones = dto.MileStones.Select(s => new MileStone
                {
                    Id = Guid.NewGuid(),
                    Title = s.Title,
                    Status = s.Status,
                    DueDate = s.DueDate
                }).ToList()
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task<bool> UpdateGoalAsync(Guid id, GoalDTO dto)
        {
            var goal = await _context.Goals
                .Include(g => g.MileStones)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (goal == null) return false;

            goal.Title = dto.Title;
            goal.Description = dto.Description;
            goal.Category = dto.Category;
            goal.TargetDate = dto.TargetDate;
            goal.Priority = dto.Priority;
            goal.Status = dto.Status;

            // Replace milestones
            _context.MileStones.RemoveRange(goal.MileStones);
            goal.MileStones = dto.MileStones.Select(s => new MileStone
            {
                Id = Guid.NewGuid(),
                Title = s.Title,
                Status = s.Status,
                DueDate = s.DueDate
            }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateGoalStatusAsync(Guid id, string status)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return false;
            }

            goal.Status = status;
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
