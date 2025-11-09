using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Application.Authentication.Queries.GetItemStatusBySchool;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Command.CloseFiscalYear.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool;
using TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberBySchool;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberBySchool.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase.RequestCommandMapper;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales.RequestCommandMapper;
using TN.Shared.Application.Shared.Queries.FiscalYearStartDate;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYearBySchool;
using TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetInventoryMethodBySchool;
using TN.Shared.Application.Shared.Queries.GetJournalRefBySchool;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInSales;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;

namespace TN.Web.Controllers.Settings.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(IMediator mediator, ILogger<SettingsController> logger,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
            _logger = logger;

        }
        #region SelectedFiscalYear
        [HttpGet("FiscalYearStartedDate")]
        public async Task<IActionResult> SelectedFiscalYear()
        {
            var query = new FiscalYearStartDateQuery();
            var fiscalYearStartedDateResult = await _mediator.Send(query);
            #region Switch Statement
            return fiscalYearStartedDateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(fiscalYearStartedDateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = fiscalYearStartedDateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(fiscalYearStartedDateResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region SelectedFiscalYear
        [HttpGet("SelectedFiscalYear")]
        public async Task<IActionResult> SelectedFiscalYear([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetSelectedFiscalYearQuery(paginationRequest);
            var selectedfiscalYearResult = await _mediator.Send(query);
            #region Switch Statement
            return selectedfiscalYearResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(selectedfiscalYearResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = selectedfiscalYearResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(selectedfiscalYearResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region GetTaxStatusInPurchase
        [HttpGet("GetTaxStatusInPurchase/{id}")]
        public async Task<IActionResult> GetTaxStatusInPurchase([FromRoute] string id)
        {
            var query = new GetTaxStatusInPurchaseQuery(id);
            var taxStatusInPurchase = await _mediator.Send(query);
            #region Switch Statement
            return taxStatusInPurchase switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(taxStatusInPurchase.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = taxStatusInPurchase.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(taxStatusInPurchase.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region GetTaxStatusInSales
        [HttpGet("GetTaxStatusInSales/{id}")]
        public async Task<IActionResult> GetTaxStatusInSales([FromRoute] string id)
        {
            var query = new GetTaxStatusInSalesQuery(id);
            var taxStatusInSales = await _mediator.Send(query);
            #region Switch Statement
            return taxStatusInSales switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(taxStatusInSales.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = taxStatusInSales.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(taxStatusInSales.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region GetPurchaseReferenceNumberBySchoolId
        [HttpGet("GetPurchaseReferenceNumberBySchool/{id}")]
        public async Task<IActionResult> GetPurchaseReferenceNumberBySchool([FromRoute] string id)
        {
            var query = new GetPurchaseReferenceNumberQuery(id);
            var purchaseReferenceNumbers = await _mediator.Send(query);
            #region Switch Statement
            return purchaseReferenceNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(purchaseReferenceNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = purchaseReferenceNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(purchaseReferenceNumbers.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region GetSalesReferenceNumberBySchoolId
        [HttpGet("GetSalesReferenceNumberBySchool/{id}")]
        public async Task<IActionResult> GetSalesReferenceNumberByCompany([FromRoute] string id)
        {
            var query = new GetSalesReferenceNumberQuery(id);
            var salesReferenceNumbers = await _mediator.Send(query);
            #region Switch Statement
            return salesReferenceNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(salesReferenceNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = salesReferenceNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(salesReferenceNumbers.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region GetItemStatusBySchoolId
        [HttpGet("GetItemStatusBySchool/{id}")]
        public async Task<IActionResult> GetItemStatusBySchool([FromRoute] string id)
        {
            var query = new GetItemStatusBySchoolQuery(id);
            var expiredTimeItemStatus = await _mediator.Send(query);
            #region Switch Statement
            return expiredTimeItemStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(expiredTimeItemStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = expiredTimeItemStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(expiredTimeItemStatus.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region GetJournalRefBySchoolId
        [HttpGet("GetJournalRefBySchool/{id}")]
        public async Task<IActionResult> GetJournalRefBySchool([FromRoute] string id)
        {
            var query = new GetJournalRefBySchoolQuery(id);
            var journalRef = await _mediator.Send(query);
            #region Switch Statement
            return journalRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(journalRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = journalRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(journalRef.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateItemStatusBySchool
        [HttpPatch("UpdateItemStatusBySchool/{id}")]

        public async Task<IActionResult> UpdateItemStatusBySchool([FromRoute] string id, [FromBody] UpdateItemStatusBySchoolRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateExpiredDateItemStatus = await _mediator.Send(command);
            #region Switch Statement
            return updateExpiredDateItemStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateExpiredDateItemStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateExpiredDateItemStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateExpiredDateItemStatus.Errors),
                _ => BadRequest("Invalid Fields for Update ExpiredDateItem status")
            };

            #endregion


        }
        #endregion

        #region UpdateJournalRefBySchool
        [HttpPatch("UpdateJournalRefBySchool/{schoolId}")]

        public async Task<IActionResult> UpdateJournalRefBySchool([FromRoute] string schoolId, [FromBody] UpdateJournalRefBySchoolRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateJournalRef = await _mediator.Send(command);
            #region Switch Statement
            return updateJournalRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateJournalRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateJournalRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateJournalRef.Errors),
                _ => BadRequest("Invalid Fields for Update journal reference status")
            };

            #endregion


        }
        #endregion

        #region GetInventoryMethodBySchoolId
        [HttpGet("GetInventoryMethodBySchool/{schoolId}")]
        public async Task<IActionResult> GetInventoryMethodBySchool([FromRoute] string schoolId)
        {
            var query = new GetInventoryBySchoolIdQuery(schoolId);
            var inventory = await _mediator.Send(query);
            #region Switch Statement
            return inventory switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(inventory.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = inventory.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(inventory.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateInventoryMethodBySchool
        [HttpPatch("UpdateInventoryMethod/{schoolId}")]

        public async Task<IActionResult> UpdateInventoryMethod([FromRoute] string schoolId, [FromBody] UpdateInventoryMethodRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateInventory = await _mediator.Send(command);
            #region Switch Statement
            return updateInventory switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateInventory.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateInventory.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateInventory.Errors),
                _ => BadRequest("Invalid Fields for Update Inventory Method")
            };

            #endregion


        }
        #endregion

        #region GetSerialNumberForPurchase
        [HttpGet("GetSerialNumberForPurchase/{schoolId}")]
        public async Task<IActionResult> GetSerialNumberForPurchase([FromRoute] string schoolId)
        {
            var query = new GetSerialNumberForPurchaseQuery(schoolId);
            var serialNumber = await _mediator.Send(query);
            #region Switch Statement
            return serialNumber switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(serialNumber.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = serialNumber.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(serialNumber.Errors),
                _ => BadRequest("Invalid schoolId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateSalesRefBySchool
        [HttpPatch("UpdateSalesRefBySchool/{schoolId}")]

        public async Task<IActionResult> UpdateSalesRefByCompany([FromRoute] string schoolId, [FromBody] UpdateSalesReferenceNumberRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateSalesRef = await _mediator.Send(command);
            #region Switch Statement
            return updateSalesRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSalesRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSalesRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSalesRef.Errors),
                _ => BadRequest("Invalid Fields for Update sales reference status")
            };

            #endregion


        }
        #endregion

        #region UpdatePurchaseRefBySchool
        [HttpPatch("UpdatePurchaseRefBySchool/{schoolId}")]

        public async Task<IActionResult> UpdatePurchaseRefBySchool([FromRoute] string schoolId, [FromBody] UpdatePurchaseReferenceNumberRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updatePurchaseRef = await _mediator.Send(command);
            #region Switch Statement
            return updatePurchaseRef switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePurchaseRef.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePurchaseRef.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePurchaseRef.Errors),
                _ => BadRequest("Invalid Fields for Update purchase reference status")
            };

            #endregion


        }
        #endregion

        #region UpdateTaxStatusInPurchaseBySchool
        [HttpPatch("UpdateTaxStatusInPurchase/{schoolId}")]

        public async Task<IActionResult> UpdateTaxStatusInPurchase([FromRoute] string schoolId, [FromBody] UpdateTaxStatusInPurchaseRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateTaxStatus = await _mediator.Send(command);
            #region Switch Statement
            return updateTaxStatus switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateTaxStatus.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateTaxStatus.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateTaxStatus.Errors),
                _ => BadRequest("Invalid Fields for Update taxStatus in Purchase")
            };

            #endregion


        }
        #endregion

        #region UpdateTaxStatusInSalesBySchool
        [HttpPatch("UpdateTaxStatusInSales/{schoolId}")]

        public async Task<IActionResult> UpdateTaxStatusInSales([FromRoute] string schoolId, [FromBody] UpdateTaxStatusInSalesRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateTaxStatusInSales = await _mediator.Send(command);
            #region Switch Statement
            return updateTaxStatusInSales switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateTaxStatusInSales.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateTaxStatusInSales.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateTaxStatusInSales.Errors),
                _ => BadRequest("Invalid Fields for Update taxStatus in Sales")
            };

            #endregion


        }
        #endregion

        #region GetFiscalYearBySchoolId
        [HttpGet("GetCurrentFiscalYearBy/{schoolId}")]
        public async Task<IActionResult> GetCurrentFiscalYearBy([FromRoute] string schoolId)
        {
            var query = new GetCurrentFiscalYearByQuery(schoolId);
            var fiscalYearResult = await _mediator.Send(query);
            #region Switch Statement
            return fiscalYearResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(fiscalYearResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = fiscalYearResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(fiscalYearResult.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region AllFiscalYear
        [HttpGet("all-FiscalYear")]
        public async Task<IActionResult> AllFiscalYear([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllFiscalYearQuery(paginationRequest);
            var fiscalYearResult = await _mediator.Send(query);
            #region Switch Statement
            return fiscalYearResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(fiscalYearResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = fiscalYearResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(fiscalYearResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region UpdateFiscalYearBySchoolId
        [HttpPatch("UpdateCurrentFiscalYearBy/{schoolId}")]

        public async Task<IActionResult> UpdateCurrentFiscalYear([FromRoute] string schoolId, [FromBody] UpdateFiscalYearRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateFiscalYear = await _mediator.Send(command);
            #region Switch Statement
            return updateFiscalYear switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateFiscalYear.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateFiscalYear.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateFiscalYear.Errors),
                _ => BadRequest("Invalid Fields for Update fisclay year in Sales")
            };

            #endregion


        }
        #endregion


        #region ClosedFiscalYear
        [HttpPost("ClosedFiscalYear")]

        public async Task<IActionResult> ClosedFiscalYear( [FromBody] CloseFiscalYearRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addClosedFiscalYear = await _mediator.Send(command);
            #region Switch Statement
            return addClosedFiscalYear switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(addClosedFiscalYear.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addClosedFiscalYear.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addClosedFiscalYear.Errors),
                _ => BadRequest("Invalid Fields for add closed fiscal year")
            };

            #endregion


        }
        #endregion

        #region GetReceiptTransactionNumberType
        [HttpGet("GetReceiptTransactionNumberType/{schoolId}")]
        public async Task<IActionResult> GetReceiptTransactionNumberType([FromRoute] string schoolId)
        {
            var query = new ReceiptTransactionNumberTypeQuery(schoolId);
            var receiptTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return receiptTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(receiptTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = receiptTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(receiptTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetPaymentsTransactionNumberType
        [HttpGet("GetPaymentsTransactionNumberType/{schoolId}")]
        public async Task<IActionResult> GetPaymentsTransactionNumberType([FromRoute] string schoolId)
        {
            var query = new GetPaymentTransactionNumberTypeQuery(schoolId);
            var paymentsTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return paymentsTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(paymentsTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = paymentsTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(paymentsTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateReceiptTransactionType
        [HttpPatch("UpdateReceiptTransactionType/{schoolId}")]

        public async Task<IActionResult> UpdateReceiptTransactionType([FromRoute] string schoolId, [FromBody] UpdateReceiptTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateReceiptTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updateReceiptTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateReceiptTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateReceiptTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateReceiptTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Receipt Transaction Status")
            };

            #endregion


        }
        #endregion


        #region UpdatePaymentTransactionType
        [HttpPatch("UpdatePaymentTransactionType/{schoolId}")]

        public async Task<IActionResult> UpdatePaymentTransactionType([FromRoute] string schoolId, [FromBody] UpdatePaymentTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updatePaymentTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updatePaymentTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updatePaymentTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updatePaymentTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updatePaymentTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Payment Transaction status")
            };

            #endregion


        }
        #endregion

        #region GetIncomeTransactionNumberType
        [HttpGet("GetIncomeTransactionNumberType/{schoolId}")]
        public async Task<IActionResult> GetIncomeTransactionNumberType([FromRoute] string schoolId)
        {
            var query = new GetIncomeTransactionNumberTypeQuery(schoolId);
            var incomeTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return incomeTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(incomeTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = incomeTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(incomeTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetExpenseTransactionNumberType
        [HttpGet("GetExpenseTransactionNumberType/{schoolId}")]
        public async Task<IActionResult> GetExpenseTransactionNumberType([FromRoute] string schoolId)
        {
            var query = new GetExpenseTransactionNumberTypeQuery(schoolId);
            var expenseTransactionNumberType = await _mediator.Send(query);
            #region Switch Statement
            return expenseTransactionNumberType switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(expenseTransactionNumberType.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = expenseTransactionNumberType.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(expenseTransactionNumberType.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateExpenseTransactionType
        [HttpPatch("UpdateExpenseTransactionType/{schoolId}")]

        public async Task<IActionResult> UpdateExpenseTransactionType([FromRoute] string schoolId, [FromBody] UpdateExpenseTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateExpenseTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updateExpenseTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateExpenseTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateExpenseTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateExpenseTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Expense Transaction Status")
            };

            #endregion


        }
        #endregion

        #region UpdateIncomeTransactionType
        [HttpPatch("UpdateIncomeTransactionType/{schoolId}")]

        public async Task<IActionResult> UpdateIncomeTransactionType([FromRoute] string schoolId, [FromBody] UpdateIncomeTransactionNumberTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateIncomeTransaction = await _mediator.Send(command);
            #region Switch Statement
            return updateIncomeTransaction switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateIncomeTransaction.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateIncomeTransaction.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateIncomeTransaction.Errors),
                _ => BadRequest("Invalid Fields for Update Income Transaction Status")
            };

            #endregion


        }
        #endregion

        #region GetPurchaseReturnNumberBySchoolId
        [HttpGet("GetPurchaseReturnNumber/{schoolId}")]
        public async Task<IActionResult> GetPurchaseReturnNumber([FromRoute] string schoolId)
        {
            var query = new GetPurchaseReturnNumberQuery(schoolId);
            var returnNumbers = await _mediator.Send(query);
            #region Switch Statement
            return returnNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(returnNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = returnNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(returnNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetSalesReturnNumberBySchoolId
        [HttpGet("GetSalesReturnNumber/{schoolId}")]
        public async Task<IActionResult> GetSalesReturnNumber([FromRoute] string schoolId)
        {
            var query = new GetSalesReturnNumberQuery(schoolId);
            var returnNumbers = await _mediator.Send(query);
            #region Switch Statement
            return returnNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(returnNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = returnNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(returnNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdatePurchaseReturnTypeByCompany
        [HttpPatch("UpdatePurchaseReturnType/{schoolId}")]

        public async Task<IActionResult> UpdatePurchaseReturnType([FromRoute] string schoolId, [FromBody] UpdatePurchaseReturnTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateResult.Errors),
                _ => BadRequest("Invalid Fields for Update purchase Return Number")
            };

            #endregion


        }
        #endregion

        #region UpdateSalesReturnTypeByCompany
        [HttpPatch("UpdateSalesReturnType/{schoolId}")]

        public async Task<IActionResult> UpdateSalesReturnType([FromRoute] string schoolId, [FromBody] UpdateSalesReturnTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateResult.Errors),
                _ => BadRequest("Invalid Fields for Update sales  return number")
            };

            #endregion


        }
        #endregion

        #region GetPurchaseQuotationNumberBySchoolId
        [HttpGet("GetPurchaseQuotationNumber/{schoolId}")]
        public async Task<IActionResult> GetPurchaseQuotationNumber([FromRoute] string schoolId)
        {
            var query = new GetPurchaseQuotationNumberQuery(schoolId);
            var quotationNumbers = await _mediator.Send(query);
            #region Switch Statement
            return quotationNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(quotationNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = quotationNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(quotationNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region GetSalesQuotationNumberBySchoolId
        [HttpGet("GetSalesQuotationNumber/{schoolId}")]
        public async Task<IActionResult> GetSalesQuotationNumber([FromRoute] string schoolId)
        {
            var query = new GetSalesQuotationTypeQuery(schoolId);
            var quotationNumbers = await _mediator.Send(query);
            #region Switch Statement
            return quotationNumbers switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(quotationNumbers.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = quotationNumbers.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(quotationNumbers.Errors),
                _ => BadRequest("Invalid companyId Fields")
            };
            #endregion

        }
        #endregion

        #region UpdatePurchaseQuotationTypeBySchool
        [HttpPatch("UpdatePurchaseQuotationType/{schoolId}")]

        public async Task<IActionResult> UpdatePurchaseQuotationType([FromRoute] string schoolId, [FromBody] UpdatePurchaseQuotationTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateResult.Errors),
                _ => BadRequest("Invalid Fields for Update purchase Quotation Number")
            };

            #endregion


        }
        #endregion

        #region UpdateSalesQuotationTypeBySchool
        [HttpPatch("UpdateSalesQuotationType/{schoolId}")]

        public async Task<IActionResult> UpdateSalesQuotationType([FromRoute] string schoolId, [FromBody] UpdateSalesQuotationTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(schoolId);
            var updateResult = await _mediator.Send(command);
            #region Switch Statement
            return updateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateResult.Errors),
                _ => BadRequest("Invalid Fields for Update sales Quotation Number")
            };

            #endregion


        }
        #endregion

    }
}
