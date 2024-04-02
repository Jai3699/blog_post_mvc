using Blogpost.Data;
using Blogpost.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Blogpost.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext dbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.dbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
           await dbContext.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
           var existingBlog= await dbContext.BlogPosts.FindAsync(id);
            if(existingBlog != null)
            {
                dbContext.BlogPosts.Remove(existingBlog);
                await dbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await dbContext.BlogPosts.Include(x =>x.Tags).ToListAsync();
            
            

           
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
          return await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x =>x.Id== id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
           return await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.UrlHandle== urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
          var existingBlog= await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags=blogPost.Tags;  
                await dbContext.SaveChangesAsync();
                return existingBlog;


            }
            return null;
        }


    }
}
