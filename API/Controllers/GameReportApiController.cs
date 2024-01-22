using BL;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameReportApiController : ControllerBase
    {
        private readonly GameReportController _gameReportController;

        public GameReportApiController(GameReportController gameReportController)
        {
            _gameReportController = gameReportController;
        }

        [HttpGet("GetAllGameReport")]
        public async Task<List<GameReportDto>> GetAllGameReport()
        {
            var gameReport = await _gameReportController.GetAllGameReportAsync();
            if (gameReport == null)
            {
                return null;
            }
            return gameReport;
        }

        [HttpGet("GetGameReportByUserId")]
        public async Task<GameReportDto> GetGameReportByUserId(int userId)
        {
            var gameReport = await _gameReportController.GetGameReportByIdAsync(userId);
            if (gameReport == null)
            {
                return null;
            }
            return gameReport;
        }
    }
}
