using ES.Enrolment.Application.Enrolments.Command.AddConsultancyClass.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.AddFollowUp.RequestCommand_Mapper;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.Appointment.DeleteAppointment;
using ES.Enrolment.Application.Enrolments.Command.Appointment.UpdateAppointment;
using ES.Enrolment.Application.Enrolments.Command.Appointment.UpdateAppointment.RequestCommandMapper;
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
using ES.Enrolment.Application.Enrolments.Command.FollowUp.DeleteFollowUp;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.Applicant;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.ApplicantsById;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.AppointmentsId;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.ScheduleAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.ShowLeadEnquiry;
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
using ES.Enrolment.Application.Enrolments.Queries.FollowUp.FollowUpId;
using ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetAllUserProfile;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetUserProfileById;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Visa.Application.Visa.Command.VisaApplication.DeleteVisaApplication;
using ES.Visa.Application.Visa.Command.VisaStatus.UpdateVisaStatus;
using ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus;
using ES.Visa.Application.Visa.Queries.VisaStatus.VisaStatus;
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddFollowUp),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region UpdateFollowUp
        [HttpPatch("UpdateFollowUp/{Id}")]

        public async Task<IActionResult> UpdateFollowUp(string Id, [FromBody] UpdateFollowUpRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new ObjectResult(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                })
                {
                    StatusCode = StatusCodes.Status200OK
                },
                { IsSuccess: true, Data: null, Message: not null } => Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion
        }
        #endregion

        #region DeleteFollowUp
        [HttpDelete("DeleteFollowUp/{id}")]

        public async Task<IActionResult> DeleteFollowUp([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteFollowUpCommand(id);
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {

                { IsSuccess: true } => Ok(new
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = result.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion
        }

        #endregion

        #region FollowUpById
        [HttpGet("FollowUpById/{followUpId}")]
        public async Task<IActionResult> FollowUpById([FromRoute] string followUpId)
        {
            var query = new FollowUpIdQuery(followUpId);
            var queryResult = await _mediator.Send(query);
            #region Switch Statement
            return queryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(queryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = queryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(queryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region FilterFollowUps
        [HttpGet("FilterFollowUps")]
        public async Task<IActionResult> FilterFollowUps([FromQuery] FilterFollowUpDTOs filterFollowUpDTOs, [FromQuery] PaginationRequest paginationRequest)
        {

            var query = new FilterFollowUpQuery(paginationRequest, filterFollowUpDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddTrainingRegistration),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
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
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddConsultancyClass),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #endregion


        #region Appointments
        #region ShowLeadEnqueryDetails
        [HttpGet("ShowLeadEnqueryDetails")]
        public async Task<IActionResult> ShowLeadEnqueryDetails([FromQuery] ShowLeadEnquiryDTOs showLeadEnquiryDTOs)
        {
            var query = new ShowLeadEnquiryQuery(showLeadEnquiryDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddAppointment),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region UpdateAppointments
        [HttpPatch("UpdateAppointments/{Id}")]

        public async Task<IActionResult> UpdateAppointments(string Id, [FromBody] UpdateAppointmentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new ObjectResult(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                })
                {
                    StatusCode = StatusCodes.Status200OK
                },
                { IsSuccess: true, Data: null, Message: not null } => Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion
        }
        #endregion

        #region DeleteAppointments
        [HttpDelete("DeleteAppointments/{id}")]

        public async Task<IActionResult> DeleteAppointments([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteAppointmentCommand(id);
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {

                { IsSuccess: true } => Ok(new
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = result.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion
        }

        #endregion

        #region AppointmentsById
        [HttpGet("AppointmentsById/{appointmentsId}")]
        public async Task<IActionResult> AppointmentsById([FromRoute] string appointmentsId)
        {
            var query = new AppointmentsIdQuery(appointmentsId);
            var queryResult = await _mediator.Send(query);
            #region Switch Statement
            return queryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(queryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = queryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(queryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region FilterAppointments
        [HttpGet("FilterAppointments")]
        public async Task<IActionResult> FilterAppointments([FromQuery] FilterAppointmentDTOs filterAppointmentDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterAppointmentQuery(paginationRequest, filterAppointmentDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddCounselor),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
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
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddInquiry),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(ConvertToApplicant),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
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
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(ConvertToStudents),
                new { id = addResult.Data },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
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
