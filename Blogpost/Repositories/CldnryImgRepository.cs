
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Blogpost.Repositories
{
    public class CldnryImgRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;
        public CldnryImgRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(configuration.GetSection("Cloudinary")["CloudName"],
                                  configuration.GetSection("Cloudinary")["ApiKey"],
                                  configuration.GetSection("Cloudinary")["ApiSecret"]);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            //we will use account to create client
            var client = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);

            if(uploadResult!=null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            return null;
        }
    }
}
