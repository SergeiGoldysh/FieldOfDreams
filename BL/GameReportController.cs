using Common;
using Models.Entities;
using Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class GameReportController
    {
        private readonly IRepository<GameReport> _gameReportRepository;

        public GameReportController(IRepository<GameReport> gameReportRepository)
        {
            _gameReportRepository = gameReportRepository;
        }

        public async Task<GameReportDto> GetGameReportByIdAsync(int userId)
        {
            var gameReports = await _gameReportRepository.GetByIdAsync(userId);
            if(gameReports == null)
            {
                return null;
            }
            var gameReportDto = new GameReportDto
            {
                UserId = gameReports.UserId,
                UserName = gameReports.UserName,
                CorrectAnswers = gameReports.CorrectAnswers,
                Money = gameReports.Money,
                Massage = $"You answered {gameReports.CorrectAnswers} correct answers. Your winnings are {gameReports.Money}"
            };
            return gameReportDto;
        }

        public async Task<List<GameReportDto>> GetAllGameReportAsync()
        {
            var gameReports = await _gameReportRepository.GetAllAsync();
            if (gameReports == null)
            {
                return null;
            }

            return gameReports.Select(h => new GameReportDto
            {
                Id = h.Id,
                UserId = h.UserId,
                UserName = h.UserName,
                CorrectAnswers = h.CorrectAnswers,
                Money = h.Money,
                Massage = ""
            }).ToList();

        }
    }
}
