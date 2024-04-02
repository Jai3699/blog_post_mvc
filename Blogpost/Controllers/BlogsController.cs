using Blogpost.Models.Domain;
using Blogpost.Models.ViewModels;
using Blogpost.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace Blogpost.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository ) //inject repository
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }
        public async Task<IActionResult> Index(string urlHandle)//read more
        {
            var liked = false;
          var blogPost= await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            var blogDetailsViewModel = new BlogDetailsViewModel();
            //here we are creating a model which can send likes and blog information
            //for that we had created a BlogDetailsViewModel
            // and in the view of this function we have added @model Blogpost.Models.ViewModels.BlogDetailsViewModel to bind view
            // with the .cshtml file
            
            if (blogPost != null)
            {
               var totalLikes= await blogPostLikeRepository.GetTotalLikes(blogPost.Id);
                if (signInManager.IsSignedIn(User))
                {
                    //Get like for this blog for this user
                   var LikesForBLog= await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    //get user id from user manager
                    var userId = userManager.GetUserId(User);
                    //now we will check the number of likes by the user
                    if (userId != null)
                    {
                     var likeFromUser = LikesForBLog.FirstOrDefault(x=>x.UserId==Guid.Parse(userId));
                        liked=likeFromUser != null;
                        
                    }
                }
                //Get comments for blogpsot
                var blogCommentsDomainModel= await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
                var blogCommentsForView = new List<BlogComment>();
                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserID.ToString())).UserName
                    });
                }


                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked=liked,
                    Comments=blogCommentsForView
                };
            }
            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if(signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserID = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs",new {urlHandle=blogDetailsViewModel.UrlHandle});
            }
            return View();
         
            
        }
    }
}
