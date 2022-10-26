using CounterApi;
using Dapr;
using Dapr.Client;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICountRepository, CountRepository>();
builder.Services.AddSingleton<DaprClient>(new DaprClientBuilder().Build());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Dapr will send serialized event object vs. being raw CloudEvent
app.UseCloudEvents();

// needed for Dapr pub/sub routing
app.MapSubscribeHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/count", async (ICountRepository countRepository) =>
    Results.Ok(await countRepository.GetCountAsync())
);
app.MapPost("/count/increment", async (ICountRepository countRepository) =>
{
    await countRepository.IncrementCountAsync();
    return Results.Ok();
});

// Dapr subscription in [Topic] routes orders topic to this route
app.MapPost("/orders", [Topic("pubsub", "counts")] async (ICountRepository countRepository, CountEvent countEvent) =>
{
    await countRepository.IncrementCountAsync(countEvent.Amount);
    Console.WriteLine("Count event: " + countEvent);
    return Results.Ok(countEvent);
});


app.Run();


public record CountEvent([property: JsonPropertyName("amount")] int Amount);
