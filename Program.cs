using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookWorld.Data;
using BookWorld.Interfaces;
using BookWorld.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<BookWorldContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BookWorldContext"))
                   .LogTo(Console.WriteLine, LogLevel.Information));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BookWorldContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("User", policy => policy.RequireRole("User"));
        });

        builder.Services.AddScoped<IBookWorldService, BookWorldService>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Adjust routing to ensure Home is the default
        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllerRoute(name: "booksMvc", pattern: "{controller=BooksMvc}/{action=Index}/{id?}");
        app.MapRazorPages();

        // Seed data during development
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BookWorldContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                try
                {
                    // Seed roles
                    string[] roles = { "Admin", "User" };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            var result = await roleManager.CreateAsync(new IdentityRole(role));
                            if (!result.Succeeded)
                            {
                                Console.WriteLine($"Failed to create role {role}: {string.Join(", ", result.Errors)}");
                            }
                        }
                    }

                    // Seed users
                    var admin = await userManager.FindByEmailAsync("admin@bookworld.com");
                    if (admin == null)
                    {
                        var result = await userManager.CreateAsync(new IdentityUser { UserName = "admin@bookworld.com", Email = "admin@bookworld.com" }, "Admin123!");
                        if (result.Succeeded)
                        {
                            admin = await userManager.FindByEmailAsync("admin@bookworld.com");
                            await userManager.AddToRoleAsync(admin!, "Admin");
                            Console.WriteLine("Admin user created successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to create admin: {string.Join(", ", result.Errors)}");
                        }
                    }
                    else if (!await userManager.IsInRoleAsync(admin, "Admin"))
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        Console.WriteLine("Admin role assigned to existing user.");
                    }

                    var user = await userManager.FindByEmailAsync("user@bookworld.com");
                    if (user == null)
                    {
                        var result = await userManager.CreateAsync(new IdentityUser { UserName = "user@bookworld.com", Email = "user@bookworld.com" }, "User123!");
                        if (result.Succeeded)
                        {
                            user = await userManager.FindByEmailAsync("user@bookworld.com");
                            await userManager.AddToRoleAsync(user!, "User");
                            Console.WriteLine("User created successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to create user: {string.Join(", ", result.Errors)}");
                        }
                    }
                    else if (!await userManager.IsInRoleAsync(user, "User"))
                    {
                        await userManager.AddToRoleAsync(user, "User");
                        Console.WriteLine("User role assigned to existing user.");
                    }

                    // Check and log book data
                    var bookCount = await context.Books.CountAsync();
                    Console.WriteLine($"Number of books in database: {bookCount}");
                    if (bookCount > 0)
                    {
                        var firstBook = await context.Books.FirstOrDefaultAsync();
                        Console.WriteLine($"First book title: {firstBook?.Title ?? "No title available"}");
                    }
                    else
                    {
                        Console.WriteLine("No books found in the database. Consider seeding data.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during initialization: {ex.Message}\nStack: {ex.StackTrace}");
                }
            }
        }

        await app.RunAsync();
    }
}