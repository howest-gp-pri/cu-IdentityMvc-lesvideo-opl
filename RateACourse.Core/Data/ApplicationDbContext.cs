using RateACourse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RateACourse.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<Student>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourseReview> StudentCourseReviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //course table
            modelBuilder.Entity<Course>()
                .Property(c => c.CourseName)
                .IsRequired()
                .HasMaxLength(150);
            //student table
            modelBuilder.Entity<Student>()
                .Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(150);
            modelBuilder.Entity<Student>()
                .Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(150);
            //configure StudentCourseReview
            modelBuilder.Entity<StudentCourseReview>()
                .HasKey(sc => new { sc.CourseId, sc.StudentId });
            modelBuilder.Entity<StudentCourseReview>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.Reviews)
                .HasForeignKey(s => s.StudentId);
            modelBuilder.Entity<StudentCourseReview>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Reviews)
                .HasForeignKey(cs => cs.CourseId);

            //seed admin role and admin user
            const string adminRoleId = "00000000-0000-0000-0000-000000000001";
            const string adminRoleName = "Admin";
            const string adminUserId = "00000000-0000-0000-0000-000000000001";
            const string adminUserName = "admin@pri.be";
            const string adminFirstname = "Mike";
            const string adminLastname = "Admin";
            const string adminPassword = "Test123";//only for testing purposes!
            //PasswordHasher
            IPasswordHasher<Student> passwordHasher = new PasswordHasher<Student>();
            //create the user
            Student adminUser = new Student 
            {
                Id = adminUserId,
                UserName = adminUserName,
                NormalizedUserName = adminUserName.ToUpper(),
                Email = adminUserName,
                NormalizedEmail = adminUserName.ToUpper(),
                FirstName = adminFirstname,
                LastName = adminLastname,
                SecurityStamp = "VVPCRDAS3MJWQD5CSW2GWPRADBXEZINA",
                ConcurrencyStamp = "c8554266-b401-4519-9aeb-a9283053fc58",
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser,adminPassword);
            //add admin role
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole 
                {
                    Id = adminRoleId,
                    Name = adminRoleName,
                    NormalizedName = adminRoleName.ToUpper(),
                }
                );
            //Add admin role to student
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> 
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId,
                }
                );
            //add user to database
            modelBuilder.Entity<Student>().HasData(adminUser);
            base.OnModelCreating(modelBuilder);
        }
    }
}
