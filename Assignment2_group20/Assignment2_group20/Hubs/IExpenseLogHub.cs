using Assignment2_group20.Models;

namespace Assignment2_group20.Hubs;

public interface IExpenseLogHub
{
    Task LogExpense(string expense);
}