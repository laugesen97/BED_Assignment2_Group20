using Assignment2_group20.Models;
using Microsoft.AspNetCore.SignalR;

namespace Assignment2_group20.Hubs
{
    public class ExpenseLogHub : Hub<IExpenseLogHub>
    {
        public async Task LogExpense(string expense)
        {
            await Clients.All.LogExpense(expense);
        }

    }
}
