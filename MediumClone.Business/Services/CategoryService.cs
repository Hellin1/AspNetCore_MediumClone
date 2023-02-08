using AutoMapper;
using FluentValidation;
using MediumClone.Business.Interfaces;
using MediumClone.Common.ResponseObjects;
using MediumClone.DataAccess.UnitOfWork;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Services
{
    public class CategoryService : Service<CategoryCreateDto, CategoryUpdateDto, CategoryListDto, Category>, ICategoryService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDto> _createDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _updateDtoValidator;
        public CategoryService(IUow uow, IValidator<CategoryCreateDto> createDtoValidator, IValidator<CategoryUpdateDto> updateDtoValidator, IMapper mapper) : base(uow, createDtoValidator, updateDtoValidator, mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
        }

        public IResponse<List<CategoryListDto>> GetSearchResult(string searchWord)
        {
            //var context.MyTable.Where(x => EF.Functions.ILike(x.Name, $"%{searchWord}%")).ToList();

            var categorySearchResult =  _uow.GetRepository<Category>().GetQuery().Where(x => EF.Functions.Like(x.Title, $"%{searchWord}%")).ToList();

            var dto = _mapper.Map<List<CategoryListDto>>(categorySearchResult);

            return new Response<List<CategoryListDto>>(ResponseType.Success, dto);
        }


        public IResponse<List<BlogListDto>> GetBlogsByCategory(int categoryId)
        {
            //    //var blogs = _uow.GetRepository<Blog>().GetQuery().Where(x => x.BlogCategories).Include(x => x.BlogCategories).ThenInclude(x => x.Category);


            //    //var blogs = _uow.GetRepository<Blog>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.CategoryId).Where(x => x.CategoryId == categoryId);

            //    //return await Response<Blog>();

            //var blogs = _uow.GetRepository<Category>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Blog).FirstOrDefault(x => x.Id == categoryId);
            //var blogs2 = _uow.GetRepository<Category>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Blog).SingleOrDefault(x => x.Id == categoryId);
            //var blog3 = _uow.GetRepository<Category>().GetQuery().Join();

        //    context.Categories
        //.Where(c => c.Id == categoryId)
        //.SelectMany(c => c.BlogCategories)
        //.Select(bc => bc.Blog)
        //.ToList();

            //var blogs4 = _uow.GetRepository<Category>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Blog).Where(x => x.Category.Id == categoryId).FirstOrDefault();


            // pagination need

            var blogs5 = _uow.GetRepository<Category>().GetQuery().Where(x => x.Id == categoryId).SelectMany(x => x.BlogCategories).Select(x => x.Blog).ToList();

            var dto = _mapper.Map<List<BlogListDto>>(blogs5);

            return new Response<List<BlogListDto>>(ResponseType.Success, dto);

        }
    }
}
