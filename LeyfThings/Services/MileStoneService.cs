using LeyfThings.DTOs;
using LeyfThings.LeyfThingsDB;
using LeyfThings.Models;
using Microsoft.EntityFrameworkCore;

namespace LeyfThings.Services
{
    public class MilestoneService : IMileStoneService
    {
        private readonly AppDbContext _context;

        public MilestoneService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MileStoneDTO>> GetMilestonesByGoalAsync(Guid goalId)
        {
            return await _context.MileStones
                .Where(m => m.GoalId == goalId)
                .Select(m => new MileStoneDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Status = m.Status,
                    DueDate = m.DueDate,
                    GoalId = m.GoalId
                })
                .ToListAsync();
        }

        public async Task<MileStoneDTO?> GetMilestoneAsync(Guid id)
        {
            var m = await _context.MileStones.FindAsync(id);
            if (m == null) return null;

            return new MileStoneDTO
            {
                Id = m.Id,
                Title = m.Title,
                Status = m.Status,
                DueDate = m.DueDate,
                GoalId = m.GoalId
            };
        }

        public async Task<MileStoneDTO> CreateMilestoneAsync(MileStoneDTO dto)
        {
            var goalExists = await _context.Goals.AnyAsync(g => g.Id == dto.GoalId);
            if (!goalExists)
                throw new ArgumentException("Invalid GoalId. Cannot create milestone.");
            var entity = new MileStone
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Status = dto.Status,
                DueDate = dto.DueDate,
                GoalId = dto.GoalId
            };

            _context.MileStones.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id;
            return dto;
        }

        public async Task<bool> UpdateMilestoneAsync(Guid id, MileStoneDTO dto)
        {
            var milestone = await _context.MileStones.FindAsync(id);
            if (milestone == null) return false;

            milestone.Title = dto.Title;
            milestone.Status = dto.Status;
            milestone.DueDate = dto.DueDate;
            milestone.GoalId = dto.GoalId;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateMilestoneStatusAsync(Guid id, string status)
        {
            var milestone = await _context.MileStones.FindAsync(id);
            if (milestone == null) return false;

            milestone.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteMilestoneAsync(Guid id)
        {
            var milestone = await _context.MileStones.FindAsync(id);
            if (milestone == null) return false;

            _context.MileStones.Remove(milestone);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
