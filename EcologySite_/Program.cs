using Ecology.Data;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Repositories;
using EcologySite.CustomMiddlewares;
using EcologySite.Hubs;
using EcologySite.Services;
using EcologySite.Services.Apis;
using EcologySite.Services.BackgroundServices;
using Everything.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using EcologyRepository = Ecology.Data.Repositories.EcologyRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(AuthService.AUTH_TYPE_KEY)
    .AddCookie(AuthService.AUTH_TYPE_KEY, config =>
    {
        config.LoginPath = "/Auth/Login";
        config.AccessDeniedPath = "/Home/Forbidden";

    });


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<WebDbContext>(options => options.UseNpgsql(WebDbContext.CONNECTION_STRING));


builder.Services.AddSignalR();


// Register in DI container our services/repository
/*
builder.Services.AddScoped<ApiExplorerService>();
builder.Services.AddScoped<IEcologyRepositoryReal, EcologyRepository>();
builder.Services.AddScoped<ICommentRepositoryReal, CommentRepository>();
builder.Services.AddScoped<IUserRepositryReal, UserRepository>();
builder.Services.AddScoped<IChatMessageRepositryReal, ChatMessageRepositry>();*/

var registrationHelper = new RegistrationHelper();
registrationHelper.AutoRegisterRepositories(builder.Services);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EnumHelper>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddHttpClient<HttpNumberApi>(httpClient =>
    httpClient.BaseAddress = new Uri("http://numbersapi.com/")
);

builder.Services.AddHttpClient<HttpWoofApi>(httpClient =>
    httpClient.BaseAddress = new Uri("https://random.dog/")
);

builder.Services.AddHttpClient<WeatherApi>(httpClient =>
    httpClient.BaseAddress = new Uri("https://api.open-meteo.com/v1/")
);

builder.Services.AddHostedService<NotificationExparator>();

registrationHelper.AutoRegisterServiceByAttribute(builder.Services);
registrationHelper.AutoRegisterServiceByAttributeOnConstructor(builder.Services);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(); // Регистрация HttpClientFactory

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.SetIsOriginAllowed(origin => true);
        policy.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

var seed = new Seed();
seed.Fill(app.Services);

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

app.UseAuthentication(); // Who Am I?
app.UseAuthorization(); // May I?


app.UseMiddleware<CustomLocalizationMiddleware>();
app.UseMiddleware<CustomThemeMiddleware>();

app.MapHub<ChatHub>("/hub/chatMainPage");
app.MapHub<NotificationHub>("/hub/notification");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ecology}/{action=Index}/{id?}");

app.MapControllers();

app.Run();