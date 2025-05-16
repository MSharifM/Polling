using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Polling.Core.Services;
using Polling.Core.Services.Interfaces;
using Polling.Datalayer.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

#region Authentication

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "/Login";
    option.LogoutPath = "/Logout";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

#endregion

#region DataBase Context

builder.Services.AddDbContext<PollingContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("PollingConnection"));
});

#endregion

#region IoC

builder.Services.AddTransient<IUserServices, UserService>();
builder.Services.AddTransient<IVoteService, VoteService>();

#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
