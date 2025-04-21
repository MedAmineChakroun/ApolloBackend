using ApolloBackend.Data;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class NotificationFunctions : INotification
{
    private readonly AppDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;
    public NotificationFunctions(AppDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;

    }

    public async Task AddNotificationAsync(string tiersCode, string title, string message, string type)
    {
        var notification = new Notification
        {
            TiersCode = tiersCode,
            Title = title,
            Message = message,
            type = type,

        };
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.Group(tiersCode).SendAsync("ReceiveNotification", notification);

    }

    public async Task<List<Notification>> GetUserNotifications(string tiersCode)
    {
        return await _context.Notifications
            .Where(n => n.TiersCode == tiersCode)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsRead(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<Notification>> getUserUnreadNotifications(string tiersCode)
    {
        return await _context.Notifications
            .Where(n => n.TiersCode == tiersCode && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
