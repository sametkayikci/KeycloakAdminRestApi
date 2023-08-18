using KeycloakAdminRestApi.Configuration;
using KeycloakAdminRestApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerPreConfigured(options => { builder.Configuration.GetSection("Swagger").Bind(options); });
builder.Services.AddKeycloakPreConfigured(options =>
{
    builder.Configuration.GetSection("KeycloakOptions").Bind(options);
});

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>("KeycloakApiClient",
    client => { client.BaseAddress = new Uri(builder.Configuration.GetSection("KeycloakOptions:Url").Value!); });


builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerPreConfigured();
}

app.UseExceptionsHandler();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();

app.Run();