namespace CounterApi
{
    public interface ICountRepository
    {
        Task<int> GetCountAsync();
        Task IncrementCountAsync();
    }
}