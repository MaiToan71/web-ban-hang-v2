using AutoMapper;
using Project.Data.Entities;
using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModels.Attributes;
using Project.ViewModels.BankConfig;
using Project.ViewModels.BankUsers;
using Project.ViewModels.Departments;
using Project.ViewModels.Notification;
using Project.ViewModels.Orders;
using Project.ViewModels.Schedule;
using Project.ViewModels.TimeShifts;
using Project.ViewModels.TimeShiftTypes;

namespace Project.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TimeShift, TimeShiftCreateRequest>().ReverseMap();
            CreateMap<TimeShift, TimeShiftUpdateRequest>().ReverseMap();

            CreateMap<TimeShiftType, TimeShiftTypeCreateRequest>().ReverseMap();
            CreateMap<TimeShiftType, TimeShiftTypeUpdateRequest>().ReverseMap();


            CreateMap<Permission, PermissionCreateRequest>().ReverseMap();
            CreateMap<Permission, PermissionUpdateRequest>().ReverseMap();

            CreateMap<Notification, NotificationCreateRequest>().ReverseMap();
            CreateMap<Notification, NotificationUpdateRequest>().ReverseMap();

            CreateMap<BankConfig, BankCreateRequest>().ReverseMap();
            CreateMap<BankConfig, BankUpdateRequest>().ReverseMap();

            CreateMap<BankUser, BankUserCreateRequest>().ReverseMap();
            CreateMap<BankUser, BankUserUpdateRequest>().ReverseMap();

            CreateMap<Department, DepartmentCreateRequest>().ReverseMap();
            CreateMap<Department, DepartmentUpdateRequest>().ReverseMap();


            CreateMap<Order, OrderCreateRequest>().ReverseMap();
            CreateMap<Order, OrderUpdateRequest>().ReverseMap();


            CreateMap<OrderDetail, OrderDetailRequest>().ReverseMap();


            CreateMap<UpdateCalendarEmployeeDTO, WorkRecord>().ReverseMap();
            CreateMap<UpdateCalendarEmployeeAdminDTO, WorkRecord>().ReverseMap();

            CreateMap<AttributeCreateRequest, Project.Data.Entities.Attribute>().ReverseMap();
            CreateMap<AttributeUpdateRequest, Project.Data.Entities.Attribute>().ReverseMap();
        }
    }
}
