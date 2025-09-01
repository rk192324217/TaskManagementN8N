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

app.Run();
