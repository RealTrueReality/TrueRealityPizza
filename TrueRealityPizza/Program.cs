global using TrueRealityPizza.Shared;
global using TrueRealityPizza;
using System.Diagnostics;
using TrueRealityPizza.Client;
using TrueRealityPizza.Components;
using TrueRealityPizza.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// Add Security
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options => {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();


builder.Services.AddDbContext<PizzaStoreContext>(options =>
    options.UseSqlite("Data Source=pizza.db"));

// Add Identity
builder.Services.AddIdentityCore<PizzaStoreUser>(options => 
    {
        // 修改密码规则配置
        options.Password.RequiredLength = 6; // 密码至少 6 个字符
        options.Password.RequireDigit = true; // 要求包含数字
        options.Password.RequireLowercase = true; // 要求包含小写字母
        options.Password.RequireUppercase = false; // 要求包含大写字母
        options.Password.RequireNonAlphanumeric = false; // 要求不包含非字母数字字符（符号）
        options.Password.RequiredUniqueChars = 1; // 至少要求 1 个唯一字符
    
        options.SignIn.RequireConfirmedAccount = true; // 保留账户确认要求
    })
    .AddRoles<IdentityRole>() // 添加角色支持
    .AddEntityFrameworkStores<PizzaStoreContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<CustomIdentityErrorDescriber>(); // 使用自定义的错误描述器

builder.Services.AddSingleton<IEmailSender<PizzaStoreUser>, IdentityNoOpEmailSender>();


builder.Services.AddScoped<IRepository, EfRepository>();
builder.Services.AddScoped<IManagePizzaRepository, ManagePizzaRepository>();
builder.Services.AddScoped<OrderState>();

builder.Services.AddControllers();

var app = builder.Build();

// 自动打开浏览器
OpenBrowser("https://localhost:5001"); // 修改URL为你实际的启动地址

// 初始化数据库和其他配置
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated()) {
        SeedData.Initialize(db);
        await SeedData.SeedRolesAndUsers(scope.ServiceProvider);
    }
}

// 配置中间件和请求管道...
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapPizzaApi();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(TrueRealityPizza.Client._Imports).Assembly,
        typeof(TrueRealityPizza.ComponentsLibrary._Imports).Assembly
    );

app.MapAdditionalIdentityEndpoints();

app.Run();

static void OpenBrowser(string url)
{
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"无法打开浏览器: {ex.Message}");
    }
}