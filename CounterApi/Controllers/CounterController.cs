using CounterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CounterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private readonly ILogger<CounterController> _logger;
        private readonly CounterManager _counterManager;

        public CounterController(ILogger<CounterController> logger, CounterManager counterManager)
        {
            _logger = logger;
            _counterManager = counterManager;
        }

        [HttpGet("Get")]
        public int Get()
        {
            return _counterManager.GetCounter();
        }

        [HttpGet("Increment")]
        public int Increment()
        {
            return _counterManager.IncrementCounter();
        }

        [HttpGet("Decrement")]
        public int Decrement()
        {
            return _counterManager.DecrementCounter();
        }
    }
}
