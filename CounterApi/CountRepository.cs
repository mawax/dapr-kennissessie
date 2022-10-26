using Dapr.Client;

namespace CounterApi;

public class CountRepository : ICountRepository
{
    private const string DAPR_STORE_NAME = "statestore";
    private const string KEY_COUNT = "count";

    private readonly DaprClient _daprClient;

    public CountRepository(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task IncrementCountAsync()
    {
        var count = await GetCountAsync();
        await _daprClient.SaveStateAsync(DAPR_STORE_NAME, KEY_COUNT, count + 1);
    }

    public Task<int> GetCountAsync()
    {
        return _daprClient.GetStateAsync<int>(DAPR_STORE_NAME, KEY_COUNT);
    }

    public async Task IncrementCountAsync(int amount)
    {
        var count = await GetCountAsync();
        await _daprClient.SaveStateAsync(DAPR_STORE_NAME, KEY_COUNT, count + amount);
    }
}
