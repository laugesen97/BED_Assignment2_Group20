using Assignment2_group20.Data;
using Assignment2_group20.Hubs;
using Assignment2_group20.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add signalR
builder.Services.AddSignalR();
builder.Services.AddDbContext<DataDb>(opt => opt.UseInMemoryDatabase("TempDb"));

var app = builder.Build();


//Seeding db
using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<DataDb>();

    // Seed the database.

    AddTestData(context);
    static void AddTestData(DataDb context)
    {

        var testExpense1 = new Expense()
        {
            amount = 123,
            Date = DateTime.Now,
            ExpenseId = 1,
            JobId = 1,
            ModelId = 1,
            Text = "Mad"
        };

        context.Expenses.Add(testExpense1);


        var testModel1 = new Model()
        {
            ModelId = 1,
            AddresLine1 = "Abevej",
            AddresLine2 = "2q",
            BirthDay = DateTime.Now,
            City = "Aarhus",
            Comments = "None",
            Email = "email@as.dk",
            PhoneNo = "123234",
            Expenses = new List<Expense>()
            {testExpense1
            },
            Jobs = new List<Job>(){},
            FirstName = "Anne",
            LastName = "Doe",
            HairColor = "blond",
            Height = 187
        };
        context.Models.Add(testModel1);

        var testJob1 = new Job()
        {
            Comments = "none",
            Customer = "Netto",
            Days = 4,
            Expenses = new List<Expense>()
            {
testExpense1
            },
            JobId = 1,
            Location = "Oslo",
            Models = new List<Model>() { testModel1 },
            StartDate = DateTimeOffset.Now
        };

        context.Jobs.Add(testJob1);



        context.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<ExpenseLogHub>("/logExpenseHub");
app.MapControllers();

app.Run();
