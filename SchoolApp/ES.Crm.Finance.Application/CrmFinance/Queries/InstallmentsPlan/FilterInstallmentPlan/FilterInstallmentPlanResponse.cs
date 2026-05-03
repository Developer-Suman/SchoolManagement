namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan
{
    public record FilterInstallmentPlanResponse
    (
        string id="",
            string invoiceId="",
            int numberOfInstallments=0,
            decimal totalAmount=0,
            bool isActive=false,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
