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

            #endregion Employee

            #region User

            //User
            CreateMap<User, UserViewModel>()
                // CuttingPlanNo
                .ForMember(x => x.NameThai,
                           o => o.MapFrom(s => s.EmpCodeNavigation == null ? "-" : $"คุณ{s.EmpCodeNavigation.NameThai}"))
                .ForMember(x => x.EmpCodeNavigation, o => o.Ignore());

            #endregion User

            #region ColorItem

            //Color
            CreateMap<ColorItem, ColorItemViewModel>()
                // Color Percent
                .ForMember(x => x.VolumeSolidsString,
                            o => o.MapFrom(s => s.VolumeSolids == null ? "0.0%" : $"{s.VolumeSolids.Value.ToString("00.0")}%"));

            #endregion ColorItem

            #region StandardTime

            // StandardTime
            CreateMap<StandradTime, StandardTimeViewModel>()
                // TypeStandardTimeString
                .ForMember(x => x.TypeStandardTimeString,
                            o => o.MapFrom(s => s.TypeStandardTime == null ? "-" : $"{(s.TypeStandardTime == TypeStandardTime.Paint ? "Paint" : "Blast")}")   )
                // PercentLossString
                .ForMember(x => x.PercentLossString,
                            o => o.MapFrom(s => s.PercentLoss == null ? "0.0%" : $"{s.PercentLoss.Value.ToString("00.0")}%"))
                // StandardTime
                .ForMember(x => x.RateWithUnit,
                            o => o.MapFrom(s => s.Rate == null || s.RateUnit == null ? "-" : $"{s.Rate.Value.ToString("0.00")} {s.RateUnit}"));

            #endregion StandardTime

            #region ProjectCodeMaster

            CreateMap<ProjectCodeMaster, ProjectMasterViewModel>();
            CreateMap<ProjectMasterViewModel, ProjectCodeMaster>();

            #endregion
        }
    }
}