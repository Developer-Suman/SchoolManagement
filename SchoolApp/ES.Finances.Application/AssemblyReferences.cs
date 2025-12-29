using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ES.Finances.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddFinancesApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddFeeTypeCommand>, AddFeetypeCommandValidator>();
            services.AddScoped<IValidator<UpdateFeeStructureCommand>, UpdateFeeStructureCommandValidator>();
            services.AddScoped<IValidator<AddFeeStructureCommand>, AddFeeStructureCommandValidator>();
            services.AddScoped<IValidator<AddStudentFeeCommand>, AddStudentFeeCommandValidator>();
            services.AddScoped<IValidator<AddPaymentsRecordsCommand>, AddPaymentsRecordsCommandValidators>();
            services.AddScoped<IValidator<AssignMonthlyFeeCommand>, AssignMonthlyFeeCommandValidators>();
            return services;
        }

    }
}
