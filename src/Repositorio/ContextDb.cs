
using backEndMaster.Modelos;
using Microsoft.EntityFrameworkCore;

namespace backEndMaster.Context
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options)
            : base(options)
        { }

        public DbSet<User> Usuarios { get; set; }
    }
}