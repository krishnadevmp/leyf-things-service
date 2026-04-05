using LeyfThings.DTOs;

namespace LeyfThings.Services
{
    public interface IMileStoneService
    {
        Task<IEnumerable<MileStoneDTO>> GetMilestonesByGoalAsync(Guid goalId);
        Task<MileStoneDTO> GetMilestoneAsync(Guid id);
        Task<MileStoneDTO> CreateMilestoneAsync(MileStoneDTO dto);
        Task UpdateMilestoneAsync(Guid id, MileStoneDTO dto);
        Task UpdateMilestoneStatusAsync(Guid id, string status);
        Task DeleteMilestoneAsync(Guid id);
    }
}
