using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface INotification

    {
        Task AddNotificationAsync(string tiersCode, string title, string message ,string type);
        Task<List<Notification>> GetUserNotifications(string tiersCode);
        Task MarkAsRead(int notificationId);

        Task<List<Notification>> getUserUnreadNotifications(string tiersCode);
    }

}
