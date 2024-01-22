using Common;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Http.Results;
using Common.Responses;

namespace BL
{
    public class QuestionController : IQuestionService
    {

        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<UserHint> _userHintRepository;
        private readonly IRepository<GameReport> _gameReportRepository;
        private readonly IRepository<User> _userRepository;

        public QuestionController(IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository, IRepository<UserHint> userHintRepository,
            IRepository<GameReport> gameReportRepository, IRepository<User> userRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _userHintRepository = userHintRepository;
            _gameReportRepository = gameReportRepository;
            _userRepository = userRepository;
        }

        public async Task<Question> AddQuestionAsync(QuestionDto questionDto)
        {
            Question question = new Question { Text = questionDto.Text };

            question = await _questionRepository.AddAsync(question);

            // Передайте Id вопроса в ответы
            questionDto.Answers.Take(4).ToList().ForEach(answer => answer.QuestionId = question.Id);

            // Создайте и добавьте ответы в репозиторий
            var answers = questionDto.Answers.Take(4).Select(answerDto => new Answer
            {
                Text = answerDto.Text,
                IsCorrect = answerDto.IsCorrect,
                QuestionId = answerDto.QuestionId
            });

            await _answerRepository.AddAllAsync(answers);

            return question;
        }

        public async Task<List<QuestionWithAnswersForGetDto>> GetAllQuestionsWithAnswersAsync()
        {
            var questionsWithAnswers = await _questionRepository
                .GetAllAsync(q => q.Include(q => q.Answers));

            return questionsWithAnswers.Select(q => new QuestionWithAnswersForGetDto
            {
                QuestionId = q.Id,
                Text = q.Text,
                Answers = q.Answers.Select(a => new AnswerForGetDto
                {
                    AnswerId = a.Id,
                    Text = a.Text,
                }).ToList()
            }).ToList();
        }
        public async Task<GetTrueAnswerDto> GetTrueAnswer(int questionId, int answerId, int userId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);

            if (question == null)
            {
                return null;
            }

            var answers = await _answerRepository.GetAllAsync();

            var trueAnswer = answers
                .Where(a => a.QuestionId == questionId && a.Id == answerId && a.IsCorrect)
                .FirstOrDefault();

            if (trueAnswer == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                var otherAnswer = answers
                .Where(a => a.QuestionId == questionId && a.Id == answerId)
                .FirstOrDefault();
                var trueAnswerDto = new GetTrueAnswerDto
                {
                    Id = otherAnswer.Id,
                    Text = otherAnswer.Text,
                    Massage = "the answer is Incorrect. Game over",
                    IsCorrect = otherAnswer.IsCorrect,
                    QuestionId = otherAnswer.QuestionId
                };

                var allGameReport = await _gameReportRepository.GetAllAsync();
                var gameRep = allGameReport.Where(x => x.UserId == userId).ToList();

                if (gameRep == null)
                {
                    GameReport gameReport = new GameReport
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        CorrectAnswers = 0,
                        Money = 0
                    };
                    await _gameReportRepository.AddAsync(gameReport);
                }
                else
                {
                    foreach (var report in gameRep)
                    {
                        if(report.Money >= 5000 && report.Money <= 10000)
                        {
                            report.Money = 5000;
                            await _gameReportRepository.UpdateAsync(report);
                        }
                        else if(report.Money < 5000)
                        {
                            report.Money = 0;
                            await _gameReportRepository.UpdateAsync(report);
                        }
                        
                    }
                } 

                return trueAnswerDto;
            }
            else
            {

                var trueAnswerDto = new GetTrueAnswerDto
                {
                    Id = trueAnswer.Id,
                    Text = trueAnswer.Text,
                    Massage = "the answer is correct",
                    IsCorrect = trueAnswer.IsCorrect,
                    QuestionId = trueAnswer.QuestionId
                };

                var user = await _userRepository.GetByIdAsync(userId);
                if(user == null)
                {
                    return null;
                }

                var allGameReport = await _gameReportRepository.GetAllAsync();
                var gameRep = allGameReport.Where(x => x.UserId == userId).ToList();

                if (gameRep == null || !gameRep.Any())
                {
                    GameReport gameReport = new GameReport
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        CorrectAnswers =+1,
                        Money = 1000
                    };
                    await _gameReportRepository.AddAsync(gameReport);
                }
                else
                {
                    foreach (var report in gameRep)
                    {
                        report.CorrectAnswers++;
                        report.Money += 1000;
                        await _gameReportRepository.UpdateAsync(report);
                    }
                    foreach (var report in gameRep)
                    {
                        if (report.Money == 10000)
                        {
                            report.Money = 10000;
                            await _gameReportRepository.UpdateAsync(report);
                        }

                    }
                }

