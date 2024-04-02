using Blogpost.Models.Domain;
using Blogpost.Models.ViewModels;
using Blogpost.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;

namespace Blogpost.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }
        public async Task<IActionResult> Add()
        {
            //get tags from repository
            var tags=await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,


            };
            //Map Tags from selected tags
            var selectedTag = new List<Tag>();
            foreach(var selectedTagsId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagsId);
                var existingTag= await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTag.Add(existingTag);

                }
            }
            blogPost.Tags = selectedTag;
            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Display");
        }

        public async Task<IActionResult> Display()
        {
            //call the repository 
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            //Reterive the result from the repository
            var blogPost= await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();


            //map the domain model into the view model
            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
                
            }
           
         
            


            //pass data to view
            return View(null);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                ShortDescription = editBlogPostRequest.ShortDescription,
                Visible = editBlogPostRequest.Visible,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,

            };
            //map tags into domain
            var selectedTags = new List<Tag>();
            foreach(var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if(Guid.TryParse(selectedTag,out var tag))
                {
                   var foundTag= await tagRepository.GetAsync(tag);
                    if(foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                    
                }
            }
            blogPostDomainModel.Tags = selectedTags;



            //submit informatuin to repository to update
            var updatedBlog= await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if(updatedBlog != null)
            {//show success message
                return RedirectToAction("Display");
            }
            //show error
            return RedirectToAction("Edit");

            //redirect to get

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // talk to repository to delete this blogpost and tags
            var deletedBlogPost= await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if(deletedBlogPost != null)
            {
                //show success message
                return RedirectToAction("Display");
            }
            //show error message
            return RedirectToAction("Edit", new {id=editBlogPostRequest.Id});
            //display the response


        }
    }
}
