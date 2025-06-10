using Empty.Entity;
using Microsoft.EntityFrameworkCore;

namespace Empty
{
    public class DbCtx : DbContext
    {
        public DbSet<studyzzzz> studys => Set<studyzzzz>(); // DB 세션, 컨텍스트 이게 테이블이다 알려줌

        public DbCtx(DbContextOptions<DbCtx> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<studyzzzz>().ToTable("Entity");
            modelBuilder.Entity<studyzzzz>().HasKey(s => s.id);
        }
    }
}
