using Microsoft.EntityFrameworkCore;
using AngularNewsApp.Models;

namespace AngularNewsApp.Models
{
    public class NewsAppContext : DbContext
    {
        public NewsAppContext(DbContextOptions<NewsAppContext> options)
         : base(options)
        { }


        //public DbSet<Article> Article { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<RssLink> RssLink { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Source> Source { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ArticleUser> ArticleUser { get; set; }
        public DbSet<UserValidationToken> UserValidationToken { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(x => x.RssLink)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.id_category);

            modelBuilder.Entity<RssLink>()
                .HasMany(x => x.Articles)
                .WithOne(x => x.Link)
                .HasForeignKey(x => x.id_rss_link);

            modelBuilder.Entity<Source>()
                .HasMany(x => x.Links)
                .WithOne(x => x.Source)
                .HasForeignKey(x => x.id_source);

            modelBuilder.Entity<Article>()
                .HasMany(x => x.ArticleUsers)
                .WithOne(x => x.Article)
                .HasForeignKey(x => x.id_article);

            modelBuilder.Entity<User>()
                 .HasMany(x => x.ArticleUsers)
                 .WithOne(x => x.User)
                 .HasForeignKey(x => x.id_user);

            modelBuilder.Entity<User>()
               .HasMany(x => x.Validations)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.id_user);
        }


        
    }
}