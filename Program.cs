using ManchesterUnitedApp.Data;
using ManchesterUnitedApp.Data.DataSeeders;
using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Models.Account;
using ManchesterUnitedApp.Repositories;
using ManchesterUnitedApp.Repositories.IMatchRepository;
using ManchesterUnitedApp.Repositories.PlayerRepository;
using ManchesterUnitedApp.Repositories.PostRepositories;
using ManchesterUnitedApp.Services;
using ManchesterUnitedApp.Services.IMatchService;
using ManchesterUnitedApp.Services.PlayerServices;
using ManchesterUnitedApp.Services.PostServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ‑‑ Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();  // If you are using Razor Pages (e.g., Identity UI)  

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity to use your custom User class
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// Register any application‑specific services
builder.Services.AddScoped<MasterDataSeeder>();

// Configure authentication & authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ItemRepository>();
builder.Services.AddScoped<ItemTypeRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PurchaseRepository>();
builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<GenderTypeRepository>();
builder.Services.AddScoped<PlayerPositionRepository>();
builder.Services.AddScoped<PlayerStatusRepository>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<PostStatusRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();


builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ItemTypeService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<GenderTypeService>();
builder.Services.AddScoped<PlayerPositionService>();
builder.Services.AddScoped<PlayerStatusService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<PostStatusService>();
builder.Services.AddScoped<IMatchService, MatchService>();


builder.Services.AddScoped<MasterDataSeeder>();

builder.Services.AddScoped<AdminService>();


var app = builder.Build();

// Seed data (roles, superadmin, master data)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await IdentitySeeder.SeedSuperAdminAsync(userManager, roleManager);

    var masterDataSeeder = services.GetRequiredService<MasterDataSeeder>();
    masterDataSeeder.Seed();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // Must come before UseAuthorization()
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
