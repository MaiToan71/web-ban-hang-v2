using Project.Enums;

namespace Project.ViewModel.Usermanagers.Permissions
{
    public class PermissionCreateRequest
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public int ParentId { get; set; }
        public string Route { get; set; }
        public string Component { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
        public bool Hide { get; set; }
        public string NormalizedName { get; set; }
        public PermissionEnum PermissionType { get; set; }
    }
}
