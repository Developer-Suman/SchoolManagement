using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.DeleteExam;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.MarkSheetByStudent;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate.RequestCommandMapper;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate.RequestCommandMapper;
using ES.Certificate.Application.Certificates.Command.Awards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.AddAwards.RequestCommandMapper;
using ES.Certificate.Application.Certificates.Command.Awards.DeleteAwards;
using ES.Certificate.Application.Certificates.Command.DeleteCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.DeleteIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate.RequestCommandMapper;
using ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate.RequestCommandMapper;
using ES.Certificate.Application.Certificates.Queries.Awards;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.GenerateCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificateById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Authentication.v1;

namespace TN.Web.Controllers.Certificate.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class CertificateController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CertificateController> _logger;
        public CertificateController(IMediator mediator, ILogger<CertificateController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region Awards

        #region AddAwards
        [HttpPost("AddAwards")]

        public async Task<IActionResult> AddAwards([FromBody] AddAwardsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addAwardsResult = await _mediator.Send(command);
            #region Switch Statement
            return addAwardsResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddAwards), addAwardsResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addAwardsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addAwardsResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region GetAllAwards
        [HttpGet("GetAllAwards")]
        public async Task<IActionResult> GetAllAwards([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new AwardsQuery(paginationRequest);
            var allAwards = await _mediator.Send(query);
            #region Switch Statement
            return allAwards switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allAwards.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allAwards.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allAwards.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AwardsById
        [HttpGet("Awards/{AwardsById}")]
        public async Task<IActionResult> AwardsById([FromRoute] string awardsById)
        {
            var query = new IssuedCertificateByIdQuery(awardsById);
            var awardsByIdResult = await _mediator.Send(query);
            #region Switch Statement
            return awardsByIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(awardsByIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = awardsByIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(awardsByIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteAwards
        [HttpDelete("DeleteAwards/{id}")]

        public async Task<IActionResult> DeleteAwards([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteAwardsCommand(id);
            var deleteAwardsResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteAwardsResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteAwardsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteAwardsResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Awards")
            };

            #endregion
        }

        #endregion

        #endregion


        #region GeneratECertificate

        #region GenerateCertificateByStudent
        [HttpGet("GenerateCertificateByStudent")]
        public async Task<IActionResult> GenerateCertificateByStudent([FromQuery] MarksSheetDTOs marksSheetDTOs)
        {
            var query = new GenerateCertificateQuery(marksSheetDTOs);
            var generateCertificate = await _mediator.Send(query);
            #region Switch Statement
            return generateCertificate switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(generateCertificate.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = generateCertificate.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(generateCertificate.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #endregion

        #region CertificateTemplate
        #region AddCertificateTemplate
        [HttpPost("AddCertificateTemplate")]

        public async Task<IActionResult> AddCertificateTemplate([FromBody] AddCertificateTemplateRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addCertificateTemplateResult = await _mediator.Send(command);
            #region Switch Statement
            return addCertificateTemplateResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddCertificateTemplate), addCertificateTemplateResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addCertificateTemplateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addCertificateTemplateResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region AllCertificateTemplate
        [HttpGet("all-certificateTemplate")]
        public async Task<IActionResult> GetAllCertificateTemplate([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CertificateTemplateQuery(paginationRequest);
            var allCertificateTemplate = await _mediator.Send(query);
            #region Switch Statement
            return allCertificateTemplate switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allCertificateTemplate.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allCertificateTemplate.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allCertificateTemplate.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterCertificateTemplate
        [HttpGet("FilterCertificateTemplate")]
        public async Task<IActionResult> GetFilterCertificateTemplate([FromQuery] FilterCertificateTemplatesDTOs filterCertificateTemplatesDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCertificateTemplateQuery(paginationRequest, filterCertificateTemplatesDTOs);
            var filterCertificateTemplateResult = await _mediator.Send(query);
            #region Switch Statement
            return filterCertificateTemplateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterCertificateTemplateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterCertificateTemplateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterCertificateTemplateResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteExam
        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> DeleteCertificateTemplate([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteCertificateTemplateCommand(id);
            var deleteCertificateTemplateResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteCertificateTemplateResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteCertificateTemplateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteCertificateTemplateResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Certificate Template")
            };

            #endregion
        }
        #endregion

        #region CertificateTemplateById
        [HttpGet("CertificateTemplate/{certificateTemplateId}")]
        public async Task<IActionResult> GetCertificateTemplateById([FromRoute] string certificateTemplateId)
        {
            var query = new IssuedCertificateByIdQuery(certificateTemplateId);
            var certificateTemplateByIdResult = await _mediator.Send(query);
            #region Switch Statement
            return certificateTemplateByIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(certificateTemplateByIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = certificateTemplateByIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(certificateTemplateByIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateCertificateTemplate
        [HttpPatch("UpdateCertificateTemplate/{Id}")]

        public async Task<IActionResult> UpdateCertificateTemplate([FromRoute] string Id, [FromBody] UpdateCertificateTemplateRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateCertificateTemplateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateCertificateTemplateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateCertificateTemplateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateCertificateTemplateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateCertificateTemplateResult.Errors),
                _ => BadRequest("Invalid Fields for Update Certificate Template")
            };

            #endregion
        }
        #endregion

        #endregion

        #region IssuedCertificate
        #region AddIssuedCertificate
        [HttpPost("AddIssuedCertificate")]

        public async Task<IActionResult> AddIssuedCertificate([FromBody] AddIssuedCertificateRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addIssuedCertificate = await _mediator.Send(command);
            #region Switch Statement
            return addIssuedCertificate switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddCertificateTemplate), addIssuedCertificate.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addIssuedCertificate.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addIssuedCertificate.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region AllIssuedCertificate
        [HttpGet("all-issuedCertificate")]
        public async Task<IActionResult> GetAllIssuedCertificate([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new IssuedCertificateQuery(paginationRequest);
            var allIssuedCertificateResult = await _mediator.Send(query);
            #region Switch Statement
            return allIssuedCertificateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allIssuedCertificateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allIssuedCertificateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allIssuedCertificateResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterIssuedCertificate
        [HttpGet("FilterIssuedCertificate")]
        public async Task<IActionResult> GetFilterIssuedCertificate([FromQuery] FilterIssuedCertificateDTOs filterIssuedCertificateDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterIssuedCertificateQuery(paginationRequest, filterIssuedCertificateDTOs);
            var filterIssuedCertificateResult = await _mediator.Send(query);
            #region Switch Statement
            return filterIssuedCertificateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterIssuedCertificateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterIssuedCertificateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterIssuedCertificateResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteExam
        [HttpDelete("DeleteIssuedCertificate/{id}")]

        public async Task<IActionResult> DeleteIssuedCertificate([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteIssuedCertificateCommand(id);
            var deleteIssuedCertificateResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteIssuedCertificateResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteIssuedCertificateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteIssuedCertificateResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Issued Certificate")
            };

            #endregion
        }
        #endregion

        #region IssuedCertificateById
        [HttpGet("IssuedCertificateById/{issuedCertificateId}")]
        public async Task<IActionResult> IssuedCertificateById([FromRoute] string issuedCertificateId)
        {
            var query = new IssuedCertificateByIdQuery(issuedCertificateId);
            var issuedCertificateByIdResult = await _mediator.Send(query);
            #region Switch Statement
            return issuedCertificateByIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(issuedCertificateByIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = issuedCertificateByIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(issuedCertificateByIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateIssuedCertificate
        [HttpPatch("UpdateIssuedCertificate/{Id}")]

        public async Task<IActionResult> UpdateIssuedCertificate([FromRoute] string Id, [FromBody] UpdateIssuedCertificateRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateIssuedCertificateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateIssuedCertificateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateIssuedCertificateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateIssuedCertificateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateIssuedCertificateResult.Errors),
                _ => BadRequest("Invalid Fields for Update Issued Certificate")
            };

            #endregion
        }
        #endregion

        #endregion
    }
}
