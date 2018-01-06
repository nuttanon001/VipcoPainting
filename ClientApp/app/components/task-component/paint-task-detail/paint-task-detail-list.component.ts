// angular
import { Component, ViewContainerRef, OnInit,OnDestroy, Input, Output, EventEmitter } from "@angular/core";
// models
import { PaintWorkItem, BlastWorkItem, PaintTaskDetail, PaintTaskMaster } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTaskDetailService } from "../../../services/paint-task/paint-task-detail.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";
import { SelectItem } from "primeng/primeng";
import { BlastRoomService } from "../../../services/task/blast-room.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";

@Component({
    selector: "paint-task-detail-list",
    templateUrl: "./paint-task-detail-list.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** paint-task-detail-list component*/
export class PaintTaskDetailListComponent implements OnInit, OnDestroy {
    /** paint-task-detail-list ctor */
    constructor(
        private service: PaintTaskDetailService,
        private serviceBlastRoom: BlastRoomService,
        private servicePaintTeam: PaintTeamService,
        private servicePaintWork: PaintWorkitemService,
        private serviceBlastWork: BlastWorkitemService,
    ) { }

    // Parameter
    //PaintWork
    intPaintWorks: Array<PaintTaskDetail> | undefined;
    extPaintWorks: Array<PaintTaskDetail> | undefined;
    //BlastWork
    intBlastWorks: Array<PaintTaskDetail> | undefined;
    extBlastWorks: Array<PaintTaskDetail> | undefined;
    // SelectedItem
    blastRooms: Array<SelectItem>;
    paintTeams: Array<SelectItem>;

    @Input() ReadOnly: boolean = false;
    @Output("onChange") onChange = new EventEmitter<Array<PaintTaskDetail>>();
    @Input() paintTaskMaster: PaintTaskMaster;

    // on Init
    ngOnInit(): void {
        this.getBlastRoomCombobox();
        this.getPaintTeamCombobox();
        if (this.paintTaskMaster) {
            if (this.paintTaskMaster.PaintTaskMasterId) {
                this.service.getByMasterId(this.paintTaskMaster.PaintTaskMasterId)
                    .subscribe(dbPaintTaskDetails => {
                        if (dbPaintTaskDetails) {
                            dbPaintTaskDetails.forEach((item, index) => {
                                // set Date
                                if (item.ActualEDate) {
                                    item.ActualEDate = item.ActualEDate != null ?
                                        new Date(item.ActualEDate) : new Date();
                                }
                                if (item.ActualSDate) {
                                    item.ActualSDate = item.ActualSDate != null ?
                                        new Date(item.ActualSDate) : new Date();
                                }
                                if (item.PlanEDate) {
                                    item.PlanEDate = item.PlanEDate != null ?
                                        new Date(item.PlanEDate) : new Date();
                                }
                                if (item.PlanSDate) {
                                    item.PlanSDate = item.PlanSDate != null ?
                                        new Date(item.PlanSDate) : new Date();
                                }

                                this.onChoosePushToArray(item);
                            });
                        }
                    });
            } else {
                if (this.paintTaskMaster.RequirePaintingListId) {
                    this.serviceBlastWork.getByMasterId(this.paintTaskMaster.RequirePaintingListId)
                        .subscribe(dbBlastWork => {
                            if (dbBlastWork) {
                                dbBlastWork.forEach(item => {
                                    //1=Blast
                                    let newPaintTaskDetail: PaintTaskDetail = {
                                        PaintTaskDetailId: 0,
                                        PaintTaskDetailStatus: 1,
                                        PaintTaskDetailType: 1,
                                        TaskDetailProgress:0,
                                        PlanSDate: new Date(),
                                        PlanEDate: new Date(),
                                        ActualEDate: undefined,
                                        ActualSDate: undefined,
                                        //FK
                                        PaintTaskMasterId: this.paintTaskMaster.PaintTaskMasterId,
                                        BlastWorkItemId: item.BlastWorkItemId,
                                        BlastRoomId: 1,
                                        //ViewModel
                                        BlastWorkItem: Object.assign({}, item),
                                    };
                                    // Internal = 1
                                    if (item.IntArea) {
                                        newPaintTaskDetail.PaintTaskDetailLayer = 1;
                                        this.onChoosePushToArray(newPaintTaskDetail);
                                    }
                                    //External = 2,
                                    if (item.ExtArea) {
                                        newPaintTaskDetail.PaintTaskDetailLayer = 2;
                                        this.onChoosePushToArray(newPaintTaskDetail);
                                    }
                                });

                                this.onHasChange(true);
                            }
                        });

                    this.servicePaintWork.getByMasterId(this.paintTaskMaster.RequirePaintingListId,"GetByMasterCalculate/")
                        .subscribe(dbPaintWork => {
                            if (dbPaintWork) {
                                dbPaintWork.forEach(item => {
                                    //2=Paint
                                    let newPaintTaskDetail: PaintTaskDetail = {
                                        PaintTaskDetailId: 0,
                                        PaintTaskDetailStatus: 1,
                                        PaintTaskDetailType: 2,
                                        TaskDetailProgress: 0,
                                        PlanSDate: new Date(),
                                        PlanEDate: new Date(),
                                        //FK
                                        PaintTaskMasterId: this.paintTaskMaster.PaintTaskMasterId,
                                        PaintWorkItemId: item.PaintWorkItemId,
                                        PaintTeamId: 1,
                                        //ViewModel
                                        PaintWorkItem: Object.assign({}, item),
                                    };

                                    // Internal = 1
                                    if (item.IntArea) {
                                        newPaintTaskDetail.PaintTaskDetailLayer = 1;
                                        this.onChoosePushToArray(newPaintTaskDetail);
                                    }
                                    //External = 2,
                                    if (item.ExtArea) {
                                        newPaintTaskDetail.PaintTaskDetailLayer = 2;
                                        this.onChoosePushToArray(newPaintTaskDetail);
                                    }
                                });

                                this.onHasChange(true);
                            }
                        });
                }
            }
        }
    }

    ngOnDestroy(): void {
        this.intBlastWorks = undefined;
        this.extBlastWorks = undefined;
        this.intPaintWorks = undefined;
        this.extPaintWorks = undefined;
    }

    // get BlastRoom Array
    getBlastRoomCombobox(): void {
        if (!this.blastRooms) {
            // BlastRoom ComboBox
            this.serviceBlastRoom.getAll()
                .subscribe(dbPatinTeam => {
                    this.blastRooms = new Array;
                    for (let item of dbPatinTeam) {
                        this.blastRooms.push({ label: `${(item.BlastRoomName || "")}/${(item.TeamBlastString || "")}`, value: item.BlastRoomId });
                    }
                }, error => console.error(error));
        }
    }

    // get PaintTeam Array
    getPaintTeamCombobox(): void {
        if (!this.paintTeams) {
            // paintTeam ComboBox
            this.servicePaintTeam.getAll()
                .subscribe(dbPatinTeam => {
                    this.paintTeams = new Array;
                    for (let item of dbPatinTeam) {
                        this.paintTeams.push({ label: `${(item.TeamName || "")}`, value: item.PaintTeamId });
                    }
                }, error => console.error(error));
        }
    }

    // on add item to array
    onChoosePushToArray(item: PaintTaskDetail): void {
        //1=Blast;2=Paint
        if (item.PaintTaskDetailType === 1) {
            // Internal = 1,External = 2,
            if (item.PaintTaskDetailLayer === 1) {
                if (!this.intBlastWorks) {
                    this.intBlastWorks = new Array;
                }
                this.intBlastWorks.push(Object.assign({}, item));
            } else {
                if (!this.extBlastWorks) {
                    this.extBlastWorks = new Array;
                }
                this.extBlastWorks.push(Object.assign({}, item));
            }
        } else {
            // Internal = 1,External = 2,
            if (item.PaintTaskDetailLayer === 1) {
                if (!this.intPaintWorks) {
                    this.intPaintWorks = new Array;
                }
                this.intPaintWorks.push(Object.assign({}, item));
            } else {
                if (!this.extPaintWorks) {
                    this.extPaintWorks = new Array;
                }
                this.extPaintWorks.push(Object.assign({}, item));
            }
        }
    }

    //onHasChange
    onHasChange(hasChange: boolean) {
        let ListPaintTaskDetail: Array<PaintTaskDetail> = new Array;
        if (this.intBlastWorks) {
            ListPaintTaskDetail.push(...this.intBlastWorks);
        }
        if (this.extBlastWorks) {
            ListPaintTaskDetail.push(...this.extBlastWorks);
        }
        if (this.intPaintWorks) {
            ListPaintTaskDetail.push(...this.intPaintWorks);
        }
        if (this.extPaintWorks) {
            ListPaintTaskDetail.push(...this.extPaintWorks);
        }

        if (ListPaintTaskDetail) {
            this.onChange.emit(ListPaintTaskDetail)
        }
    }
}