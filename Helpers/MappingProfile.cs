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
                            o => o.MapFrom(s => s.TypeStandardTime == null ? "-" : $"{(s.TypeStandardTime == TypeStandardTime.Paint ? "Paint" : "Blast")}"))
                // Condition
                .ForMember(x => x.ConditionString,
                            o => o.MapFrom(s => s.Codition == null ? "-" :
                             s.Codition == Codition.Equal ? "Equal " :
                            (s.Codition == Codition.Less ? "Less " :
                            (s.Codition == Codition.Over ? "Over " : "None"))))
                // Condition Area
                .ForMember(x => x.AreaWithUnitNameString,
                            o => o.MapFrom(s => s.AreaCodition == null ? "0 m²" : $"{s.AreaCodition} m²"))
                // PercentLossString
                .ForMember(x => x.PercentLossString,
                            o => o.MapFrom(s => s.PercentLoss == null ? "0.0%" : $"{s.PercentLoss.Value.ToString("00.0")}%"))
                // LinkStandardTime
                .ForMember(x => x.LinkStandardTimeString,
                            o => o.MapFrom(s => s.LinkStandradTime == null ? "" : $"{s.LinkStandradTime.Code} | {s.LinkStandradTime.Description}"))
                .ForMember(x => x.LinkStandradTime,o => o.Ignore())
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
                .ForMember(x => x.IntAreaString,
                            o => o.MapFrom(s => s.IntArea == null ? "" : $"Area:{s.IntArea} DFT:{s.IntDFTMin}/{s.IntDFTMax}"))
                .ForMember(x => x.ExtAreaString,
                            o => o.MapFrom(s => s.ExtArea == null ? "" : $"Area:{s.ExtArea} DFT:{s.ExtDFTMin}/{s.ExtDFTMax}"))
                .ForMember(x => x.StandradTimeExt, o => o.Ignore())
                .ForMember(x => x.StandradTimeInt, o => o.Ignore())
                .ForMember(x => x.ExtColorItem, o => o.Ignore())
                .ForMember(x => x.IntColorItem, o => o.Ignore());

            CreateMap<PaintWorkItemViewModel, PaintWorkItem>();

            #endregion PaintWorkItem

            #region BlastRoom

            CreateMap<BlastRoom, BlastRoomViewModel>()
                .ForMember(x => x.TeamBlastString,
                          o => o.MapFrom(s => s.PaintTeam == null ? "-" : $"{s.PaintTeam.TeamName}"))
                .ForMember(x => x.PaintTeam, o => o.Ignore());

            CreateMap<BlastRoomViewModel, BlastRoom>();
            #endregion
           
            #region PaintTaskMaster
            CreateMap<PaintTaskMaster, PaintTaskMasterViewModel>();
            #endregion

            #region PaintTaskDetail
            CreateMap<PaintTaskDetail, PaintTaskDetailViewModel>()
                .ForMember(x => x.BlastRoomString,
                            o => o.MapFrom(s => s.BlastRoom == null ? "-" : $"{s.BlastRoom.BlastRoomName}/{(s.BlastRoom.PaintTeam.TeamName ?? "-")}"))
                .ForMember(x => x.BlastRoom, o => o.Ignore())
                .ForMember(x => x.PaintTeamString,
                            o => o.MapFrom(s => s.PaintTeam == null ? "-" : $"{s.PaintTeam.TeamName}"))
                .ForMember(x => x.PaintTeam, o => o.Ignore());
            #endregion

            #region ColorMovementStock

            CreateMap<ColorMovementStock, ColorMovementStockViewModel>()
                .ForMember(x => x.StatusNameString,
                            o => o.MapFrom(s => s.MovementStockStatus == null ? "-" : $"{s.MovementStockStatus.StatusName}"))
                .ForMember(x => x.ColorNameString,
                            o => o.MapFrom(s => s.ColorItem == null ? "-" : $"{s.ColorItem.ColorName}"));

            #endregion

            #region FinishedGoodsMaster

            CreateMap<FinishedGoodsMaster, FinishedGoodsMasterViewModel>()
                .ForMember(x => x.ColorNameString,
                            o => o.MapFrom(s => s.ColorItem == null ? "-" : $"{s.ColorItem.ColorName}"));

            #endregion

            #region RequisitionMaster

            CreateMap<RequisitionMaster, RequisitionMasterViewModel>()
                .ForMember(x => x.ColorNameString,
                            o => o.MapFrom(s => s.ColorItem == null ? "-" : $"{s.ColorItem.ColorName}"));

            #endregion

            #region PaymentDetail
            CreateMap<PaymentDetail, PaymentDetailViewModel>()
              .ForMember(x => x.PaymentTypeString,
                          o => o.MapFrom(s => s.PaymentType == PaymentType.Blast ? "Blast" : "Paint"));
            CreateMap<PaymentDetailViewModel, PaymentDetail>();
            #endregion

            #region SubPaymentDetail

            CreateMap<SubPaymentDetail, SubPaymentDetailViewModel>()
                .ForMember(x => x.PaymentDetailString,
                            o => o.MapFrom(s => s.PaymentDetail == null ? "-" : s.PaymentDetail.Description))
                .ForMember(x => x.CurrentCost,
                            o => o.MapFrom(s => s.PaymentCostHistory == null ? 0 : s.PaymentCostHistory.PaymentCost));
            CreateMap<SubPaymentDetailViewModel, SubPaymentDetail>();
            #endregion

            #region SubPaymentMaster

            CreateMap<SubPaymentMaster, SubPaymentMasterViewModel>()
                .ForMember(x => x.PaintTeamString,
                            o => o.MapFrom(s => s.PaintTeam == null ? "-" : s.PaintTeam.TeamName))
                .ForMember(x => x.SubPaymentMasterStatusString,
                            o => o.MapFrom(s => s.SubPaymentMasterStatus == SubPaymentMasterStatus.Waiting ? "Waiting" :
                                            (s.SubPaymentMasterStatus == SubPaymentMasterStatus.Complate ? "Complate" : "Cancel")));

            CreateMap<SubPaymentMasterViewModel, SubPaymentMaster>();
            #endregion
        }
    }
}