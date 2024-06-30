using FluentAssertions;
using Jobsity.Chat.Domain.Extensions;

namespace Jobsity.Chat.Tests.Extensions;

public class DateTimeExtensionsTest
{
    [Fact]
    public void ShouldReturnTodayDate()
    {
        var date = DateTime.Today.AddHours(10);

        var dateString = date.ToFriendlyDateString();
        dateString.Should().Be("Today, at 10:00");
    }
}