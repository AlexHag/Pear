using Microsoft.EntityFrameworkCore;
using Pear.Discovery.Entities;
using System;

namespace Pear.Discovery.Persistance;

public class PearDbContext : DbContext
{
    public DbSet<PearFriend> PearFriends { get; set; }
    public DbSet<PearMessage> PearMessages { get; set; }

    public PearDbContext(DbContextOptions<PearDbContext> options) : base(options)
    { }
}
