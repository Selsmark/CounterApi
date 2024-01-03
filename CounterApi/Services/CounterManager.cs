
namespace CounterApi.Services
{
    public class CounterManager
    {
        private int _counter;

        public CounterManager()
        {
            _counter = 0;
        }

        public int IncrementCounter()
        {
            return Interlocked.Increment(ref _counter);
        }

        public int DecrementCounter()
        {
            return Interlocked.Decrement(ref _counter);
        }

        public int GetCounter()
        {
            return _counter;
        }
    }
}
