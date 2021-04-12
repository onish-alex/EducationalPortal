namespace EducationPortal.DAL.DbContexts
{
    using EducationPortal.DAL.Entities.EF;
    using Microsoft.EntityFrameworkCore;

    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User and Account entities as one table
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Account>().ToTable("Users");

            modelBuilder.Entity<User>().HasOne(u => u.Account).WithOne(a => a.User).HasForeignKey<User>(u => u.Id);
            modelBuilder.Entity<Account>().HasOne(a => a.User).WithOne(u => u.Account).HasForeignKey<Account>(u => u.Id);

            // Configure Material subtypes as different tables
            modelBuilder.Entity<Article>().ToTable("Articles");
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Video>().ToTable("Videos");

            // Configure User - Course "one to many" relation
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedCourses)
                .HasForeignKey(c => c.CreatorId);

            // Configure User - Skill "many to many" relation
            modelBuilder.Entity<UserSkills>().HasKey(us => new { us.UserId, us.SkillId });

            modelBuilder.Entity<UserSkills>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSkills>()
                .HasOne(us => us.Skill)
                .WithMany(s => s.UserSkills)
                .HasForeignKey(us => us.SkillId);

            // Configure User - User joined courses "many to many" relation
            modelBuilder.Entity<UserJoinedCourses>().HasKey(ujc => new { ujc.UserId, ujc.CourseId });

            modelBuilder.Entity<UserJoinedCourses>()
                .HasOne(ujc => ujc.User)
                .WithMany(u => u.JoinedCourses)
                .HasForeignKey(ujc => ujc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserJoinedCourses>()
                .HasOne(ujc => ujc.Course)
                .WithMany(c => c.JoinedUsers)
                .HasForeignKey(ujc => ujc.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure User - User completed courses "many to many" relation
            modelBuilder.Entity<UserCompletedCourses>().HasKey(ujc => new { ujc.UserId, ujc.CourseId });

            modelBuilder.Entity<UserCompletedCourses>()
                .HasOne(ujc => ujc.User)
                .WithMany(u => u.CompletedCourses)
                .HasForeignKey(ujc => ujc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserCompletedCourses>()
                .HasOne(ujc => ujc.Course)
                .WithMany(c => c.CompletedUsers)
                .HasForeignKey(ujc => ujc.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure property constraints
            modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50);

            modelBuilder.Entity<Account>().Property(a => a.Email).HasMaxLength(50);
            modelBuilder.Entity<Account>().Property(a => a.Login).HasMaxLength(20);
            modelBuilder.Entity<Account>().Property(a => a.Password).HasMaxLength(128);

            modelBuilder.Entity<Course>().Property(c => c.Name).HasMaxLength(100);
            modelBuilder.Entity<Course>().Property(c => c.Description).HasMaxLength(250);

            modelBuilder.Entity<Skill>().Property(s => s.Name).HasMaxLength(30);

            modelBuilder.Entity<Material>().Property(m => m.Name).HasMaxLength(50);
            modelBuilder.Entity<Material>().Property(m => m.Url).HasMaxLength(200);

            modelBuilder.Entity<Book>().Property(b => b.AuthorNames).HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.Format).HasMaxLength(15);

            modelBuilder.Entity<Video>().Property(v => v.Quality).HasMaxLength(20);

            // Configuring property types
            modelBuilder.Entity<Article>().Property(a => a.PublicationDate).HasColumnType("date");

            modelBuilder.Entity<Video>().Property(v => v.Duration).HasColumnType("time(0)");
        }
    }
}
