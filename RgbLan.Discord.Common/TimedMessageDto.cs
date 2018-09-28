namespace RgbLan.Discord.Common
{
    public class TimedMessageDto
    {
        public TimedMessageDto()
        {
            TimedMessageId = 0;
            Channel = 0;
            Message = string.Empty;
            MessageRepeatMilliseconds = 0;
        }

        public TimedMessageDto(ulong channel, string message, int messageRepeatMilliseconds)
        {
            Channel = channel;
            Message = message;
            MessageRepeatMilliseconds = messageRepeatMilliseconds;
        }

        public int TimedMessageId { get; set; }
        public ulong Channel { get; set; }
        public string Message { get; set; }
        public int MessageRepeatMilliseconds { get; set; }
    }
}
