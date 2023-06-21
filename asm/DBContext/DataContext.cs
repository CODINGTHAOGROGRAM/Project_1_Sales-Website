using Microsoft.EntityFrameworkCore;

namespace asm.Models
{
    public class DataContext:DbContext
    {
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FoodItem>().ToTable(nameof(FoodItem));
            modelBuilder.Entity<Employee>().ToTable(nameof(Employee));
            modelBuilder.Entity<Order>().ToTable(nameof(Order));
            modelBuilder.Entity<OrderDetails>().ToTable(nameof(OrderDetails));
            modelBuilder.Entity<Customer>().ToTable(nameof(Customer));

        }

    }
}
