using Library.Data.Data;
using Library.Server.Interfaces;
using Library.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Обязательно регистрируем контроллеры!
builder.Services.AddControllers();

// 2. Регистрируем контекст базы данных (укажите ваш провайдер, например UseNpgsql / UseSqlite / UseSqlServer)
builder.Services.AddDbContext<DataBase>();

// 3. Настраиваем Cookie-аутентификацию
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Library.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Разрешаем работу по http://localhost:5063
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// 4. Регистрируем ваши сервисы в DI!
builder.Services.AddSingleton<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMainService, MainService>(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Закомментировано, чтобы при запросах на http://localhost:5063 не было редиректа на https://localhost:7112
// app.UseHttpsRedirection();

// 5. Подключаем middleware аутентификации и авторизации (строго перед MapControllers!)
app.UseAuthentication();
app.UseAuthorization();

// 6. Подключаем маршруты ваших контроллеров!
app.MapControllers();

app.Run();
