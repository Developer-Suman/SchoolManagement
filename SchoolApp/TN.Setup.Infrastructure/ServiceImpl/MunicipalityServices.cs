using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId;
using TN.Setup.Application.Setup.Queries.Municipality;
using TN.Setup.Application.Setup.Queries.MunicipalityById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;


namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class MunicipalityServices : IMunicipalityServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public MunicipalityServices(IUnitOfWork unitOfWork, IMapper mapper,IMemoryCacheRepository memoryCacheRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository= memoryCacheRepository;
        }

        public async Task<Result<PagedResult<GetAllMunicipalityResponse>>> GetAllMunicipality(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try

            {


                var municipality = await _unitOfWork.BaseRepository<Municipality>().GetAllAsyncWithPagination();
                var municipalityPagedResult = await municipality.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allMunicipalityResponse = _mapper.Map<PagedResult<GetAllMunicipalityResponse>>(municipalityPagedResult.Data);

                
                return Result<PagedResult<GetAllMunicipalityResponse>>.Success(allMunicipalityResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Municipality", ex);
            }
        }

        public async Task<Result<GetMunicipalityByIdResponse>> GetMunicipalityById(int municipalityId, CancellationToken cancellationToken = default)
        {
            try
            {
  
                var municipality = await _unitOfWork.BaseRepository<Municipality>().GetById(municipalityId);
                var municipalityResponse = _mapper.Map<GetMunicipalityByIdResponse>(municipality);
               
              
                
                return Result<GetMunicipalityByIdResponse>.Success(municipalityResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Municipality  by using Id", ex);
            }
        }

        public async Task<Result<List<GetMunicipalityByDistrictIdResponse>>> GetMunicipalityByDistrictId(int districtId, CancellationToken cancellationToken = default)
        {
            try
            {

                var municipality= await _unitOfWork.BaseRepository<Municipality>().GetConditionalAsync(x => x.DistrictId == districtId);    
                if(municipality == null)
                {
                    return Result<List<GetMunicipalityByDistrictIdResponse>>.Failure("Not Found", "Municipality Data are not found");
                }
                var municipalityResponse=_mapper.Map<List<GetMunicipalityByDistrictIdResponse>>(municipality);
                
           
                
                return Result<List<GetMunicipalityByDistrictIdResponse>>.Success(municipalityResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Municipality  by using Id", ex);
            }


        }

       
        }
    }



