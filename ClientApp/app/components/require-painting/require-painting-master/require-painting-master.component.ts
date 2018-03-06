import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import {
    RequirePaintMaster, RequirePaintMasterHasList,
    Scroll, ScrollData, RequirePaintList, PaintWorkItem, BlastWorkItem
} from "../../../models/model.index";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
// timezone
import * as moment from "moment-timezone";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";

@Component({
    selector: "require-painting-master",
    templateUrl: "./require-painting-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
// require-painting-master component*/
export class RequirePaintingMasterComponent 
    extends BaseMasterComponent<RequirePaintMaster, RequirePaintMasterService> {
     
    /** require-painting-master ctor */
    constructor(
        service: RequirePaintMasterService,
        serviceCom: RequirePaintMasterServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<RequirePaintMaster>,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
        private serverAuth: AuthService,
        private servicePaintList: RequirePaintListService
    ) {
        super(
            service,
            serviceCom,
            serviceComDataTable,
            dialogsService,
            viewContainerRef
        );
    }

    // Parameter
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    onlyUser: boolean;
    requirePaintLists: Array<RequirePaintList> | undefined;
    hiddenTool: boolean;
    columns: Array<TableColumn> = [
        { prop: "RequireNo", name: "Code", flexGrow: 1 },
        { prop: "JobCode", name: "Job", flexGrow: 1 },
        { prop: "RequireDate", name: "Date", flexGrow: 1, pipe: this.datePipe },
    ];

    // on init override
    ngOnInit(): void {

        this.hiddenTool = false;
        this.ShowEdit = false;
        this.canSave = false;

        this.subscription1 = this.serviceCom.ToParent$.subscribe(
            (TypeValue: [RequirePaintMasterHasList, boolean]) => {
                this.editValue = TypeValue[0].RequirePaintMaster;
                this.requirePaintLists = TypeValue[0].RequirePaintLists;
                this.canSave = TypeValue[1];
            });

        this.subscription2 = this.dataTableServiceCom.ToParent$
            .subscribe((scroll: Scroll) => this.loadPagedData(scroll));
    }

    // on get data with lazy load
    loadPagedData(scroll: Scroll): void {
        if (this.onlyUser) {
            if (this.serverAuth.getAuth) {
                scroll.Where = this.serverAuth.getAuth.UserName || "";
            }
        } else {
            scroll.Where = "";
        }

        if (this.scroll) {
            if (this.scroll.Filter && scroll.Reload) {
                scroll.Filter = this.scroll.Filter;
            }
        }

        this.scroll = scroll;
        this.service.getAllWithScroll(scroll)
            .subscribe((scrollData: ScrollData<RequirePaintMaster>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: RequirePaintMaster): RequirePaintMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.FinishDate !== null) {
                value.FinishDate = moment.tz(value.FinishDate, zone).toDate();
            }
            if (value.ReceiveDate !== null) {
                value.ReceiveDate = moment.tz(value.ReceiveDate, zone).toDate();
            }
            if (value.RequireDate !== null) {
                value.RequireDate = moment.tz(value.RequireDate, zone).toDate();
            }
        }
        return value;
    }

    // on change time zone befor update to webapi
    changeTimezone2(value: RequirePaintList): RequirePaintList {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.PlanEnd !== null) {
                value.PlanEnd = moment.tz(value.PlanEnd, zone).toDate();
            }
            if (value.PlanStart !== null) {
                value.PlanStart = moment.tz(value.PlanStart, zone).toDate();
            }
            if (value.SendWorkItem !== null) {
                value.SendWorkItem = moment.tz(value.SendWorkItem, zone).toDate();
            }
        }
        return value;
    }
    
    // on insert data
    onInsertToDataBase(value: RequirePaintMaster): void {
        if (this.serverAuth.getAuth) {
            value.Creator = this.serverAuth.getAuth.UserName || "";
        }
        let attachs: FileList | undefined = value.AttachFile;
        // change timezone
        value = this.changeTimezone(value);
        // insert data
        this.service.post(value).subscribe(
            (complete: RequirePaintMaster) => {
                if (complete && attachs) {
                    this.onAttactFileToDataBase(complete.RequirePaintingMasterId, attachs, complete.Creator || "");
                }

                this.displayValue = complete;
                if (this.requirePaintLists && complete) {
                    this.requirePaintLists.forEach(item => {
                        // change timezone
                        item = this.changeTimezone2(item);
                        item.RequirePaintingMasterId = complete.RequirePaintingMasterId;
                        item.Creator = complete.Creator;
                    });
                    //debug here
                    // console.log(JSON.stringify(this.requirePaintLists));
                    this.servicePaintList.postLists2(this.requirePaintLists)
                        .subscribe((complate: any) => {
                            this.requirePaintLists = undefined;
                            this.onSaveComplete();
                        },
                        (error: any) => {
                            this.dialogsService.error("Failed !",
                                "Save failed with the following error: WorkItem has error !!!", this.viewContainerRef);
                        });
                } else {
                    this.onSaveComplete();
                }
            },
            (error: any) => {
                console.error(error);
                this.editValue.Creator = undefined;
                this.canSave = true;
                this.dialogsService.error("Failed !",
                    "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
            }
        );
    }

    // on update data
    onUpdateToDataBase(value: RequirePaintMaster): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        let attachs: FileList | undefined = value.AttachFile;

        // remove attach
        if (value.RemoveAttach) {

            this.onRemoveFileFromDataBase(value.RemoveAttach);
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.RequirePaintingMasterId).subscribe(
            (complete: RequirePaintMaster) => {
                if (complete && attachs) {
                    this.onAttactFileToDataBase(complete.RequirePaintingMasterId, attachs,complete.Modifyer || "Someone");
                }
                this.displayValue = complete;
                if (this.requirePaintLists && complete) {
                    this.requirePaintLists.forEach(item => {
                        // change timezone
                        item = this.changeTimezone2(item);

                        if (!item.RequirePaintingListId) {
                            item.RequirePaintingMasterId = complete.RequirePaintingMasterId;
                            item.Creator = complete.Creator;
                        } else {
                            item.Modifyer = complete.Modifyer;
                        }

                        if (item.BlastWorkItems) {
                            item.BlastWorkItems.forEach((blastWork, index) => {
                                if (item.BlastWorkItems) {
                                    let newData: BlastWorkItem = {
                                        BlastWorkItemId: blastWork.BlastWorkItemId,
                                        IntArea: blastWork.IntArea,
                                        IntCalcStdUsage: blastWork.IntCalcStdUsage,
                                        ExtArea: blastWork.ExtArea,
                                        ExtCalcStdUsage: blastWork.ExtCalcStdUsage,
                                        Creator: blastWork.Creator,
                                        CreateDate: blastWork.CreateDate,
                                        Modifyer: blastWork.Modifyer,
                                        ModifyDate: blastWork.ModifyDate,
                                        // StandradTimeInt
                                        StandradTimeIntId: blastWork.StandradTimeIntId,
                                        // StandradTimeExt
                                        StandradTimeExtId: blastWork.StandradTimeExtId,
                                        // SurfaceTypeInt
                                        SurfaceTypeIntId: blastWork.SurfaceTypeIntId,
                                        // SurfaceTypeExt
                                        SurfaceTypeExtId: blastWork.SurfaceTypeExtId,
                                        // RequirePaintingList
                                        RequirePaintingListId: blastWork.RequirePaintingListId
                                    };
                                    item.BlastWorkItems[index] = newData;
                                }
                            })
                        }

                        if (item.PaintWorkItems) {
                            item.PaintWorkItems.forEach((paintWork, index) => {
                                // can't update FromBody with same data from webapi try new object and send back update
                                if (item.PaintWorkItems) {
                                    let newData: PaintWorkItem = {
                                        PaintWorkItemId: paintWork.PaintWorkItemId,
                                        PaintLevel: paintWork.PaintLevel,
                                        IntArea: paintWork.IntArea,
                                        IntDFTMin: paintWork.IntDFTMin,
                                        IntDFTMax: paintWork.IntDFTMax,
                                        IntCalcColorUsage: paintWork.IntCalcColorUsage,
                                        IntCalcStdUsage: paintWork.IntCalcStdUsage,
                                        ExtArea: paintWork.ExtArea,
                                        ExtDFTMin: paintWork.ExtDFTMin,
                                        ExtDFTMax: paintWork.ExtDFTMax,
                                        ExtCalcColorUsage: paintWork.ExtCalcColorUsage,
                                        ExtCalcStdUsage: paintWork.ExtCalcStdUsage,
                                        Creator: paintWork.Creator,
                                        CreateDate: paintWork.CreateDate,
                                        Modifyer: paintWork.Modifyer,
                                        ModifyDate: paintWork.ModifyDate,
                                        // ColorItemInt
                                        IntColorItemId: paintWork.IntColorItemId,
                                        // ColorItemExt
                                        ExtColorItemId: paintWork.ExtColorItemId,
                                        // StandradTimeInt
                                        StandradTimeIntId: paintWork.StandradTimeIntId,
                                        // StandradTimeExt
                                        StandradTimeExtId: paintWork.StandradTimeExtId,
                                        // RequirePaintingList
                                        RequirePaintingListId: paintWork.RequirePaintingListId
                                    };
                                    item.PaintWorkItems[index] = newData;
                                }
                            });
                        }
                    });

                    //debug here
                    // console.log(JSON.stringify(this.requirePaintLists));

                    this.servicePaintList.postLists2(this.requirePaintLists)
                        .subscribe((complate: any) => {
                            this.requirePaintLists = undefined;
                            this.onSaveComplete();
                        },
                        (error: any) => {
                            this.dialogsService.error("Failed !",
                                "Save failed with the following error: WorkItem has error !!!", this.viewContainerRef);
                        });
                } else {
                    this.onSaveComplete();
                }
            },
            (error: any) => {
                console.error(error);
                this.canSave = true;
                this.dialogsService.error("Failed !",
                    "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
            }
        );
    }

    // on detail view
    onDetailView(value?: RequirePaintMaster): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingMasterId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }

    // on cancel edit override
    onCancelEdit(): void {
        this.editValue = undefined;
        this.displayValue = undefined;
        this.requirePaintLists = undefined;
        this.canSave = false;
        this.ShowEdit = false;
        this.onDetailView(undefined);
    }

    // on detail edit OverRide
    onDetailEdit(editValue?: RequirePaintMaster): void {
        // debug here
        // console.log("onDetailEdit on Base Master Component");
        if (editValue) {
            if (editValue.RequirePaintingStatus !== 1 && editValue.RequirePaintingStatus !== 2) {
                this.dialogsService.error("Access Denied", "Status war not waited. you can't edit it.", this.viewContainerRef);
                return;
            }

            if (this.serverAuth.getAuth) {
                if (this.serverAuth.getAuth.LevelUser < 3) {
                    if (this.serverAuth.getAuth.UserName !== editValue.Creator) {
                        this.dialogsService.error("Access Denied", "You don't have permission to access.", this.viewContainerRef);
                        return;
                    }
                }
            }
        }
        super.onDetailEdit(editValue);
    }

    // Attach
    // on attact file
    onAttactFileToDataBase(RequirePaintListId: number, Attacts: FileList, CreateBy: string): void {
        this.service.postAttactFile(RequirePaintListId, Attacts, CreateBy)
            .subscribe(complate => console.log("Upload Complate"), error => console.error(error));
    }

    // on remove file
    onRemoveFileFromDataBase(Attachs: Array<number>): void {
        Attachs.forEach((value: number) => {
            this.service.deleteAttactFile(value)
                .subscribe(complate => console.log("Delete Complate"), error => console.error(error));
        });
    }
}