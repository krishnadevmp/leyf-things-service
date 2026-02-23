using LeyfThings.DTOs;
using LeyfThings.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (goal == null) return NotFound();
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
            var success = await _goalService.UpdateGoalAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateGoalStatus(Guid id, [FromBody] StatusUpdateDto dto)
        {
            var success = await _goalService.UpdateGoalStatusAsync(id, dto.Status);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(Guid id)
        {
            var success = await _goalService.DeleteGoalAsync(id);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// General AI chat endpoint
        /// </summary>
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Prompt))
                return BadRequest(new { error = "Please provide a message" });

            try
            {
                var createdGoalDTO = await _openAIService.ExtractGoalDataAsync(request.Prompt);
                if (createdGoalDTO == null)
                    return StatusCode(500, "Cannot create the goal");
                var goal = await _goalService.CreateGoalAsync(createdGoalDTO);
                return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, goal);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { error = "Something went wrong." });
            }
        }
    }

}
