namespace TN.Reports.Application.SchoolReports.PaymentDetailsReport
{
    public record PaymentDetailsReportResponse
    (
        string? studentName="",
        decimal? totalAmount=0,
        decimal? paidAmount=0,
        decimal? discountAmount=0,
        decimal? dueAmount=0
        );
}