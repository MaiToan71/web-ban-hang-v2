using Project.ViewModel.PostTypes;

namespace Project.ViewModels.Images
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public string Caption { get; set; }

        public string User { get; set; }

        public bool IsDefault { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public Guid CreatedId { get; set; }
        public string CreatedUser { get; set; }
        public List<PostTypeViewModel> Categories { get; set; } = new List<PostTypeViewModel>();
    }
}
