using ES.Enrolment.Application.Enrolments.Command.AddConsultancyClass.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.AddFollowUp.RequestCommand_Mapper;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor;
using ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.Enquiry.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.Enquiry.AddInquiry.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.Applicant;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.ApplicantsById;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.ScheduleAppointment;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.Counselor;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor;
using ES.Enrolment.Application.Enrolments.Queries.CRMStudents.CRMStudentsById;
using ES.Enrolment.Application.Enrolments.Queries.CRMStudents.FilterCRMStudents;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.InqueryById;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.Inquiry;
using ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp;
using ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetAllUserProfile;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetUserProfileById;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Communication.v1;

namespace TN.Web.Controllers.Enrolments.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class EnrolmentsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EnrolmentsController> _logger;

        public EnrolmentsController(IMediator mediator, ILogger<EnrolmentsController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }




        #region AddFollowUp
        [HttpPost("AddFollowUp")]

        public async Task<IActionResult> AddFollowUp([FromBody] AddFollowUpRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddFollowUp), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion
        #region FilterFollowUps
        [HttpGet("FilterFollowUps")]
        public async Task<IActionResult> FilterFollowUps([FromQuery] FilterFollowUpDTOs filterFollowUpDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterFollowUpQuery(paginationRequest, filterFollowUpDTOs);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region TrainingRegistration

        #region FilterTrainingRegistration
        [HttpGet("FilterTrainingRegistration")]
        public async Task<IActionResult> FilterTrainingRegistration([FromQuery] FilterTrainingRegistrationDTOs filterTrainingRegistrationDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterTrainingRegistrationQuery(paginationRequest, filterTrainingRegistrationDTOs);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddTrainingRegistration
        [HttpPost("AddTrainingRegistration")]

        public async Task<IActionResult> AddTrainingRegistration([FromBody] AddTranningRegistrationRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddTrainingRegistration), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #endregion




        #region ConsultancyClasss
        #region AllConsultancyClasss
        [HttpGet("AllConsultancyClasss")]
        public async Task<IActionResult> AllConsultancyClasss([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ConsultancyClassQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #region FilterConsultancyClasss
        [HttpGet("FilterConsultancyClasss")]
        public async Task<IActionResult> FilterConsultancyClasss([FromQuery] FilterConsultancyClassDTOs filterConsultancyClassDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterConsultancyClassQuery(filterConsultancyClassDTOs,paginationRequest);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddConsultancyClass
        [HttpPost("AddConsultancyClass")]

        public async Task<IActionResult> AddConsultancyClass([FromBody] AddConsultancyClassRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddConsultancyClass), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #endregion






        #region Appointments
        #region ScheduleAppointments
        [HttpGet("ScheduleAppointments")]
        public async Task<IActionResult> ScheduleAppointments([FromQuery] ScheduleAppointmentDTOs scheduleAppointmentDTOs)
        {
            var query = new ScheduleAppointmentQuery(scheduleAppointmentDTOs);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #region AddAppointment
        [HttpPost("AddAppointment")]

        public async Task<IActionResult> AddAppointment([FromBody] AddAppointmentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddAppointment), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion
        #region FilterAppointments
        [HttpGet("FilterAppointments")]
        public async Task<IActionResult> FilterAppointments([FromQuery] FilterAppointmentDTOs filterAppointmentDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterAppointmentQuery(paginationRequest, filterAppointmentDTOs);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion

        #region Counselor
        #region AllCounselor
        [HttpGet("AllCounselor")]
        public async Task<IActionResult> AllCounselor([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CounselorQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #region FilterCounselor
        [HttpGet("FilterCounselor")]
        public async Task<IActionResult> FilterCounselor([FromQuery] FilterCounselorDTOs filterCounselorDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCounselorQuery(paginationRequest, filterCounselorDTOs);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddCounselor
        [HttpPost("AddCounselor")]

        public async Task<IActionResult> AddCounselor([FromBody] AddCounselorRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddCounselor), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #endregion


      

        #region ApplicantsById
        [HttpGet("Applicants/{id}")]
        public async Task<IActionResult> Applicants([FromRoute] string id)
        {
            var query = new ApplicantsByIdQuery(id);
            var applicantsResult = await _mediator.Send(query);
            #region Switch Statement
            return applicantsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(applicantsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = applicantsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(applicantsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region CRMStudentsById
        [HttpGet("CRMStudents/{id}")]
        public async Task<IActionResult> CRMStudents([FromRoute] string id)
        {
            var query = new CRMStudentsByIdQuery(id);
            var crmStudentsResult = await _mediator.Send(query);
            #region Switch Statement
            return crmStudentsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(crmStudentsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = crmStudentsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(crmStudentsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region InquiryById
        [HttpGet("Inquiry/{id}")]
        public async Task<IActionResult> Inquiry([FromRoute] string id)
        {
            var query = new InqueryByIdQuery(id);
            var inquiryResult = await _mediator.Send(query);
            #region Switch Statement
            return inquiryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inquiryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inquiryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inquiryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AllInquiry
        [HttpGet("AllInquiry")]
        public async Task<IActionResult> AllInquiry([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new InquiryQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #region AllApplicant
        [HttpGet("AllApplicant")]
        public async Task<IActionResult> AlAllApplicantlInquiry([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ApplicantQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #region UserProfileById
        [HttpGet("UserProfile/{id}")]
        public async Task<IActionResult> UserProfile([FromRoute] string id)
        {
            var query = new GetUserProfileByIdQuery(id);
            var userProfileByUserResult = await _mediator.Send(query);
            #region Switch Statement
            return userProfileByUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(userProfileByUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = userProfileByUserResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(userProfileByUserResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterCRMStudents
        [HttpGet("FilterCRMStudents")]
        public async Task<IActionResult> FilterCRMStudents([FromQuery] FilterCRMStudentsDTOs filterCRMStudentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCRMStudentsQuery(paginationRequest, filterCRMStudentsDTOs);
            var filterCRMStudents = await _mediator.Send(query);
            #region Switch Statement
            return filterCRMStudents switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterCRMStudents.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterCRMStudents.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterCRMStudents.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterApplicants
        [HttpGet("FilterApplicants")]
        public async Task<IActionResult> GetFilterApplicants([FromQuery] FilterApplicantDTOs filterApplicantDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterApplicantQuery(paginationRequest, filterApplicantDTOs);
            var filterApplicants = await _mediator.Send(query);
            #region Switch Statement
            return filterApplicants switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterApplicants.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterApplicants.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterApplicants.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterInquery
        [HttpGet("FilterInquery")]
        public async Task<IActionResult> GetFilterInquery([FromQuery] FilterInquiryDTOs filterInquiryDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterInquiryQuery(paginationRequest, filterInquiryDTOs);
            var filterInquery = await _mediator.Send(query);
            #region Switch Statement
            return filterInquery switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterInquery.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterInquery.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterInquery.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddInquiry
        [HttpPost("AddInquiry")]

        public async Task<IActionResult> AddInquiry([FromBody] AddInquiryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addInquiry = await _mediator.Send(command);
            #region Switch Statement
            return addInquiry switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddInquiry), addInquiry.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addInquiry.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addInquiry.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region ConvertToApplicant
        [HttpPost("ConvertToApplicant")]

        public async Task<IActionResult> ConvertToApplicant([FromBody] ConvertApplicantRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var convert = await _mediator.Send(command);
            #region Switch Statement
            return convert switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(ConvertToApplicant), convert.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = convert.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(convert.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region ConvertToStudents
        [HttpPost("ConvertToStudents")]

        public async Task<IActionResult> ConvertToStudents([FromBody] ConvertStudentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var convert = await _mediator.Send(command);
            #region Switch Statement
            return convert switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(ConvertToStudents), convert.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = convert.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(convert.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region UserProfile
        #region GetAllUserProfile
        [HttpGet("GetAllUserProfile")]
        public async Task<IActionResult> UserProfile([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllUserProfileQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #endregion
    }
}
