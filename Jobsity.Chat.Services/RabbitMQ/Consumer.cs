﻿using AutoMapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Jobsity.Chat.Domain.Configuration;
using Jobsity.Chat.Domain.Dto;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Extensions;
using Jobsity.Chat.Repositories.Messages;
using Jobsity.Chat.Services.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using Jobsity.Chat.Bot;

namespace Jobsity.Chat.Services.RabbitMQ;

public sealed class Consumer : IConsumer
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;
    private readonly IModel _channel;
    private readonly IBotProcessor _botProcessor;
    private readonly RabbitMq _rabbitMq;
    private readonly IHubContext<ChatHub> _chatHub;

    public Consumer(IMessageRepository messageRepository,
        IMapper mapper,
        IServiceProvider serviceProvider,
        IBotProcessor botProcessor,
        ApplicationConfig applicationConfig)
    {
        _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _botProcessor = botProcessor ?? throw new ArgumentNullException(nameof(botProcessor));
        _rabbitMq = applicationConfig.RabbitMq!;

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMq.Hostname,
            RequestedHeartbeat = TimeSpan.FromSeconds(10),
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _chatHub = (IHubContext<ChatHub>)serviceProvider.GetService(typeof(IHubContext<ChatHub>))!;
    }

    public void Connect()
    {
        _channel.QueueDeclare(queue: _rabbitMq.QueueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += ConsumerReceived;
        _channel.BasicConsume(queue: _rabbitMq.QueueName, autoAck: true, consumer: consumer);
    }

    private void ConsumerReceived(object? model, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            var messageJson = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var message = JsonConvert.DeserializeObject<MessageDto>(messageJson);

            InvokeHandlers(message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred in processing message or invoking handlers");
        }
    }

    private async Task InvokeHandlers(MessageDto messageDto)
    {
        if (string.IsNullOrEmpty(messageDto.Message)) return;

        if (messageDto.Message.IsBotCommand())
        {
            var processedMessage = await _botProcessor.ProcessCommand(messageDto.Message);
            messageDto.BotMessage(processedMessage);
        }
        else
        {
            await _messageRepository.AddAsync(_mapper.Map<Message>(messageDto));
        }

        Send(messageDto.User, messageDto.Message);
    }

    private void Send(string? user, string message)
    {
        _chatHub.Clients.All.SendAsync(Domain.Constants.ReceiveMessage, user, message,
            DateTime.UtcNow.ToFriendlyDateString());
    }
}