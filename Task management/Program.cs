using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add controller support (for your API controllers)
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();


// Serve index.html by default + allow static files (from wwwroot)
app.UseDefaultFiles();
app.UseStaticFiles();

// Optional: enable HTTPS redirection (if you want)
app.UseHttpsRedirection();

// Enable routing and map controllers (API endpoints)
app.UseRouting();
app.MapControllers();
// Bind to Render's dynamic port (falls back to 8080 for local Docker runs)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Option A: on the builder (works with minimal hosting)
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// If you already built 'app' earlier, you can alternatively do:
// app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
