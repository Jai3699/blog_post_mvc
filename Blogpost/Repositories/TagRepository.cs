using Azure;
using Blogpost.Data;
using Blogpost.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogpost.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext dbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.dbContext = bloggieDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await dbContext.Tags.AddAsync(tag);//now repository will save the data to database 
            await dbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
           var existingTag= await dbContext.Tags.FindAsync(id);
            if(existingTag != null)
            {
                dbContext.Tags.Remove(existingTag);
                await dbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return await dbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
           return dbContext.Tags.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await dbContext.Tags.FindAsync(tag.Id);
            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await dbContext.SaveChangesAsync();
                return existingTag;

            }
            return null;


        }
    }
}
