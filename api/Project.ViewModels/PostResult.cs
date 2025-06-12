namespace Project.ViewModels
{
    public class PostResult
    {
        public bool Status { get; set; } = true;
        public dynamic Data { get; set; } = "";

        public dynamic Error
        {
            get
            {
                return Status == false ? Data : "";
            }
        }
        public string Message
        {
            get
            {
                return Status == true ? "Thành công" : "Thất bại";
            }
        }
    }


    public class DataResult
    {
        public bool Status { get; set; } = true;
        public dynamic Data { get; set; }

        public dynamic Error
        {
            get
            {
                return Status == false ? Data : "";
            }
        }
        public string Message
        {
            get
            {
                return Status == true ? "Thành công" : "Thất bại";
            }
        }
    }
}
