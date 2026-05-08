using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Data.Entities.Player;
using ManchesterUnitedApp.Data.Entities.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Emit;
using ManchesterUnitedApp.Models.Post;
using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Models.PurchaseModel;
using ManchesterUnitedApp.Data.Entities.MatchEntity;
using ManchesterUnitedApp.Models.ViewModel;

namespace ManchesterUnitedApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }
        public DbSet<PlayerStatus> PlayerStatuses { get; set; }
        public DbSet<GenderType> GenderTypes { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostStatus> PostStatuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //one -many
            builder.Entity<Item>()
                .HasOne(t => t.ItemType)
                .WithMany(c => c.Items)
                .HasForeignKey(t => t.ItemTypeId);

            builder.Entity<Purchase>()
                .HasOne(t => t.Item)
                .WithMany(c => c.Purchases)
                .HasForeignKey(t => t.ItemId);
            //_________________________________________

            builder.Entity<Player>()
                .HasOne(p => p.PlayerPosition)
                .WithMany(c => c.Players)
                .HasForeignKey(p => p.PlayerPositionId);

            builder.Entity<Player>()
                .HasOne(p => p.GenderType)
                .WithMany(c => c.Players)
                .HasForeignKey(p => p.GenderTypeId);

            builder.Entity<Player>()
                .HasOne(p => p.PlayerStatus)
                .WithMany(c => c.Players)
                .HasForeignKey(p => p.PlayerStatusId);

            builder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(c => c.Players)
                .HasForeignKey(p => p.TeamId);
            //_____________________________________________

            builder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId);

            builder.Entity<Post>()
                .HasOne(p => p.PostStatus)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.PostStatusId);

            //Change Identity default table names
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");

            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
       
        public DbSet<ManchesterUnitedApp.Models.Post.PostDetailModel> PostDetailModel { get; set; } = default!;
        public DbSet<ManchesterUnitedApp.Models.Player.PlayerDetailModel> PlayerDetailModel { get; set; } = default!;
        public DbSet<ManchesterUnitedApp.Models.PurchaseModel.PurchaseListModel> PurchaseListModel { get; set; } = default!;
        public DbSet<ManchesterUnitedApp.Models.PurchaseModel.PurchaseDetailModel> PurchaseDetailModel { get; set; } = default!;
        public DbSet<ManchesterUnitedApp.Models.ViewModel.MatchViewModel> MatchViewModel { get; set; } = default!;
        public DbSet<ManchesterUnitedApp.Models.ViewModel.MatchCreateViewModel> MatchCreateViewModel { get; set; } = default!;
      
    }
}
