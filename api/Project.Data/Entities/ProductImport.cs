using Project.Enums;

namespace Project.Data.Entities
{
    public class ProductImport : BaseViewModel
    {
        public string Name { get; set; }

        public Guid UserInChangeId { get; set; }

        public DateTime InputDate { get; set; }
        public float Total { get; set; }

        public int PostTypeId { get; set; }

        public string Description { get; set; }

        public Workflow Workflow { get; set; }
    }
}
