using FluentAssertions;
using Jobsity.Chat.Domain.Extensions;

namespace Jobsity.Chat.Tests.Extensions;

public class StringExtensionsTest
{
    [Theory]
    [InlineData(true, "/stock=wig")]
    [InlineData(false, "/stock=")]
    [InlineData(false, " /stock=wig ")]
    [InlineData(false, "trying /stock=wig")]
    [InlineData(false, "-/stock=wig")]
    public void ShouldReturnValidationForCommand(bool expected, string input)
    {
        var isValid = input.IsBotCommand();
        isValid.Should().Be(expected);
    }
}