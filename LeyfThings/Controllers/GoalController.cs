using LeyfThings.DTOs;
using LeyfThings.Exceptions;
using LeyfThings.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeyfThings.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IOpenAIService _openAIService;

        public GoalsController(IGoalService goalService, IOpenAIService openAIService)
        {
            _goalService = goalService;
            _openAIService = openAIService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGoals()
        {
            var goals = await _goalService.GetGoalsAsync();
            return Ok(goals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGoal(Guid id)
        {
            var goal = await _goalService.GetGoalAsync(id);
            return Ok(goal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] GoalDTO dto)
        {
            var goal = await _goalService.CreateGoalAsync(dto);
            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, goal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] GoalDTO dto)
        {
            await _goalService.UpdateGoalAsync(id, dto);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateGoalStatus(Guid id, [FromBody] StatusUpdateDto dto)
        {
            await _goalService.UpdateGoalStatusAsync(id, dto.Status);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(Guid id)
        {
            await _goalService.DeleteGoalAsync(id);
            return NoContent();
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Prompt))
                throw new ValidationException("Prompt is required.");

            var createdGoalDTO = await _openAIService.ExtractGoalDataAsync(request.Prompt);
            var goal = await _goalService.CreateGoalAsync(createdGoalDTO);
            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, goal);
        }
    }
}
