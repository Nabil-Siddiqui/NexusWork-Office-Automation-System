using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusWork.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// IMPORTANT: Added .AddRoles<IdentityRole>() to enable role system
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// ========================================
// SEED ROLES AND ADMIN USER ON STARTUP
// ========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Step 1: Create roles if they don't exist
        string[] roles = { "Admin", "Employee" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"Role '{role}' created successfully.");
            }
        }

        // Step 2: Assign Admin role to specific user
        // CHANGE THIS EMAIL to your admin email
        var adminEmail = "admin@nexuswork.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser != null)
        {
            // Check if user already has Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Admin role assigned to {adminEmail}");
            }
        }
        else
        {
            Console.WriteLine($"Warning: User {adminEmail} not found. Register this user first.");
        }

        // Step 3: Assign Employee role to all other users who have no role
        var allUsers = userManager.Users.ToList();
        foreach (var user in allUsers)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles.Count == 0)
            {
                await userManager.AddToRoleAsync(user, "Employee");
                Console.WriteLine($"Employee role assigned to {user.Email}");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles.");
    }
}

app.Run();