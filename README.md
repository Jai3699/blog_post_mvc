This project is on version .Net 8.0
Download below packacges in dependencies 
Cloudinary DotNet
Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.VisualStudio.Web.CodeGeneration.Design
Now go to Tools->NuGetPackageManager->Package Manager Console and write these below lines
Add-Migration "give_a_name" -context "BloggieDbContext"
Add-Migration "give_a_name" -context "AuthDbContext"
Update-Database -context "BloggieDbContext"
Update-Database -context "AuthDbContext"
