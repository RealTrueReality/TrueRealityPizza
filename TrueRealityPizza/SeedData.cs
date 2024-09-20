using Microsoft.AspNetCore.Identity;

namespace TrueRealityPizza;

public static class SeedData {
    public static void Initialize(PizzaStoreContext db) {
        var toppings = new Topping[] {
            new Topping() {
                Id = 1,
                Name = "双倍芝士",
                Price = 2.50m,
            },
            new Topping() {
                Id = 2,
                Name = "美式培根",
                Price = 2.99m,
            },
            new Topping() {
                Id = 3,
                Name = "英式培根",
                Price = 2.99m,
            },
            new Topping() {
                Id = 4,
                Name = "加拿大培根",
                Price = 2.99m,
            },
            new Topping() {
                Id = 5,
                Name = "英式下午茶",
                Price = 5.00m
            },
            new Topping() {
                Id = 6,
                Name = "新鲜烤制的司康",
                Price = 4.50m,
            },
            new Topping() {
                Id = 7,
                Name = "彩椒",
                Price = 1.00m,
            },
            new Topping() {
                Id = 8,
                Name = "洋葱",
                Price = 1.00m,
            },
            new Topping() {
                Id = 9,
                Name = "蘑菇",
                Price = 1.00m,
            },
            new Topping() {
                Id = 10,
                Name = "意大利辣香肠",
                Price = 1.00m,
            },
            new Topping() {
                Id = 11,
                Name = "鸭肉香肠",
                Price = 3.20m,
            },
            new Topping() {
                Id = 12,
                Name = "鹿肉丸",
                Price = 2.50m,
            },
            new Topping() {
                Id = 13,
                Name = "银盘精致盛宴",
                Price = 250.99m,
            },
            new Topping() {
                Id = 14,
                Name = "顶级龙虾",
                Price = 64.50m,
            },
            new Topping() {
                Id = 15,
                Name = "鲟鱼鱼子酱",
                Price = 101.75m,
            },
            new Topping() {
                Id = 16,
                Name = "洋蓟心",
                Price = 3.40m,
            },
            new Topping() {
                Id = 17,
                Name = "新鲜番茄",
                Price = 1.50m,
            },
            new Topping() {
                Id = 18,
                Name = "罗勒",
                Price = 1.50m,
            },
            new Topping() {
                Id = 19,
                Name = "五分熟牛排",
                Price = 8.50m,
            },
            new Topping() {
                Id = 20,
                Name = "炽热辣椒",
                Price = 4.20m,
            },
            new Topping() {
                Id = 21,
                Name = "水牛鸡肉",
                Price = 5.00m,
            },
            new Topping() {
                Id = 22,
                Name = "蓝纹奶酪",
                Price = 2.50m,
            },
        };

        PizzaSpecial?[] specials = new PizzaSpecial[] {
            new PizzaSpecial() {
                Name = "经典芝士披萨",
                Description = "芝士满满，奶香四溢，无法抗拒的美味选择！",
                BasePrice = 9.99m,
                ImageUrl = "img/pizzas/cheese.jpg",
            },
            new PizzaSpecial() {
                Id = 2,
                Name = "培根狂热",
                Description = "汇聚每一种培根的美味盛宴",
                BasePrice = 11.99m,
                ImageUrl = "img/pizzas/bacon.jpg",
            },
            new PizzaSpecial() {
                Id = 3,
                Name = "经典辣香肠披萨",
                Description = "与童年的味道相同，但火辣升级，挑战你的味蕾！",
                BasePrice = 10.50m,
                ImageUrl = "img/pizzas/pepperoni.jpg",
            },

            new PizzaSpecial() {
                Id = 4,
                Name = "水牛鸡肉披萨",
                Description = "辛辣鸡肉配上蓝纹奶酪，再加上一抹热辣的酱汁，温暖你整个冬天！",
                BasePrice = 12.75m,
                ImageUrl = "img/pizzas/meaty.jpg",
            },
            new PizzaSpecial() {
                Id = 5,
                Name = "蘑菇爱好者",
                Description = "各类精选蘑菇，鲜香四溢，蘑菇迷们的终极选择！",
                BasePrice = 11.00m,
                ImageUrl = "img/pizzas/mushroom.jpg",
            },
            new PizzaSpecial() {
                Id = 6,
                Name = "英伦风情披萨",
                Description = "伦敦风味与披萨的完美结合，带你品味不列颠的独特魅力！",
                BasePrice = 10.25m,
                ImageUrl = "img/pizzas/brit.jpg",
            },
            new PizzaSpecial() {
                Id = 7,
                Name = "素食之恋",
                Description = "每一片蔬菜都新鲜采摘，健康与美味在这里碰撞，素食者的天堂！",
                BasePrice = 11.50m,
                ImageUrl = "img/pizzas/salad.jpg",
            },
            new PizzaSpecial() {
                Id = 8,
                Name = "玛格丽塔披萨",
                Description = "意大利经典，简约而不简单，番茄与罗勒的绝佳搭配，让味道回归纯粹！",
                BasePrice = 9.99m,
                ImageUrl = "img/pizzas/margherita.jpg",
            },
        };

        db.Toppings.AddRange(toppings);
        db.Specials.AddRange(specials);
        db.SaveChanges();
    }

    public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider) {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<PizzaStoreUser>>();

        string[] roleNames = { "Admin", "Customer" };
        IdentityResult roleResult;

        // 创建角色
        foreach (var roleName in roleNames) {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) {
                // 创建角色
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 创建管理员用户
        var adminUser = new PizzaStoreUser {
            UserName = "admin@pizzastore.com",
            Email = "admin@pizzastore.com",
            EmailConfirmed = true,
        };

        string adminPassword = "Admin@123";
        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null) {
            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdmin.Succeeded) {
                // 添加管理员角色
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // 创建顾客用户
        var customerUser = new PizzaStoreUser {
            UserName = "customer@pizzastore.com",
            Email = "customer@pizzastore.com",
            EmailConfirmed = true,
        };

        string customerPassword = "Customer@123";
        user = await userManager.FindByEmailAsync(customerUser.Email);

        if (user == null) {
            var createCustomer = await userManager.CreateAsync(customerUser, customerPassword);
            if (createCustomer.Succeeded) {
                // 添加顾客角色
                await userManager.AddToRoleAsync(customerUser, "Customer");
            }
        }
    }
}