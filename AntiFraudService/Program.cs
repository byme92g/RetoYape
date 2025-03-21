using AntiFraudService.AntiFraudService.Application.Handlers;
using Confluent.Kafka;
using SharedLib.DTOs;
using SharedLib.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kafka Config
var producerConfig = new ProducerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    EnableIdempotence = true,
    Acks = Acks.All
};

builder.Services.AddSingleton(producerConfig);
builder.Services.AddSingleton<IKafkaProducer<FraudCheckResponseDto>, KafkaProducer<FraudCheckResponseDto>>();

builder.Services.AddSingleton(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
        GroupId = builder.Configuration["Kafka:GroupId"],
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<Ignore, string>(config).Build();
});

builder.Services.AddScoped<IKafkaConsumerHandler<FraudCheckDto>, ValidateTransactionHandler>();
builder.Services.AddHostedService<KafkaConsumer<FraudCheckDto>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
