﻿using Jobsity.Chat.Domain.Dto;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Repositories.Messages;
using Jobsity.Chat.Services.Hubs;
using Jobsity.Chat.Services.RabbitMQ;
using Jobsity.Chat.Tests.Builders;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Jobsity.Chat.Tests.Hub;

public class Tests
{
    private readonly Mock<IProducer> _producer = new();
    private readonly Mock<IMessageRepository> _messageRepository = new();
    private readonly Mock<IHubCallerClients> _mockClients = new();
    private readonly Mock<IClientProxy> _mockClientProxy = new();
    private readonly Mock<HubCallerContext> _mockContext = new();
    private readonly ChatHub _chatHub;

    private readonly List<string> _clientIds = ["0", "1", "2"];

    public Tests()
    {
        _chatHub = new ChatHub(_producer.Object, _messageRepository.Object);

        _mockClients.Setup(client => client.All).Returns(_mockClientProxy.Object);
        _mockContext.Setup(context => context.ConnectionId).Returns(It.IsIn<string>(_clientIds));

        _chatHub = new ChatHub(_producer.Object, _messageRepository.Object)
        {
            Clients = _mockClients.Object,
            Context = _mockContext.Object
        };
    }

    [Fact]
    public void AllClientsNotified()
    {
        _chatHub.SendMessage("TEST", "Luffy willbe Pirate king");
        _producer.Verify(clients => clients.Send(It.IsAny<MessageDto>()), Times.Once);
    }

    [Fact]
    public async Task ShouldNotifyAllClientsWhenNotifyAsync()
    {
        var message = new MessageBuilder().Build();

        _messageRepository.Setup(x => x.GetHistoryMessagesAsync())
            .ReturnsAsync(new List<Message> { message });

        await _chatHub.OnConnectedAsync();
        _mockClients.Verify(clients => clients.All, Times.Once);
    }

    [Fact]
    public async Task ShouldNotNotifyWhenMessageNotAvailavle()
    {
        _messageRepository.Setup(x => x.GetHistoryMessagesAsync())
            .ReturnsAsync(new List<Message> { });

        await _chatHub.OnConnectedAsync();
        _mockClients.Verify(clients => clients.All, Times.Never);
    }
}