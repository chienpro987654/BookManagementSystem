using BookManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "ecc324b0-6830-404f-9663-75b07defbf32", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Id = "6a0b036e-b3c3-4c39-ab24-07bd00938457", Name = "Manager", ConcurrencyStamp = "2", NormalizedName = "Manager" },
                new IdentityRole() { Id = "87b2c67f-c614-48c6-bc90-67a04322ee50", Name = "User", ConcurrencyStamp = "10", NormalizedName = "User" });

            //modelBuilder.UseCollation("Vietnamese_CI_AI");

            modelBuilder.Entity<Book>()
                .Property(x => x.Title).UseCollation("Vietnamese_CI_AI");
            modelBuilder.Entity<Book>()
                .Property(x => x.Description).UseCollation("Vietnamese_CI_AI");

            modelBuilder.Entity<Book>()
                .HasMany(x => x.Categories)
                .WithMany(y => y.Books)
                .UsingEntity<BookCategory>(
                l => l.HasOne<Category>(e => e.Category).WithMany(e => e.BookCategories),
                r => r.HasOne<Book>(e => e.Book).WithMany(e => e.BookCategories));

            modelBuilder.Entity<Author>()
                .HasMany(x => x.Books)
                .WithOne(y => y.Author)
                .HasForeignKey(e => e.AuthorId)
                .IsRequired(false);

            modelBuilder.Entity<Author>()
                .HasMany(x => x.Categories)
                .WithMany(x => x.Authors)
                .UsingEntity<AuthorCategory>(
                r => r.HasOne<Category>(e => e.Category).WithMany(e => e.AuthorCategories),
                l => l.HasOne<Author>(e => e.Author).WithMany(e => e.AuthorCategories)
                );
            modelBuilder.Entity<Cart>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<User>(x => x.CartId)
                .IsRequired(false);

            modelBuilder.Entity<Cart>()
                .HasMany(x => x.CardItems)
                .WithOne()
                .HasForeignKey(x => x.CartId)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .HasMany<CartItem>()
                .WithOne(x => x.Book).
                HasForeignKey(x => x.BookId)
                .IsRequired();

            modelBuilder.Entity<CartItem>().
                HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }

        public DbSet<Book> Books { get; set; }
        new
        public DbSet<User> Users
        { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
