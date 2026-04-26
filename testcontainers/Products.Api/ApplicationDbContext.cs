using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;

namespace Products.Api;

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
