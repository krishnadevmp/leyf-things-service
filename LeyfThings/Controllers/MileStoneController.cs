using LeyfThings.DTOs;
using LeyfThings.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeyfThings.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MilestonesController : ControllerBase
    {
        private readonly IMileStoneService _milestoneService;

        public MilestonesController(IMileStoneService milestoneService)
        {
            _milestoneService = milestoneService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMilestones([FromQuery] Guid goalId)
        {
            var milestones = await _milestoneService.GetMilestonesByGoalAsync(goalId);
            return Ok(milestones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMilestone(Guid id)
        {
            var milestone = await _milestoneService.GetMilestoneAsync(id);
            return milestone == null ? NotFound() : Ok(milestone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMilestone([FromBody] MileStoneDTO dto)
        {
            var milestone = await _milestoneService.CreateMilestoneAsync(dto);
            return CreatedAtAction(nameof(GetMilestone), new { id = milestone.Id }, milestone);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMilestone(Guid id, [FromBody] MileStoneDTO dto)
        {
            var success = await _milestoneService.UpdateMilestoneAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateMilestoneStatus(Guid id, [FromBody] StatusUpdateDto dto)
        {
            var success = await _milestoneService.UpdateMilestoneStatusAsync(id, dto.Status);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilestone(Guid id)
        {
            var success = await _milestoneService.DeleteMilestoneAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
