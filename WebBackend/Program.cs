using WebBackend.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => 
{
    options.Filters.Add<CheckSessionAttribute>();
});

builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);  // Session 过期时间
    options.Cookie.HttpOnly = true;                 // 安全设置
    options.Cookie.IsEssential = true;              // GDPR 合规性
    options.Cookie.Name = ".GOV.Session";       // 自定义 Cookie 名称
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
