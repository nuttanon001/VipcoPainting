// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import {
    TaskMaster, TaskBlastDetail,
    TaskPaintDetail, RequirePaintList
} from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { TaskMasterService, TaskMasterServiceCommunicate } from "../../../services/task/task-master.service";
import { TaskBlastDetailService } from "../../../services/task/task-blast-detail.service";
import { TaskPaintDetailService } from "../../../services/task/task-paint-detail.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";

@Component({
    selector: "task-edit",
    templateUrl: "./task-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** task-edit component*/
export class TaskEditComponent extends BaseEditComponent<TaskMaster, TaskMasterService> {
    /** task-edit ctor */
    constructor(
        service: TaskMasterService,
        serviceCom: TaskMasterServiceCommunicate,
        private serviceRequiePaintList: RequirePaintListService,
        private serviceTaskPaint: TaskPaintDetailService,
        private serviceTaskBlast: TaskBlastDetailService,
        private servicePaintWork: PaintWorkitemService,
        private serviceBlastWork: BlastWorkitemService,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private serviceDialogs: DialogsService,
        private fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
        );
    }

    // Parameter
    requirePaintList: RequirePaintList;

    // on get data by key
    onGetDataByKey(value?: TaskMaster): void {
        if (value) {
            if (value.RequirePaintingListId) {
                this.serviceRequiePaintList.getOneKeyNumber(value.RequirePaintingListId)
                    .subscribe(dbRequirePaintList => {
                        this.requirePaintList = dbRequirePaintList;
                    });
            }

            if (value.TaskMasterId) {
                this.service.getOneKeyNumber(value.TaskMasterId)
                    .subscribe(dbData => {
                        this.editValue = dbData;
                        // set Date
                        if (this.editValue.ActualEDate) {
                            this.editValue.ActualEDate = this.editValue.ActualEDate != null ?
                                new Date(this.editValue.ActualEDate) : new Date();
                        }

                        if (this.editValue.ActualSDate) {
                            this.editValue.ActualSDate = this.editValue.ActualSDate != null ?
                                new Date(this.editValue.ActualSDate) : new Date();
                        }

                        this.serviceTaskBlast.getByMasterId(this.editValue.TaskMasterId)
                            .subscribe(dbTaskBlast => {
                                this.editValue.TaskBlastDetails = dbTaskBlast.slice();
                                this.editValueForm.patchValue({
                                    TaskBlastDetails: this.editValue.TaskBlastDetails,
                                });
                            });

                        this.serviceTaskPaint.getByMasterId(this.editValue.TaskMasterId)
                            .subscribe(dbTaskPaint => {
                                this.editValue.TaskPaintDetails = dbTaskPaint.slice();
                                this.editValueForm.patchValue({
                                    TaskPaintDetails: this.editValue.TaskPaintDetails,
                                });
                            });

                    }, error => console.error(error), () => this.defineData());
            } else {
                this.editValue = {
                    TaskMasterId: 0,
                    RequirePaintingListId: value.RequirePaintingListId,
                    AssignDate: new Date
                };

                if (this.serviceAuth.getAuth) {
                    this.editValue.AssignBy = this.serviceAuth.getAuth.EmpCode || "";
                    this.editValue.AssignByString = this.serviceAuth.getAuth.NameThai || "";
                }

                this.defineData();
            }
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // onGetPaintWorkItem
    onGetPaintWorkItemAndBlastWorkItem(MasterId: number): void {
        if (MasterId) {
            this.servicePaintWork.getByMasterId(MasterId)
                .subscribe(dbPaintWork => {
                    if (!this.editValue.TaskPaintDetails) {
                        this.editValue.TaskPaintDetails = new Array;
                    }

                    dbPaintWork.forEach(item => {
                        let newTaskPaint: TaskPaintDetail = {
                            TaskPaintDetailId: 0,
                            PaintWorkItemId: item.PaintWorkItemId,
                            PaintWorkItem: item,
                            PaintTeamId: 1,
                        };
                        if (this.editValue.TaskPaintDetails) {
                            this.editValue.TaskPaintDetails.push(newTaskPaint);
                        }
                    });
                });

            this.serviceBlastWork.getByMasterId(MasterId)
                .subscribe(dbBlastWork => {
                    if (!this.editValue.TaskBlastDetails) {
                        this.editValue.TaskBlastDetails = new Array;
                    }

                    dbBlastWork.forEach(item => {
                        let newTaskPaint: TaskBlastDetail = {
                            TaskBlastDetailId: 0,
                            BlastWorkItemId: item.BlastWorkItemId,
                            BlastWorkItem: item,
                            BlastRoomId: 1,
                        };
                        if (this.editValue.TaskBlastDetails) {
                            this.editValue.TaskBlastDetails.push(newTaskPaint);
                        }
                    });
                });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TaskMasterId: [this.editValue.TaskMasterId],
            TaskNo: [this.editValue.TaskNo],
            AssignDate: [this.editValue.AssignDate],
            AssignBy: [this.editValue.AssignBy],
            ActualSDate: [this.editValue.ActualSDate,
                [
                    Validators.required
                ]
            ],
            ActualEDate: [this.editValue.ActualEDate,
                [
                    Validators.required
                ]
            ],
            TaskProgress: [this.editValue.TaskProgress],
            TaskStatus: [this.editValue.TaskStatus],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //FK
            RequirePaintingListId: [this.editValue.RequirePaintingListId,
                [
                    Validators.required
                ]
            ],
            TaskBlastDetails: [this.editValue.TaskBlastDetails],
            TaskPaintDetails: [this.editValue.TaskPaintDetails],
            //ViewModel
            AssignByString: [this.editValue.AssignByString],
            ProjectCodeSubString: [this.editValue.ProjectCodeSubString],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }
}