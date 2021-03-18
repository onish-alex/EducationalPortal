namespace EducationPortal.MVC
{
    using System.Linq;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers.Profiles;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Settings;
    using EducationPortal.BLL.Validation;
    using EducationPortal.MVC.Utilities;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddSwaggerGen(x =>
            {
                x.CustomSchemaIds(type => type.FullName);
                x.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.AddHttpContextAccessor();

            services.AddBLLDependencies(this.Configuration);

            services.Configure<AccountSettings>(this.Configuration.GetSection("AccountSettings"));

            services.AddScoped<FluentValidation.IValidator<AccountDTO>, AccountValidator>(provider => new AccountValidator(provider.GetService<IOptionsSnapshot<AccountSettings>>()));
            services.AddScoped<FluentValidation.IValidator<UserDTO>, UserValidator>(provider => new UserValidator(provider.GetService<IOptionsSnapshot<AccountSettings>>()));
            services.AddScoped<FluentValidation.IValidator<MaterialDTO>, MaterialValidator>();
            services.AddScoped<FluentValidation.IValidator<SkillDTO>, SkillValidator>();
            services.AddScoped<FluentValidation.IValidator<CourseDTO>, CourseValidator>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MainProfile>());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMaterialService, MaterialService>();

            services.AddSingleton<IResourceHelper, ResourceHelper>();
            services.AddSingleton<ISignInManager, SignInManager>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        var isFromApi = context.Request.Path.StartsWithSegments(new PathString("/api"));

                        if (isFromApi)
                        {
                            context.Response.StatusCode = 401;
                        }
                        else
                        {
                            context.Response.StatusCode = 303;
                            context.Response.Headers["Location"] = "/Account/Login";
                        }

                        return Task.CompletedTask;
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Section/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EducationPortal V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Section}/{action=Global}/{id?}");
            });
        }
    }
}
