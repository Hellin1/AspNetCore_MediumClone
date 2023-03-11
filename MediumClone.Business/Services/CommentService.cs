using AutoMapper;
using FluentValidation;
using MediumClone.Business.Extensions;
using MediumClone.Business.Interfaces;
using MediumClone.Business.ValidationRules;
using MediumClone.Common.ResponseObjects;
using MediumClone.DataAccess.UnitOfWork;
using MediumClone.Dtos.Interfaces;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Services
{
    public class CommentService : Service<CommentCreateDto, CommentUpdateDto, CommentListDto, Comment>, ICommentService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<CommentCreateDto> _createDtoValidator;
        private readonly IValidator<CommentUpdateDto> _updateDtoValidator;

        public CommentService(IUow uow, IValidator<CommentCreateDto> createDtoValidator, IValidator<CommentUpdateDto> updateDtoValidator, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) : base(uow, createDtoValidator, updateDtoValidator, mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;

        }

        // IUow uow, IValidator<CreateDto> createDtoValidator, IValidator<UpdateDto> updateDtoValidator, IMapper mapper



        public async Task CreateComment(CommentCreateDto dto)
        {
            var result = _createDtoValidator.Validate(dto);

            if (result.IsValid)
            {
                var appUser = _userManager.Users.SingleOrDefault(x => x.Id == dto.UserId);
                var blog = await _uow.GetRepository<Blog>().GetById(dto.BlogId);

                await _uow.GetRepository<Comment>().CreateAsync(new()
                {
                    AppUser = appUser,
                    AppUserId = dto.UserId,
                    Content = dto.Content,
                    BlogId = dto.BlogId,
                    Blog = blog
                });

                await _uow.SaveChanges();
                
            }
        }

        public async Task<List<CommentListDto>> GetComments()
        {
            var comments = await _uow.GetRepository<Comment>().GetQuery().Include(x => x.AppUser).Include(x => x.Blog).Take(6).ToListAsync();
            var dto = _mapper.Map<List<CommentListDto>>(comments);
            return dto;
        }


        public async Task<String> GetCount()
        {
            var count = await _uow.GetRepository<Comment>().GetQuery().CountAsync();
            return count.ToString();
        }

        public async Task<IResponse<CommentUpdateDto>> UpdateComment(CommentUpdateDto dto)
        {
            var result = _updateDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                //var unchangedData = await _uow.GetRepository<Comment>().GetById(dto.Id);
                //public async Task<Blog> GetRelationalDataById(Expression<Func<Blog, bool>> filter)
                //{
                //    return await _context.Blogs.Include(x => x.Comments).ThenInclude(x => x.AppUser).AsNoTracking().SingleOrDefaultAsync(filter);
                //}



                var unchangedData =await  _uow.GetRepository<Comment>().GetQuery().Include(x => x.AppUser).Include(x => x.Blog).ThenInclude(x => x.AppUser).ThenInclude(x => x.Blogs).SingleOrDefaultAsync(x => x.Id == dto.Id);

                if (unchangedData == null)
                    return new Response<CommentUpdateDto>(ResponseType.NotFound, $"{dto.Id} id sine sahip kullanıcı bulunamadı.");
                unchangedData.UpdatedTime = DateTime.Now;
                unchangedData.Content = dto.Content;

                //var entity = _mapper.Map<Comment>(dto);


                //_uow.GetRepository<Comment>().Update(entity, unchangedData);
                await _uow.SaveChanges();
                return new Response<CommentUpdateDto>(ResponseType.Success, dto);
            }
            return new Response<CommentUpdateDto>(dto, result.ConvertToCustomValidationError());
        }

    }
}