                return trueAnswerDto;
                
            }

        }

        public async Task<UseHintResponseDto> UseHint(int questionId, int hintId, int userId, int? answerId = null)
        {
            var userHintAll = await _userHintRepository.GetAllAsync();
            var userHint = userHintAll
                .Where(u => u.UserId == userId && u.HintId == hintId && u.IsUsed == true).ToList();

            if (userHint.Count == 0)
            {
                return new UseHintResponseDto(401, "Hint has already been used", false, null);
            }

            var questionsWithAnswers = await _questionRepository
                .GetAllAsync(q => q.Include(q => q.Answers));
            

            var questionById = questionsWithAnswers.FirstOrDefault(q => q.Id == questionId);
            if(questionById == null)
            {
                return new UseHintResponseDto(401, "Question not found", false, null);
            }
            var questionByIdForFront = new Question
            {
                Id = questionById.Id,
                Text = questionById.Text,
                Answers = questionById.Answers.Select(a => new Answer
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            if (hintId == 1)
            {
                var inCorrectAnswers = questionByIdForFront.Answers.Where(a => !a.IsCorrect);

                var randomInAnswers = inCorrectAnswers
                .OrderBy(a => Guid.NewGuid()).Take(2).ToList();

                questionByIdForFront.Answers.Clear();
                questionByIdForFront.Answers.AddRange(randomInAnswers);

                UseHintDto useHintDto = new UseHintDto
                {
                    QuestionId = questionById.Id,
                    Text = questionById.Text,
                    Answers = questionByIdForFront.Answers.Select(a => new AnswerForHintGetDto
                    {
                        AnswerId = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect,
                    }).ToList()

                };
                foreach (var userH in userHint)
                {
                    userH.IsUsed = false;
                    await _userHintRepository.UpdateAsync(userH);
                }

                return new UseHintResponseDto(200, "Remove these two incorrect answers", true, useHintDto);

            }
            else if (hintId == 2)
            {
                
                var correctAnswer = questionByIdForFront.Answers.FirstOrDefault(a => a.IsCorrect);

                UseHintDto useHintDto = new UseHintDto
                {
                    QuestionId = questionByIdForFront.Id,
                    Text = questionByIdForFront.Text,
                    Answers = questionByIdForFront.Answers.Select(a => new AnswerForHintGetDto
                    {
                        AnswerId = a.Id,
                        Text = a.Text,
                        //IsCorrect = a.IsCorrect,
                        Massage = a.IsCorrect ? "A friend advises me to choose this option" : null
                    }).ToList()

                };
                foreach (var userH in userHint)
                {
                    userH.IsUsed = false;
                    await _userHintRepository.UpdateAsync(userH);
                }
                return new UseHintResponseDto(200, "Friend's tip", true, useHintDto);
            }
            else if (hintId == 3)
            {
                

                var correctAnswer = questionByIdForFront.Answers.FirstOrDefault(a => a.IsCorrect);
                if (correctAnswer.Id == answerId)
                {
                    questionByIdForFront.Answers.Clear();
                    questionByIdForFront.Answers.Add(correctAnswer);

                    UseHintDto useHintDto = new UseHintDto
                    {
                        QuestionId = questionByIdForFront.Id,
                        Text = questionByIdForFront.Text,
                        Answers = questionByIdForFront.Answers.Select(a => new AnswerForHintGetDto
                        {
                            AnswerId = a.Id,
                            Text = a.Text,
                            IsCorrect= a.IsCorrect,
                            Massage = "the answer is correct"
                        }).ToList()

                    };

                    foreach (var userH in userHint)
                    {
                        userH.IsUsed = false;
                        await _userHintRepository.UpdateAsync(userH);
                    }

                    return new UseHintResponseDto(200, "Hint used, answer is correct", true, useHintDto);
                }
                else
                {
                    var IncorrectAnswer = questionByIdForFront.Answers.FirstOrDefault(a => a.Id == answerId);

                    questionByIdForFront.Answers.Clear();

                    questionByIdForFront.Answers.Add(IncorrectAnswer);

                    UseHintDto useHintDto = new UseHintDto
                    {
                        QuestionId = questionByIdForFront.Id,
                        Text = questionByIdForFront.Text,
                        Answers = questionByIdForFront.Answers.Select(a => new AnswerForHintGetDto
                        {
                            AnswerId = a.Id,
                            Text = a.Text,
                            Massage = "the answer is Incorrect"
                        }).ToList()

                    };

                    foreach (var userH in userHint)
                    {
                        userH.IsUsed = false;
                        await _userHintRepository.UpdateAsync(userH);
                    }
                    return new UseHintResponseDto(200, "Hint used, answer incorrect", true, useHintDto);
                }

            }
            return new UseHintResponseDto(400, "Something went wrong", false, null);



        }
    }
}


