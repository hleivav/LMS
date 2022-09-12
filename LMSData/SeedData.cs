using Bogus;
using LMS.Core.Entities;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Data
{
    public class SeedData 
    {
        private static Faker faker = null!;
        private static ApplicationDbContext db = default;
        private static RoleManager<IdentityRole<int>> roleManager = default;
        private static UserManager<User> userManager = default;


        public static async Task InitAsync
            (ApplicationDbContext context, IServiceProvider services, string adminPW)
        {
            if (string.IsNullOrWhiteSpace(adminPW))
                throw new Exception("Cant get password from config");
            if (context is null)
                throw new NullReferenceException(nameof(ApplicationDbContext));
            db = context;
            if (db.Users.Any()) return;                                                                 //togglar seed on off
            roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            if (roleManager is null)
                throw new NullReferenceException(nameof(RoleManager<IdentityRole<int>>));
            userManager = services.GetRequiredService<UserManager<User>>();
            if (userManager is null)
                throw new NullReferenceException(nameof(UserManager<User>));

            faker = new Faker();
            var teatcher = await roleManager.CreateAsync(new IdentityRole<int> { Name = "Teacher" });
            var student = await roleManager.CreateAsync(new IdentityRole<int> { Name = "Student" });

            var activitytype =new ActivityType();                   //create 4 activityTypes
            activitytype.Name = "E-Learning";
           await db.ActivityType.AddAsync(activitytype);

            activitytype = new ActivityType();
            activitytype.Name = "Programming Assignment";
            await db.ActivityType.AddAsync(activitytype);

            activitytype = new ActivityType();
            activitytype.Name = "Lectures";
            await db.ActivityType.AddAsync(activitytype);

            activitytype = new ActivityType();
            activitytype.Name = "Individual studies";
            await db.ActivityType.AddAsync(activitytype);
            

            await  db.SaveChangesAsync();

           
            for (int i = 0; i < 4; i++)                               //create 4 courses that dont overlap
            {


                var course = new Course();

                course.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                course.Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                    
                    course.StartDate = DateTime.Now.AddMonths(3*i - 3);
                course.EndDate = course.StartDate.AddMonths(3);

                await db.Course.AddAsync(course);
                await db.SaveChangesAsync();

                for (int u = 0; u < 10; u++)                            //create 10 users for each course of which one is a teacher
                {
                    var user = new User();



                    user.FirstName = faker.Name.FirstName();
                    user.LastName = faker.Name.LastName();
                    user.Email = $"{user.FirstName}.{user.LastName}@Lexicon.se";
                    user.UserName = user.Email;
                    user.EmailConfirmed= true;
                    user.CourseId = course.Id;
                    await userManager.CreateAsync(user, adminPW);

                    if (u == 0)
                    {
                        await userManager.AddToRoleAsync(user, "Teacher");
                    }
                    else await userManager.AddToRoleAsync(user, "Student");

                    await db.AddAsync(user);

                }

                    for (int r = 0; r < 3; r++)                                    //create 3 modules for each course without timeoverlap
                    {
                        var module=new Module();
                        module.Name= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                        module.Description= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                        module.StartDate = course.StartDate.AddMonths(r);
                        module.EndDate = module.StartDate.AddMonths(1);
                        module.CourseId = course.Id;
                        await db.AddAsync(module);
                        await db.SaveChangesAsync();
                        for (int a = 0; a < 4; a++)
                        {
                            var activity = new Activity();                                   //create 4 activities for each module without timeoverlap
                            activity.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                            activity.Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                            activity.StartDate = module.StartDate.AddDays(7 * a);
                            activity.EndDate = activity.StartDate.AddDays(7);
                            activity.ModuleId = module.Id;
                            activity.ActivityTypeId = r + 1;
                            await db.AddAsync(activity);
                        }
                    
                        await db.SaveChangesAsync();
                    };

                       
       
       

             };
                    await db.SaveChangesAsync();





                

               

          




        }
        


    }
}

