using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public class OdeToFoodDbContext : DbContext
    {
        public OdeToFoodDbContext(DbContextOptions<OdeToFoodDbContext> options) : base (options)
        {

        }

        // Use DbSet when handling CRUD operations on this collection. For get only - use a simple list.
        public DbSet<Resturant> Resturants { get; set; }
    }
}
