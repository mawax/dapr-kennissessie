using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("counter-api", client =>
{
    var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") +
        ":" + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");
    client.BaseAddress = new Uri(baseURL);
    client.DefaultRequestHeaders.Add("dapr-app-id", "counter-api");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
