using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.WebRequestMethods;

namespace CounterFrontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public int Count { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("counter-api");
            Count = await client.GetFromJsonAsync<int>("count");
        }

        public async Task<IActionResult> OnPostWithHttpClientAsync()
        {
            var client = _httpClientFactory.CreateClient("counter-api");
            _ = await client.PostAsync("count/increment", null);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostWithDaprClientAsync()
        {
            using var client = new DaprClientBuilder().Build();
            var methodRequest = client.CreateInvokeMethodRequest(HttpMethod.Post, "counter-api", "count/increment");
            await client.InvokeMethodAsync(methodRequest);
            return RedirectToPage("./Index");
        }
    }
}