using CustomerRecognitionService;
using CustomerRecognitionService.Repository;
using CustomerRecognitionService.Repository.Interfaces;
using CustomerRecognitionService.Services;
using CustomerRecognitionService.Services.Background;
using CustomerRecognitionService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
// repos
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerMergeRepository, CustomerMergeRepository>();
builder.Services.AddScoped<IPendingMergeRepository, PendingMergeRepository>();
builder.Services.AddScoped<IMergedCustomerHistoryRepository, MergedCustomerHistoryRepository>();
// services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerValidationService, CustomerValidationService>();
builder.Services.AddScoped<ICustomerMergeService, CustomerMergeService>();
builder.Services.AddScoped<IMergedCustomerHistoryService, MergedCustomerHistoryService>();
builder.Services.AddScoped<IPendingMergeService, PendingMergeService>();
// background
builder.Services.AddHostedService<MergeDuplicateCustomersBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
