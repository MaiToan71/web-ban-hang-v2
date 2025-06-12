namespace Project.Data.Entities
{
    public class Image : BaseViewModel
    {
        public string ImagePath { get; set; }

        public string Caption { get; set; }

        public bool IsDefault { get; set; } = false;
        public bool IsBanner { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public int SortOrder { get; set; } = 1;

        public long FileSize { get; set; } = 0;

        public List<PostTypeImage> PostTypeImages { get; set; }

        public List<PostImage> PostImages { get; set; }

        public List<ProductImage> ProductImages { get; set; }

    }
}
