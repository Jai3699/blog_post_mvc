namespace Blogpost.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);//once the image is uploaded it will return the url which will be saved to the database
    }
}
