using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Meeting.Models;
using Smart_Meeting.Data;
using System;
using Smart_Meeting.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Security.Claims;

namespace Smart_Meeting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Smart_Meeting.DTOs.AutoMapper));
            builder.Services.AddHostedService<MeetingStatusBackgroundService>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy",
                    builder => builder.WithOrigins("https://smart-meeting-ids-internship-frontend-9jnu.onrender.com").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });


            builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<Employee,IdentityRole>(options =>
            {
                //password requirements
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true; 
            }).AddEntityFrameworkStores<AppDBContext>(); //Use Entity Framework to store identity information in the AppDBContext

            // Add JWT Bearer authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            { 
                // Define how incoming JWT tokens should be validated
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],

                    // Validate the signing key used to generate the token(Ensure the token was signed with a valid key)

                    ValidateIssuerSigningKey = true,

                    //The key used to verify the token's signature
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                        )
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                //checking ownership of resources
                options.AddPolicy("OwnerOnly", policy =>
                    policy.RequireClaim("employee_id"));

                //Restrict access to admin-only routes 
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                //Owner only and also let admin have access
                options.AddPolicy("OwnerOrAdmin", policy =>
                    policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "employee_id") ||
                    context.User.IsInRole("Admin")));

                //Let employees access a resource, but also allow admins to
                options.AddPolicy("EmployeeOrAdmin", policy =>
                policy.RequireAssertion(context =>
                context.User.IsInRole("Employee") || context.User.IsInRole("Admin")));

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            //This middleware reads the token from the cookie and sets it in the Authorization header
            //so that the JWT Bearer middleware can validate it.
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Authorization = $"Bearer {token}";
                }

                await next();
            });

            app.UseRouting();
            app.UseCors("ReactPolicy");
            app.UseAuthentication();
            app.UseAuthorization();            
            app.UseHttpsRedirection();
            app.MapControllers();


            app.Run();
        }
    }
}
