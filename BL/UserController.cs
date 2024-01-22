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
    public class UserController
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserHint> _userHintRepository;
        private readonly IRepository<Hint> _hintRepository;
        public UserController(IRepository<User> userRepository,
            IRepository<UserHint> userHintRepository, IRepository<Hint> hintRepository)
        {
            _userRepository = userRepository;
            _userHintRepository = userHintRepository;
            _hintRepository = hintRepository;
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                EmailUser = userDto.EmailUser,
                Password = userDto.Password,
            };

            user = await _userRepository.AddAsync(user);

            var hint = await _hintRepository.GetAllAsync();
            var hintFirst = hint.FirstOrDefault(h => h.Id == 1);
            var hintSecond = hint.FirstOrDefault(h => h.Id == 2);
            var hintThird = hint.FirstOrDefault(h => h.Id == 3);

            var userHint = new List<UserHint>
            {
                new UserHint
                {
                    UserId = user.Id,
                    HintId = hintFirst.Id,
                    IsUsed = true,
                },
                new UserHint
                {
                    UserId = user.Id,
                    HintId = hintSecond.Id,
                    IsUsed = true,
                },
                new UserHint
                {
                    UserId = user.Id,
                    HintId = hintThird.Id,
                    IsUsed = true,
                }

            };
            user.UserHints.AddRange(userHint);
            await _userHintRepository.AddAllAsync(userHint);
            return user;

        }

    }
}
