using Microsoft.EntityFrameworkCore;
using Blogpost.Models.Domain;

namespace Blogpost.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {
        }

        /*
         *  The DbSet<T> properties represent collections of the specified entities in the context.
         *  In this case, there are four DbSet properties:
         */
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLikes { get; set; }
        public DbSet<BlogPostComment> BlogPostComments { get; set;}
    }
}
