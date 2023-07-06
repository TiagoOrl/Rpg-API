using first_api.models;


namespace first_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {Id = 1, Name="Fireball", DamageVal = 34},
                new Skill {Id = 2, Name="Lightning", DamageVal = 44},
                new Skill {Id = 3, Name="Ice Shards", DamageVal = 34},
                new Skill {Id = 4, Name="Poison Gas", DamageVal = 50}
            );
        }

        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();
    }
}