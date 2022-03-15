using Assignment2_group20.Models;
using Microsoft.AspNetCore.SignalR;

namespace Assignment2_group20.Hubs
{
    public class ExpenseLogHub : Hub
    {
        public async Task LogExpense(Expense expense)
        {
            await Clients.All.SendAsync("ReceiveMessage", expense);
        }
    }
}
