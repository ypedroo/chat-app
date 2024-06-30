using Jobsity.Chat.Bot;
using Jobsity.Chat.Domain;
using Jobsity.Chat.Domain.Configuration;
using Jobsity.Chat.Repositories;
using Jobsity.Chat.Services;
using Jobsity.Chat.Services.Hubs;
using Jobsity.Chat.Services.RabbitMQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var loggerConfig = new LoggerConfiguration().WriteTo.Console();
Log.Logger = loggerConfig.CreateLogger();

var applicationConfig = builder.Configuration.GetSection(nameof(ApplicationConfig)).Get<ApplicationConfig>();
applicationConfig!.Validate();
builder.Services.AddSingleton(applicationConfig);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(applicationConfig.ConnectionString!, b => b.MigrationsAssembly("Jobsity.Chat")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.Password.RequiredLength = 3;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddValidators();
builder.Services.AddBotProcessor(applicationConfig);
builder.Services.AddMapperProfile();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddHttpClients(applicationConfig);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(configure => { configure.MapHub<ChatHub>("/chatHub"); });

app.Lifetime.ApplicationStarted.Register(() =>
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var consumer = serviceScope.ServiceProvider.GetService<IConsumer>();
    consumer?.Connect();
});

app.MapRazorPages();

await InitializeDatabase();

app.Run();

async Task InitializeDatabase()
{
    try
    {
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        if (context is not null)
            await context.Database.EnsureCreatedAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while seeding the database");
    }
}