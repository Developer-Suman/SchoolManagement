using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords
{
    public class AddPaymentsRecordsCommandhandlers : IRequestHandler<AddPaymentsRecordsCommand, Result<AddpaymentsRecordsResponse>>
    {
        private readonly IValidator<AddPaymentsRecordsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IFeeTypeServices _feeServices;

        public AddPaymentsRecordsCommandhandlers(IValidator<AddPaymentsRecordsCommand> validator, IMapper mapper, IFeeTypeServices feeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _feeServices = feeServices;
        }
        public Task<Result<AddpaymentsRecordsResponse>> Handle(AddPaymentsRecordsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
