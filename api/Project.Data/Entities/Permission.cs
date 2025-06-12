using Project.Enums;

namespace Project.Data.Entities
{
    public class Permission : BaseViewModel
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public int ParentId { get; set; } = 0;
        public string Route { get; set; }
        public string Component { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
        public bool Hide { get; set; }
        public string NormalizedName { get; set; }
        public PermissionEnum PermissionType { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<RolePermission> RolePermissions { get; set; }
    }
}
