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


#region DataBase Context

builder.Services.AddDbContext<PollingContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("PollingConnection"));
});

#endregion

#region IoC

builder.Services.AddTransient<IUserServices, UserService>();

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

app.UseMvc(endpoints =>
{
    endpoints.MapRoute(
      name: "areas",
      template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
