using AutoMapper;
using FluentValidation;
using MediumClone.Business.Interfaces;
using MediumClone.Business.ValidationRules;
using MediumClone.Common.ResponseObjects;
using MediumClone.DataAccess.UnitOfWork;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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



    }
}

