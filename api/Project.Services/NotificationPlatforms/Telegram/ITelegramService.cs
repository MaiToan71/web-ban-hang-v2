namespace Project.Services.NotificationPlatforms.Telegram
{
    public interface ITelegramService
    {
        public Task<bool> SendToGroup();
    }
}
