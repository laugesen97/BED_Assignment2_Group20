using Microsoft.EntityFrameworkCore;
using Assignment2_group20.Models;

namespace Assignment2_group20.Data
{
    public class DataDb : DbContext
    {
        public DataDb(DbContextOptions<DataDb> options) : base(options) { }
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<Job> Jobs => Set<Job>();
    }
}
