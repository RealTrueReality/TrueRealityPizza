using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TrueRealityPizza;

public class PizzaStoreContext : IdentityDbContext<PizzaStoreUser>
{
    // 构造函数，接收数据库上下文配置选项
    public PizzaStoreContext(DbContextOptions options) : base(options)
    {
    }

    // 数据库中的订单表
    public DbSet<Order> Orders { get; set; }

    // 数据库中的披萨表
    public DbSet<Pizza> Pizzas { get; set; }

    // 数据库中的披萨特别要求表
    public DbSet<PizzaSpecial?> Specials { get; set; }

    // 数据库中的配料表
    public DbSet<Topping> Toppings { get; set; }

    // 数据库中的通知订阅表
    public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

    // 配置数据库模型
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置披萨与配料的多对多关系，便于序列化
        modelBuilder.Entity<PizzaTopping>().HasKey(pst => new { pst.PizzaId, pst.ToppingId });
        modelBuilder.Entity<PizzaTopping>().HasOne<Pizza>().WithMany(ps => ps.Toppings);
        modelBuilder.Entity<PizzaTopping>().HasOne(pst => pst.Topping).WithMany();

        // 将经纬度嵌入到订单中，而不是外键关联到另一个表
        modelBuilder.Entity<Order>().OwnsOne(o => o.DeliveryLocation);
    }
}