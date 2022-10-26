using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;

namespace CounterFrontend.Pages
{
    public class PublishMessageModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var client = new DaprClientBuilder().Build();

            // Publish an event/message using Dapr PubSub
            await client.PublishEventAsync("pubsub", "counts", new CountEvent(10));
            return RedirectToPage("./Index");
        }

        public record CountEvent([property: JsonPropertyName("amount")] int Amount);

    }
}
