using Azure;
using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Data.Entities.Player;
using ManchesterUnitedApp.Data.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace ManchesterUnitedApp.Data.DataSeeders
{
    public class MasterDataSeeder
    {
        private readonly ApplicationDbContext _context;
        public MasterDataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if (!_context.ItemTypes.Any())
            {
                _context.ItemTypes.AddRange(
                new ItemType() { Name = "Shirt" },
                new ItemType() { Name = "Short" },
                new ItemType() { Name = "Sock" },
                new ItemType() { Name = "Ball" },
                new ItemType() { Name = "Hat" }
                );
                _context.SaveChanges();
            }

            if (!_context.Teams.Any())
            {
                _context.Teams.AddRange(
                new Team() { Name = "MEN" },
                new Team() { Name = "WOMEN" },
                new Team() { Name = "UNDER-21S" },
                new Team() { Name = "UNDER-18S" }
                );
                _context.SaveChanges();
            }

            if (!_context.GenderTypes.Any())
            {
                _context.GenderTypes.AddRange(
                new GenderType() { Name = "Male" },
                new GenderType() { Name = "Female" }
                );
                _context.SaveChanges();
            }
            if (!_context.PlayerPositions.Any())
            {
                _context.PlayerPositions.AddRange(
                    new PlayerPosition { Name = "Goalkeeper" },
                    new PlayerPosition { Name = "Defender" },
                    new PlayerPosition { Name = "Midfielder" },
                    new PlayerPosition { Name = "Forward" }
                );
            }
            if (! _context.PlayerStatuses.Any())
            {
                _context.PlayerStatuses.AddRange(
                    new PlayerStatus { Name = "Active" },
                    new PlayerStatus { Name = "Injured" },
                    new PlayerStatus { Name = "On Loan" }
                );
            }

            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                new Category() { Name = "Transfer", Description = "" },
                new Category() { Name = "Match Reports", Description = "" },
                new Category() { Name = "News", Description = "" },
                new Category() { Name = "Injuries", Description = "" }
                );
                _context.SaveChanges();
            }

            if (!_context.PostStatuses.Any())
            {
                _context.PostStatuses.AddRange(
                new PostStatus() { Name = "Draft" },
                new PostStatus() { Name = "Published" }
                );
                _context.SaveChanges();
            }
        }
    }
}
