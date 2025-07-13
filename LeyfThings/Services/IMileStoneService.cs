using LeyfThings.DTOs;

namespace LeyfThings.Services
{
    public interface IMileStoneService
    {
        Task<IEnumerable<MileStoneDTO>> GetMilestonesByGoalAsync(Guid goalId);
        Task<MileStoneDTO?> GetMilestoneAsync(Guid id);
        Task<MileStoneDTO> CreateMilestoneAsync(MileStoneDTO dto);
        Task<bool> UpdateMilestoneAsync(Guid id, MileStoneDTO dto);
        Task<bool> UpdateMilestoneStatusAsync(Guid id, string status);
        Task<bool> DeleteMilestoneAsync(Guid id);
    }
}
