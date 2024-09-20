using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TrueRealityPizza;

public static class PizzaApiExtensions
{

    // 扩展方法，用于映射与披萨相关的API
    public static WebApplication MapPizzaApi(this WebApplication app)
    {

        // 订阅通知
        app.MapPut("/notifications/subscribe", [Authorize] async (
            HttpContext context,
            PizzaStoreContext db,
            NotificationSubscription subscription) => {

            // 我们最多为每个用户存储一个订阅，所以删除旧的订阅记录。
            // 你也可以让用户从不同的浏览器/设备注册多个订阅。
            var userId = GetUserId(context);
            if (userId is null)
            {
                return Results.Unauthorized();
            }

            // 删除旧的订阅
            var oldSubscriptions = db.NotificationSubscriptions.Where(e => e.UserId == userId);
            db.NotificationSubscriptions.RemoveRange(oldSubscriptions);

            // 存储新的订阅
            subscription.UserId = userId;
            db.NotificationSubscriptions.Attach(subscription);

            await db.SaveChangesAsync();
            return Results.Ok(subscription);

        });

        // 获取披萨特别要求
        app.MapGet("/specials", async (PizzaStoreContext db) => {

            var specials = await db.Specials.ToListAsync();
            return Results.Ok(specials);

        });

        // 获取配料
        app.MapGet("/toppings", async (PizzaStoreContext db) => {
            var toppings = await db.Toppings.OrderBy(t => t.Name).ToListAsync();
            return Results.Ok(toppings);
        });

        return app;

    }

    // 获取当前用户的ID
    public static string? GetUserId(HttpContext context) => context.User.FindFirstValue(ClaimTypes.NameIdentifier);

}