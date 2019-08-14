using System.IO;
using AutoMapper;
using Orient.Base.Net.Core.Api.Core.Business.Caching;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Business.Models;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Extensions;
using Orient.Base.Net.Core.Api.Core.DataAccess;
using Orient.Base.Net.Core.Api.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.RMS.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.Business.Profiles;
using System.Linq;
using Orient.Base.Net.Core.Api.Core.Common.Utilities;
using System.Collections.Generic;
using Orient.Base.Net.Core.Api.Core.Business.Tasks;
using Orient.Base.Net.Core.Api.Core.Business.Services.Hubs;
using System;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using Orient.Base.Net.Core.Api.Core.Common.PDFNativeLib;
using DinkToPdf.Contracts;
using DinkToPdf;
using RazorLight;
using System.Reflection;
using Orient.Base.Net.Core.Api.Core.Business.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Orient.Base.Net.Core.Api.Core.Business.Filters.Models;
using System.Configuration;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories;

namespace Orient.Base.Net.Core.Api
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public static IConfigurationRoot Configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();


            var logPath = Configuration["AppSettings:LoggingPath"] + "Orient-{Date}-" + System.Environment.MachineName + ".txt";
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Warning()
              .WriteTo.RollingFile(logPath, retainedFileCountLimit: 15)
              .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                  builder => builder.SetIsOriginAllowed(host => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });

            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
              .AddJsonOptions(opt =>
              {
                  opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
              });

            services.AddSingleton(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(dispose: true);
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
            });
            services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>();

            //Config Automapper map
            Mapper.Initialize(config =>
            {
                config.AddProfile<CandidateProfile>();
                config.AddProfile<CategoryProfile>();
                config.AddProfile<UserProfile>();
                config.AddProfile<JobProfile>();
                config.AddProfile<CalendarProfile>();
                config.AddProfile<CommentProfile>();
                config.AddProfile<StepInJobProfile>();
                config.AddProfile<QuestionProfile>();
                config.AddProfile<AnswerProfile>();
                config.AddProfile<DepartmentProfile>();
            });

            var conn = Configuration.GetConnectionString("DefaultConnectionString");
            services.AddDbContextPool<OrientNetCoreDbContext>(options => options.UseSqlServer(conn));

            //Register Repository
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //Config Ldap
            services.AddScoped<IAuthenticationService, LdapAuthenticationService>();

            //Register Service
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISSOAuthService, SSOAuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IStepInJobService, StepInJobService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IInterviewService, InterviewService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<ICalendarTypeService, CalendarTypeService>();
            services.AddScoped<ITechnicalSkillService, TechnicalSkillService>();

            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();

            services.AddScoped<IPDFService, PDFService>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            //Register MemoryCacheManager
            services.AddScoped<ICacheManager, MemoryCacheManager>();

            //Register Hosted Services
            services.AddHostedService<BackgroundTask>();
            //services.AddHostedService<EmailBackgroundTask>();

            // Set Service Provider for IoC Helper
            IoCHelper.SetServiceProvider(services.BuildServiceProvider());

            services.AddMvc(option =>
            {
                option.Filters.Add<HandleExceptionFilterAttribute>();
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Orient API",
                    Description = "ASP.NET Core API.",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "DINH KHAC HOAI PHUNG", Email = "phung.dinh@orientsoftware.com", Url = "" },
                });

                c.DescribeAllParametersInCamelCase();
                c.OperationFilter<AccessTokenHeaderParameterOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Orient.Base.Net.Core.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

            // Add DinkToPDF Context
            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            var wkHtmlToPdfPath = Path.Combine(_hostingEnvironment.ContentRootPath, $"Core\\Common\\PDFNativeLib\\wkhtmltox\\v0.12.4\\{architectureFolder}\\libwkhtmltox");
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            // Add RazorLight
            services.AddScoped<IRazorLightEngine>(sp =>
            {
                var engine = new RazorLightEngineBuilder()
                    .UseFilesystemProject(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                    .UseMemoryCachingProvider()
                    .Build();
                return engine;
            });

            // config signal R
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // global policy - assign here or on each controller
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddSerilog();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Debug);
            }
            else if (env.IsProduction())
            {
                loggerFactory.AddSerilog();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Warning);
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orient API V1");
            });

            app.UseMvc();

            // Auto run migration
            RunMigration(app);

            // Initialize Data
            InitDataRole();
            InitUserSuperAdmin();
            InitDataDepartment();
            InitCalendarType();
            InitEmailTemplate();

            // config signal R 
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/chatHub");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        private void RunMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<OrientNetCoreDbContext>().Database.Migrate();
            }
        }

        private void InitDataRole()
        {
            var roleRepository = IoCHelper.GetInstance<IRepository<Role>>();
            var roles = new[]
            {
                new Role {
                    Id = RoleConstants.SuperAdminId,
                    Name = "Super Admin"
                },
                new Role {
                    Id = RoleConstants.AdminId,
                    Name = "Admin"
                },
                new Role {
                    Id = RoleConstants.NormalUserId,
                    Name = "Normal"
                },
            };

            roleRepository.GetDbContext().Roles.AddIfNotExist(x => x.Name, roles);
            roleRepository.GetDbContext().SaveChanges();
        }

        private void InitDataDepartment()
        {
            var departmentRepository = IoCHelper.GetInstance<IRepository<Department>>();
            var departments = new[]
            {
                new Department {
                    Id = DepartmentConstants.OfficeManager,
                    Name = "Office Manager",
                    ParentId=null
                },
                new Department {
                    Id = DepartmentConstants.Recruitment,
                    Name = "Recruitment",
                    ParentId=DepartmentConstants.OfficeManager
                },
                new Department {
                    Id = DepartmentConstants.CB,
                    Name = "C&B",
                    ParentId=DepartmentConstants.OfficeManager
                },
                new Department {
                    Id = DepartmentConstants.AdminReceptionist,
                    Name = "Admin cum Receptionist",
                    ParentId=DepartmentConstants.OfficeManager
                },
                new Department {
                    Id = DepartmentConstants.AdminEngagement,
                    Name = "Admin & Engagement",
                    ParentId=DepartmentConstants.OfficeManager
                },
            };

            departmentRepository.GetDbContext().Departments.AddIfNotExist(x => x.Name, departments);
            departmentRepository.GetDbContext().SaveChanges();
        }

        private void InitUserSuperAdmin()
        {
            var userRepository = IoCHelper.GetInstance<IRepository<User>>();
            if (userRepository.GetAll().Count() > 1)
            {
                return;// It's already init
            }

            var user = new User();
            user.Name = "Super Admin";
            user.Email = "phung.dinh@orientsoftware.com";
            user.Mobile = "0983260830";

            var password = "orient@123";
            password.GeneratePassword(out string saltKey, out string hashPass);

            user.Password = hashPass;
            user.PasswordSalt = saltKey;

            user.UserInRoles = new List<UserInRole>()
                    {
                        new UserInRole()
                        {
                            UserId = UserConstants.SuperAdminUserId,
                            RoleId = RoleConstants.SuperAdminId
                        }
                    };

            var users = new[]
            {
                user
            };

            userRepository.GetDbContext().Users.AddIfNotExist(x => x.Email, users);
            userRepository.GetDbContext().SaveChanges();
        }

        private void InitCalendarType()
        {
            var calendarTypeRepository = IoCHelper.GetInstance<IRepository<CalendarType>>();

            var calendarTypes = new[]
            {
                new CalendarType {
                    Name = "Interview"
                },
                new CalendarType {
                    Name = "New Comer"
                },
                new CalendarType {
                    Name = "Birthday"
                },
                new CalendarType {
                    Name = "Other"
                },
            };

            foreach (var calendarType in calendarTypes)
            {
                if (calendarTypeRepository.GetDbContext().CalendarTypes.FirstOrDefault(x => x.Name == calendarType.Name) == null)
                {
                    calendarTypeRepository.GetDbContext().CalendarTypes.Add(calendarType);
                }
            }

            calendarTypeRepository.GetDbContext().SaveChanges();
        }

        private void InitEmailTemplate()
        {
            var emailTemplateRepository = IoCHelper.GetInstance<IRepository<EmailTemplate>>();

            if (emailTemplateRepository.GetAll().Count() >= 3)
            {
                return;
            }

            var emailTemplates = new EmailTemplate[]
            {
                new EmailTemplate()
                {
                    Name = "Job",
                    Subject = "You have a new job.",
                    From = "quang.nguyen@orientsoftware.com",
                    FromName = "Quang",
                    CC = "",
                    BCC = "",
                    Body = DataInitializeHelper.GetResourceContent("JobRemindTemplate.txt", DataSetupResourceType.EmailTemplate),
                    Type = EmailTemplateType.Job,
                },
                new EmailTemplate()
                {
                    Name = "Interview",
                    Subject = "You have a new interview.",
                    From = "phuong.trinh@orientsoftware.com",
                    FromName = "Phuong",
                    CC = "",
                    BCC = "",
                    Body = DataInitializeHelper.GetResourceContent("InterviewRemindTemplate.txt", DataSetupResourceType.EmailTemplate),
                    Type = EmailTemplateType.Interview,
                },
                new EmailTemplate()
                {
                    Name = "Calendar",
                    Subject = "You have a new remind.",
                    From = "thanh.nguyen@orientsoftware.com",
                    FromName = "Thanh",
                    CC = "",
                    BCC = "",
                    Body = DataInitializeHelper.GetResourceContent("CalendarRemindTemplate.txt", DataSetupResourceType.EmailTemplate),
                    Type = EmailTemplateType.Calendar
                }
            };
            emailTemplateRepository.GetDbContext().EmailTemplates.AddRange(emailTemplates);
            emailTemplateRepository.GetDbContext().SaveChanges();
        }
    }
}
