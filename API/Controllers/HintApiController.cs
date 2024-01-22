using BL;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HintApiController : ControllerBase
    {
        private readonly HintController _hintController;

        public HintApiController(HintController hintController)
        {
            _hintController = hintController;
        }

        [HttpGet("GetAllHints")]
        public async Task<List<HintDto>> GetAllHints()
        {
            var allHints = await _hintController.GetAllHints();
            if(allHints == null)
            {
                return null;
            }
            return allHints;
        }
    }
}
