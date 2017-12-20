﻿using AutoMapper;
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
                            o => o.MapFrom(s => s.TypeStandardTime == null ? "-" : $"{(s.TypeStandardTime == TypeStandardTime.Paint ? "Paint" : "Blast")}"))
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

            #endregion ProjectCodeMaster

            #region RequirePaintingMaster

            CreateMap<RequirePaintingMaster, RequirePaintingMasterViewModel>()
                .ForMember(x => x.JobCode,
                            o => o.MapFrom(s => s.ProjectCodeSub == null ? "-" : $"{s.ProjectCodeSub.Code}"))
                // .ForMember(x => x.ProjectCodeSub, o => o.Ignore())
                .ForMember(x => x.RequirePaintingLists, o => o.Ignore());

            #endregion RequirePaintingMaster

            #region BlastWorkItem

            CreateMap<BlastWorkItem, BlastWorkItemViewModel>()
                .ForMember(x => x.ExtStandradTimeString,
                           o => o.MapFrom(s => s.StandradTimeExt == null ? "" : $"{ s.StandradTimeExt.Description }"))
                .ForMember(x => x.IntStandradTimeString,
                           o => o.MapFrom(s => s.StandradTimeInt == null ? "" : $"{ s.StandradTimeInt.Description }"))
                .ForMember(x => x.ExtSurfaceTypeString,
                           o => o.MapFrom(s => s.SurfaceTypeExt == null ? "" : $"{ s.SurfaceTypeExt.SurfaceName }"))
                .ForMember(x => x.IntSurfaceTypeString,
                           o => o.MapFrom(s => s.SurfaceTypeInt == null ? "" : $"{ s.SurfaceTypeInt.SurfaceName }"))
                .ForMember(x => x.StandradTimeExt, o => o.Ignore())
                .ForMember(x => x.StandradTimeInt, o => o.Ignore())
                .ForMember(x => x.SurfaceTypeExt, o => o.Ignore())
                .ForMember(x => x.SurfaceTypeInt, o => o.Ignore());

            CreateMap<BlastWorkItemViewModel, BlastWorkItem>();

            #endregion BlastWorkItem

            #region PaintWorkItem

            CreateMap<PaintWorkItem, PaintWorkItemViewModel>()
                .ForMember(x => x.PaintLevelString,
                            o => o.MapFrom(s => s.PaintLevel == null ? "-" :
                            (s.PaintLevel == PaintLevel.PrimerCoat ? "PrimerCoat" :
                            (s.PaintLevel == PaintLevel.MidCoat ? "MidCoat" :
                            (s.PaintLevel == PaintLevel.IntermediateCoat ? "IntermediateCoat" : "TopCoat")))))
                .ForMember(x => x.ExtStandradTimeString,
                            o => o.MapFrom(s => s.StandradTimeExt == null ? null : $"{ s.StandradTimeExt.Description }"))
                .ForMember(x => x.IntStandradTimeString,
                            o => o.MapFrom(s => s.StandradTimeInt == null ? null : $"{ s.StandradTimeInt.Description }"))
                .ForMember(x => x.ExtColorString,
                            o => o.MapFrom(s => s.ExtColorItem == null ? null : $"{ s.ExtColorItem.ColorName }"))
                .ForMember(x => x.IntColorString,
                            o => o.MapFrom(s => s.IntColorItem == null ? null : $"{ s.IntColorItem.ColorName }"))
                .ForMember(x => x.StandradTimeExt, o => o.Ignore())
                .ForMember(x => x.StandradTimeInt, o => o.Ignore())
                .ForMember(x => x.ExtColorItem, o => o.Ignore())
                .ForMember(x => x.IntColorItem, o => o.Ignore());

            CreateMap<PaintWorkItemViewModel, PaintWorkItem>();

            #endregion PaintWorkItem
        }
    }
}