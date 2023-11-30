﻿using Microsoft.EntityFrameworkCore;

namespace DevagramCShrap.Models
{
    public class DevagramContext : DbContext
    {
        public DevagramContext(DbContextOptions<DevagramContext> option) : base(option) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Seguidor> Seguidores { get; set; }
    }
}
