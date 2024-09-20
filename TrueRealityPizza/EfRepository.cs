using Microsoft.EntityFrameworkCore;

namespace TrueRealityPizza;

public class EfRepository : IRepository
{
    private readonly PizzaStoreContext _Context;

    // 构造函数，接收数据库上下文
    public EfRepository(PizzaStoreContext context)
    {
        _Context = context;
    }

    // 获取所有订单，并包括相关联的数据，如送货地址、披萨、特别要求和配料
    public async Task<List<OrderWithStatus>> GetOrdersAsync()
    {
        var orders = await _Context.Orders
                        .Include(o => o.DeliveryLocation)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                        .OrderByDescending(o => o.CreatedTime)
                        .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    // 根据用户ID获取该用户的所有订单，并包括相关数据
    public async Task<List<OrderWithStatus>> GetOrdersAsync(string? userId)
    {
        var orders = await _Context.Orders
                        .Where(o => o.UserId == userId)
                        .Include(o => o.DeliveryLocation)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                        .OrderByDescending(o => o.CreatedTime)
                        .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    // 根据订单ID获取订单及状态，并包括相关数据
    public async Task<OrderWithStatus> GetOrderWithStatus(int orderId)
    {
        var order = await _Context.Orders
                        .Where(o => o.OrderId == orderId)
                        .Include(o => o.DeliveryLocation)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                        .SingleOrDefaultAsync();

        // 如果订单为空，抛出异常
        if (order is null) throw new ArgumentNullException(nameof(order));

        return OrderWithStatus.FromOrder(order);
    }

    // 根据订单ID和用户ID获取订单及状态，并包括相关数据
    public async Task<OrderWithStatus> GetOrderWithStatus(int orderId, string userId)
    {
        var order = await _Context.Orders
                        .Where(o => o.OrderId == orderId && o.UserId == userId)
                        .Include(o => o.DeliveryLocation)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                        .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                        .SingleOrDefaultAsync();

        // 如果订单为空，抛出异常
        if (order is null) throw new ArgumentNullException(nameof(order));

        return OrderWithStatus.FromOrder(order);
    }

    // 获取所有披萨的特别要求
    public async Task<List<PizzaSpecial?>> GetSpecials()
    {
        return await _Context.Specials.ToListAsync();
    }
    
    // 获取所有配料，并按名称排序
    public async Task<List<Topping>> GetToppings()
    {
        return await _Context.Toppings.OrderBy(t => t.Name).ToListAsync();
    }

    // 提交订单，尚未实现
    public Task<int> PlaceOrder(Order order)
    {
        throw new NotImplementedException();
    }

    // 订阅通知，尚未实现
    public Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        throw new NotImplementedException();
    }
}
