using CounterApi;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICountRepository, CountRepository>();
builder.Services.AddSingleton<DaprClient>(new DaprClientBuilder().Build());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.Run();