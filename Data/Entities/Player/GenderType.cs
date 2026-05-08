using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities.Player
{
    public class GenderType
    {
        public Guid Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        // ✅ Navigation property
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}