using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetVdcByDistrictId;
using TN.Setup.Application.Setup.Queries.Vdc;
using TN.Setup.Application.Setup.Queries.VdcById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class VdcServices : IVdcServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;

        public VdcServices(IUnitOfWork unitOfWork,IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<GetAllVdcResponse>>> GetAllVdc(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {

            try
            {

                var vdc = await _unitOfWork.BaseRepository<Vdc>().GetAllAsyncWithPagination();
                var vdcPagedResult = await vdc.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allVdcResponse = _mapper.Map<PagedResult<GetAllVdcResponse>>(vdcPagedResult.Data);
          
                return Result<PagedResult<GetAllVdcResponse>>.Success(allVdcResponse);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Vdc", ex);
            }

        }

        public async Task<Result<List<GetVdcByDistrictIdResponse>>> GetVdcByDistrictId(int districtId, CancellationToken cancellationToken = default)
        {
            try
            {

                var vdc = await _unitOfWork.BaseRepository<Vdc>().GetConditionalAsync(x => x.DistrictId == districtId);
                if (vdc == null)
                {
                    return Result<List<GetVdcByDistrictIdResponse>>.Failure("Not Found", "Vdc Data are not found");
                }
                var vdcResponse = _mapper.Map<List<GetVdcByDistrictIdResponse>>(vdc);

           


                return Result<List<GetVdcByDistrictIdResponse>>.Success(vdcResponse);

                 }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public async Task<Result<GetVdcByIdResponse>> GetVdcById(int vdcId, CancellationToken cancellationToken = default)
        {
            try
            {

                var vdc = await _unitOfWork.BaseRepository<Vdc>().GetById(vdcId);
                var vdcResponse = _mapper.Map<GetVdcByIdResponse>(vdc);
            
                return Result<GetVdcByIdResponse>.Success(vdcResponse);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching the Vdc by using Id", ex);

            }
        }

        

        
    }
}
