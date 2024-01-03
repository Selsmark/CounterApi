using Microsoft.AspNetCore.Mvc;

namespace CounterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CounterController : ControllerBase
    {
        private readonly ILogger<CounterController> _logger;

        public CounterController(ILogger<CounterController> logger)
        {
            _logger = logger;
        }

        private int counter = 0;

        [HttpGet(Name = "Get")]
        public int Get()
        {
            return counter;
        }

        [HttpGet(Name = "Increment")]
        public int Increment()
        {
            return counter++;
        }

        [HttpGet(Name = "Decrement")]
        public int Decrement()
        {
            return counter--;
        }
    }
}
