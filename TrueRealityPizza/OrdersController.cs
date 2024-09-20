using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPush;

namespace TrueRealityPizza;

[Route("orders")]
[ApiController]
[Authorize]
public class OrdersController : Controller {
    private readonly PizzaStoreContext _db;

    // 构造函数，注入数据库上下文
    public OrdersController(PizzaStoreContext db) {
        _db = db;
    }

    // 获取当前用户的所有订单
    [HttpGet]
    public async Task<ActionResult<List<OrderWithStatus>>> GetOrders() {
        var orders = await _db.Orders
            .Where(o => o.UserId == PizzaApiExtensions.GetUserId(HttpContext))
            .Include(o => o.DeliveryLocation)
            .Include(o => o.Pizzas).ThenInclude(p => p.Special)
            .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
            .OrderByDescending(o => o.CreatedTime)
            .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    // 根据订单ID获取单个订单的详细信息
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderWithStatus>> GetOrderWithStatus(int orderId) {
        var order = await _db.Orders
            .Where(o => o.OrderId == orderId)
            .Where(o => o.UserId == PizzaApiExtensions.GetUserId(HttpContext))
            .Include(o => o.DeliveryLocation)
            .Include(o => o.Pizzas).ThenInclude(p => p.Special)
            .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
            .SingleOrDefaultAsync();

        // 如果订单为空，返回404错误
        if (order == null) {
            return NotFound();
        }

        return OrderWithStatus.FromOrder(order);
    }

    // 下订单，存入数据库
    [HttpPost]
    public async Task<ActionResult<int>> PlaceOrder(Order order) {
        // 设置订单的创建时间和配送地址
        order.CreatedTime = DateTime.Now;
        order.DeliveryLocation = new LatLong(32.55219, 117.02149);
        order.UserId = PizzaApiExtensions.GetUserId(HttpContext);

        // 强制验证Pizza.SpecialId和Topping.ToppingId在数据库中存在，防止提交者虚构新的特别要求和配料
        foreach (var pizza in order.Pizzas) {
            pizza.SpecialId = pizza.Special?.Id ?? 0;
            pizza.Special = null;

            foreach (var topping in pizza.Toppings) {
                topping.ToppingId = topping.Topping?.Id ?? 0;
                topping.Topping = null;
            }
        }

        // 将订单附加到数据库上下文并保存更改
        _db.Orders.Attach(order);
        await _db.SaveChangesAsync();

        // 在后台发送推送通知（如果可能）
        var subscription = await _db.NotificationSubscriptions
            .Where(e => e.UserId == PizzaApiExtensions.GetUserId(HttpContext)).SingleOrDefaultAsync();
        if (subscription != null) {
            _ = TrackAndSendNotificationsAsync(order, subscription);
        }

        return order.OrderId;
    }

    // 模拟跟踪订单的状态，并发送推送通知
    private static async Task TrackAndSendNotificationsAsync(Order order, NotificationSubscription subscription) {
        // 在真实情况下，后台进程会跟踪订单的配送进度，并在状态更改时发送通知。此处为模拟。
        await Task.Delay(OrderWithStatus.PreparationDuration);
        await SendNotificationAsync(order, subscription, "您的订单已送出！");

        await Task.Delay(OrderWithStatus.DeliveryDuration);
        await SendNotificationAsync(order, subscription, "您的订单已送达，请享用！");
    }

    // 发送推送通知
    private static async Task
        SendNotificationAsync(Order order, NotificationSubscription subscription, string message) {
        // 使用生成的 VAPID 密钥
        var publicKey = "BERcbPtaAS6dn9FC3kTShLwOdY6n7WUgTKIP9ZqBtZvjW0CWlmF8anzxEw_7lQ7evEmzaQPwTJWkkdr7qPWf6Ks";
        var privateKey = "HuF8IhVS4y10xc6kQ2NfDabYVZhdipo14Ery5758y_A";

        var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
        var vapidDetails = new VapidDetails("mailto:truereality@foxmail.com", publicKey, privateKey);
        var webPushClient = new WebPushClient();
        try {
            var payload = JsonSerializer.Serialize(new {
                message,
                url = $"myorders/{order.OrderId}",
            });
            await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
        }
        catch (Exception ex) {
            Console.Error.WriteLine("发送推送通知时出错: " + ex.Message);
        }
    }
}