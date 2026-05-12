using AutoMapper;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.DeleteInstallmentsPlan
{
    public class DeleteInstallmentsPlanCommandHandler : IRequestHandler<DeleteInstallmentsPlanCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IInstallmentServices _installmentServices;

        public DeleteInstallmentsPlanCommandHandler(IMapper mapper, IInstallmentServices installmentServices)
        {
            _mapper = mapper;
            _installmentServices = installmentServices;
        }
        public async Task<Result<bool>> Handle(DeleteInstallmentsPlanCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteInstallmentsPlanCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var deleteEvents = await _installmentServices.Delete(request.Id, cancellationToken);
                if (deleteEvents is null)
                {
                    return Result<bool>.Failure("NotFound", $"{entityName} not Found");
                }
                return Result<bool>.Success(true, $"{entityName} Deleted Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
