using AutoMapper;
using FluentValidation;
using MediumClone.Business.Extensions;
using MediumClone.Business.Interfaces;
using MediumClone.Common.ResponseObjects;
using MediumClone.DataAccess.UnitOfWork;
using MediumClone.Dtos.Interfaces;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Services
{
    public class Service<CreateDto, UpdateDto, ListDto, T> : IService<CreateDto, UpdateDto, ListDto, T>
        where CreateDto : class, IDto, new()
        where UpdateDto : class, IUpdateDto, new()
        where ListDto : class, IDto, new()
        where T : BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IUow _uow;
        private readonly IValidator<CreateDto> _createDtoValidator;
        private readonly IValidator<UpdateDto> _updateDtoValidator;

        public Service(IUow uow, IValidator<CreateDto> createDtoValidator, IValidator<UpdateDto> updateDtoValidator, IMapper mapper)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
            _mapper = mapper;
        }

        public async Task<IResponse<CreateDto>> CreateAsync(CreateDto dto)
        {
            var result = _createDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var createdEntity = _mapper.Map<T>(dto);
                await _uow.GetRepository<T>().CreateAsync(createdEntity);
                await _uow.SaveChanges();
                return new Response<CreateDto>(ResponseType.Success, dto);
            }
            return new Response<CreateDto>(dto, result.ConvertToCustomValidationError());

        }

        public async Task<IResponse<List<ListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRepository<T>().GetAllAsync();
            var dto = _mapper.Map<List<ListDto>>(data);

            return new Response<List<ListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<IDto>> GetByIdAsync<IDto>(int id)
        {
            var data = await _uow.GetRepository<T>().GetByFilter(x => x.Id == id);
            if (data == null)
            {
                return new Response<IDto>(ResponseType.NotFound, $"{id} id sine sahip data bulunamadı");
            }
            var dto = _mapper.Map<IDto>(data);
            return new Response<IDto>(ResponseType.Success, dto);
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var data = await _uow.GetRepository<T>().GetById(id);
            if (data == null)
            {
                return new Response(ResponseType.NotFound, $"{id} id sine sahip data bulunamadı");

            }
            _uow.GetRepository<T>().Remove(data);
            await _uow.SaveChanges();
            return new Response(ResponseType.Success);
        }

        public async Task<IResponse<UpdateDto>> UpdateAsync(UpdateDto dto)
        {
            var result = _updateDtoValidator.Validate(dto);
            if (result.IsValid)
            {
                var unchangedData = await _uow.GetRepository<T>().GetById(dto.Id);
                if (unchangedData == null)
                    return new Response<UpdateDto>(ResponseType.NotFound, $"{dto.Id} id sine sahip kullanıcı bulunamadı.");
                var entity = _mapper.Map<T>(dto);
                _uow.GetRepository<T>().Update(entity, unchangedData);
                await _uow.SaveChanges();
                return new Response<UpdateDto>(ResponseType.Success, dto);
            }
            return new Response<UpdateDto>(dto, result.ConvertToCustomValidationError());
        }
    }
}
