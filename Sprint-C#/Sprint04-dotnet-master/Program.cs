using Microsoft.EntityFrameworkCore;
using Sessions_app.Data;
using Sessions_app.Service;
using Sessions_app.Services;
using Sessions_app.Models;
using Sessions_app.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gestão de Pacientes e Médicos Odontologica",
        Version = "v1",
        Description = "API para gerenciamento de pacientes e médicos Odontologica"
    });

    // Habilitar anotações Swagger
    c.EnableAnnotations();

    // Inclui os controllers MVC no Swagger
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });

    // Filtra quais controllers e actions devem aparecer no Swagger
    c.DocInclusionPredicate((docName, api) => true);
});

// Add session configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.Name = "_ApplicationSession";
    options.Cookie.IsEssential = true;
});

// Configure database context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"));
});

// Add repositories and services
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<PacienteService>();

// Add Medico services
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<MedicoService>();

// Add Agendamento
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<AgendamentoService>();

// Add RabbitMQ service
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();
builder.Services.AddTransient<EmailService>();

//Add ML
builder.Services.AddSingleton<PacienteMLService>();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Configure Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Gestão v1");
        c.RoutePrefix = "swagger";
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayRequestDuration();
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "customRoute",
    pattern: "{controller=Paciente}/{action=Index}/{id?}");

app.Run();