using AutoMapper;
using FluentValidation;
using MediumClone.Business.Interfaces;
using MediumClone.Business.Mappings.AutoMapper;
using MediumClone.Business.Services;
using MediumClone.Business.ValidationRules;
using MediumClone.DataAccess.Contexts;
using MediumClone.DataAccess.UnitOfWork;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.DependencyResolvers.Microsoft
{
	public static class DependencyExtension
	{
		public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<NlogContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("Local"));
				opt.LogTo(Console.WriteLine, LogLevel.Information);
			});
			//var mapperConfiguration = new MapperConfiguration(opt =>
			//{
			//	opt.AddProfile(new BlogProfile());
			//	opt.AddProfile(new CategoryProfile());
			//	opt.AddProfile(new CommentProfile());
			//});

			//var mapper = mapperConfiguration.CreateMapper();
			//services.AddSingleton(mapper);


			services.AddIdentity<AppUser, AppRole>(opt =>
			{
				opt.Password.RequireDigit = false;
				opt.Password.RequiredLength = 1;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireNonAlphanumeric = false;
			}).AddEntityFrameworkStores<NlogContext>();

			services.AddTransient<IValidator<BlogCreateDto>, BlogCreateDtoValidator>();
			services.AddTransient<IValidator<BlogUpdateDto>, BlogUpdateDtoValidator>();
			services.AddTransient<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
			services.AddTransient<IValidator<CategoryUpdateDto>, CategoryUpdateDtoValidator>();
			services.AddTransient<IValidator<CommentCreateDto>, CommentCreateDtoValidator>();
			services.AddTransient<IValidator<CommentUpdateDto>, CommentUpdateDtoValidator>();
			
            services.AddScoped<IUow, Uow>();
			services.AddTransient<IBlogService, BlogService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<ICategoryService, CategoryService>();

			//services.AddTransient<IValidator<AppUserCreateDto> AppUserCreateDtoValidator>();	

		}
	}
}
