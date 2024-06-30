namespace Jobsity.Chat.Domain;

public static class Constants
{
    public const string StockApiClientName = "StockApi";
    public const string ReceiveMessage = "ReceiveMessage";

    public static class Bot
    {
        public const string Username = "Bot";
        public const string Message = "{0} quote is ${1} per share";
    }

    public static class ErrorMessages
    {
        public const string Default = "An error occurred.";
        public const string MissingApplicationConfigError = "Missing application config.";
        public const string StockCodeNotFound = "Stock code not found.";
    }
}