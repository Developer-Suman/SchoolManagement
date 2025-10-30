﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales
{
    public  class UpdateTaxStatusInSalesCommandValidator:AbstractValidator<UpdateTaxStatusInSalesCommand>
    {
        public UpdateTaxStatusInSalesCommandValidator()
        {
            RuleFor(x => x.schooLid).NotEmpty().WithMessage("SchooLi is Required");
        }
    }
}
