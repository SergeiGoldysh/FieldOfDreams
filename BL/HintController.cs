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
    public class HintController
    {
        private readonly IRepository<Hint> _hintRepository;
        public HintController(IRepository<Hint> hintRepository)
        {
            _hintRepository = hintRepository;
        }

        public async Task<List<HintDto>> GetAllHints()
        {
            List<Hint> hint = await _hintRepository.GetAllAsync();
            if(hint == null)
            {
                return null;
            }

            return hint.Select(h => new HintDto
            {
                Id= h.Id,
                Name= h.Name,
                Description= h.Description,
            }).ToList();
        }

    }
}
