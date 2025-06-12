namespace Project.ViewModels.BankConfig
{
    public class GetBankPagingRequest : PagingRequestBase
    {
        public int? StoreId { get; set; }
        public int? PaybilId { get; set; }
        public Guid? UserId { get; set; }
    }

    public class GetQrCodeRequest
    {
        public int BankId { get; set; }
        public float Amount { get; set; }
        public int BillId { get; set; }
    }
}
