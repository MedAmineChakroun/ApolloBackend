using Microsoft.AspNetCore.SignalR;

namespace ApolloBackend.Models
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var tiersCode = httpContext?.Request.Query["tiersCode"].ToString();

            if (!string.IsNullOrEmpty(tiersCode))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, tiersCode);
            }

            await base.OnConnectedAsync();
        }
    }

}