namespace Project.Data.Entities
{
    public class BankConfig : BaseViewModel
    {
        public string? BankId { get; set; } // mã bin nghân hàng
        public string? BankName { get; set; } // Tên ngân hàng

        public string? AccountNo { get; set; } // số tài khoản thụ hưởng
        public string? Template { get; set; }// quy định ID của template. 
        public string? Amount { get; set; }//quy định số tiền chuyển khoản
        public string? Description { get; set; }//quy định nội dung chuyển khoản.
        public string? AccountName { get; set; }//quy định tên người thụ hưởng hiển thị lên file ảnh 
        public string? QrCode { get; set; } // đường dẫn ảnh
    }
}
