using Autofac.Extensions.DependencyInjection;
using Autofac;
using Carglass.TechnicalAssessment.Backend.DL.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAutoMapper(typeof(Carglass.TechnicalAssessment.Backend.BL.Module).Assembly);

builder.Host.ConfigureServices((hostContext, services) =>
{
    services.AddDbContext<ApplicationContext>();
});

// Use and configure Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder
        .RegisterModule<Carglass.TechnicalAssessment.Backend.BL.Module>()
        .RegisterModule<Carglass.TechnicalAssessment.Backend.DL.Module>()
        .RegisterModule<Carglass.TechnicalAssessment.Backend.Dtos.Module>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();
app.Run();