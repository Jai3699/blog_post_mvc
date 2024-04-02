using Blogpost.Data;
using Blogpost.Models.Domain;
using Blogpost.Models.ViewModels;
using Blogpost.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogpost.Controllers
{
    [Authorize(Roles = "Admin")] //Every method in this controller has to go through Authorization process
    public class AdminController : Controller
    {
        private readonly ITagRepository tagRepository;

        // private BloggieDbContext dbContext;
        public AdminController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        
        public async Task<IActionResult> Add()
        {
            return View();
        }
        
        [HttpPost]
        
        public async Task<IActionResult> Add(AddTagRequest obj) {
            ValidateAddTagRequest(obj); //this is for custom validation that our name and display name should not be same
            if (ModelState.IsValid == false)
            {
                return View();
            }
            var tag = new Tag
            {
                Name = obj.Name,
                DisplayName = obj.DisplayName,
            };
           
                await tagRepository.AddAsync(tag);//now its the job of repository to save the data to the database not controller's job
                return RedirectToAction("Display");
        
        }
        
        public async Task<IActionResult> Display()
        {
            var data = await tagRepository.GetAllAsync();
            return View(data);
        }
        
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag=await tagRepository.GetAsync(id);
  
             if(tag!=null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }




            return View(null);
        }
     
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };
          var updatedTag= await tagRepository.UpdateAsync(tag);
            if(updatedTag != null)
            {
                //show success msg
            }
            else
            {
                //show error msg
            }

            // return RedirectToAction("Edit", new {id=editTagRequest.Id});
            return RedirectToAction("Display");
        }

     
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
       var deleted=await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deleted != null)
            {
                //show success notification
                return RedirectToAction("Display");
            }
            //show error 
            return RedirectToAction("Edit",new {id=editTagRequest.Id});

        }
        
        private void ValidateAddTagRequest(AddTagRequest obj)
        {
            if(obj.Name is not null && obj.DisplayName is not null)
            {
                if (obj.Name == obj.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Please don't copy the Name");
                }
            }
        }


    }
}
