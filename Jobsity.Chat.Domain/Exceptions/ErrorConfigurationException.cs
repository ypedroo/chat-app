using System.Runtime.Serialization;

namespace Jobsity.Chat.Domain.Exceptions;

public class ErrorConfigurationException : Exception
{
    protected ErrorConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ErrorConfigurationException(string errorMessage) : base(errorMessage)
    {
    }
}