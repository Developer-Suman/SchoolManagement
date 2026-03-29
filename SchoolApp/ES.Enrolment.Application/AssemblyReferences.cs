using ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor;
using ES.Enrolment.Application.Enrolments.Command.Enquiry.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddEnrolmentApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddInquiryCommand>, AddInquiryCommandValidators>();
            services.AddScoped<IValidator<ConvertApplicantCommand>, ConvertApplicantCommandValidators>();
            services.AddScoped<IValidator<ConvertStudentCommand>, ConvertStudentCommandValidator>();
            services.AddScoped<IValidator<AddCounselorCommand>, AddCounselorCommandValidator>();
            services.AddScoped<IValidator<AddAppointmentCommand>, AddAppointmentCommandValidator>();
            services.AddScoped<IValidator<AddConsultancyClassCommand>, AddConsultancyClassCommandValidator>();
            services.AddScoped<IValidator<AddTranningRegistrationCommand>, AddTranningRegistrationCommandValidator>();
            services.AddScoped<IValidator<AddFollowUpCommand>, AddFollowUpCommandValidator>();
            services.AddScoped<IValidator<UploadApplicantDocumentsCommand>, UploadApplicantDocumentsCommandValidators>();
            return services;
        }

    }
}
