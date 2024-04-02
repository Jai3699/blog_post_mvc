using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogpost.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //This is a way of seeding data into the database.
            // create roles (User,Admin,SuperAdmin)

            //we need a guid id here for every role,to create that we have to go to view-otherwindows-c# interactive 
            //and write Console.WriteLine(Guid.NewGuid());

            var adminRoleId = "ff7f28d9-0a9a-4c4e-81bb-e0158b5f9c8a";
            var superAdminRoleId = "f7b94a11-3a93-457a-998e-7def2bbe3b73";
            var userRoleId = "7745803b-b20d-455e-87c6-3b23afe88549";

            var roles = new List<IdentityRole>
            {

                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                 new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                },
                  new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId
                }

            }; //These are the list of roles 

            //now we have to insert the roles in the builder above so that
            // when we run entityframework core migrations these roles will be taken as seed
            //and inserted in the database
            builder.Entity<IdentityRole>().HasData(roles); //now entity framework will create the 
            //roles in the database


            //now we will create SuperAdminUser
            var superAdminId = "42d59ac9-c314-453c-a767-1a276c431af4";
            var superAdminUser = new IdentityUser
            {
                UserName = "superAdmin@myblog.com",
                Email = "superAdmin@myblog.com",
                NormalizedEmail = "superAdmin@myblog.com".ToUpper(),
                NormalizedUserName = "superAdmin@myblog.com".ToUpper(),
                Id = superAdminId
            };

            //now we have to create the password
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().
                HashPassword(superAdminUser, "Jai@12345");


            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //now we will give the roles to the superAdmin

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId= adminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                 {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId

                },
                new IdentityUserRole<string>
                  {
                    RoleId=userRoleId,
                    UserId=superAdminId

                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);


        }
    }
}
