using AutoMapper;
using VipcoPainting.Models;
using VipcoPainting.ViewModels;

namespace VipcoPainting.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Employee

            // Employee
            CreateMap<Employee, EmployeeViewModel>()
                // TypeMachine
                .ForMember(x => x.TypeEmployeeString,
                           o => o.MapFrom(s => s.TypeEmployee == null ? "ไม่ระบุ" :
                           (s.TypeEmployee.Value == 1 ? "พนักงานตามโครงการ" :
                           (s.TypeEmployee.Value == 2 ? "พนักงานทดลองงาน" :
                           (s.TypeEmployee.Value == 3 ? "พนักงานประจำรายชั่วโมง" :
                           (s.TypeEmployee.Value == 4 ? "พนักงานประจำรายเดือน" : "พนักงานพม่า"))))));
            CreateMap<EmployeeViewModel, Employee>();

            #endregion

            #region User
            //User
            CreateMap<User, UserViewModel>()
                // CuttingPlanNo
                .ForMember(x => x.NameThai,
                           o => o.MapFrom(s => s.EmpCodeNavigation == null ? "-" : $"คุณ{s.EmpCodeNavigation.NameThai}"))
                .ForMember(x => x.EmpCodeNavigation, o => o.Ignore());

            #endregion
        }
    }
}