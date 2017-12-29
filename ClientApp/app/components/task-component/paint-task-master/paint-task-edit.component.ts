// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators, AbstractControl } from "@angular/forms";
// models
import { PaintTaskMaster, RequirePaintList, RequirePaintMaster, PaintTaskDetail } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";
import { PaintTaskMasterService,PaintTaskMasterServiceCommunicate } from "../../../services/paint-task/paint-task-master.service";

@Component({
    selector: "paint-task-edit",
    templateUrl: "./paint-task-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** paint-task-edit component*/
export class PaintTaskEditComponent extends BaseEditComponent<PaintTaskMaster, PaintTaskMasterService> {
    /** paint-task-edit ctor */
    constructor(
        service: PaintTaskMasterService,
        serviceCom: PaintTaskMasterServiceCommunicate,
        private serviceRequirePaintMaster: RequirePaintMasterService,
        private serviceRequirePaintList: RequirePaintListService,
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
    requirePaintMaster: RequirePaintMaster;

    maxDate: Date = new Date;
   
    // on get data by key
    onGetDataByKey(value?: PaintTaskMaster): void {
        if (value) {
            if (value.RequirePaintingListId) {
                this.serviceRequirePaintList.getOneKeyNumber(value.RequirePaintingListId)
                    .subscribe(dbRequirePaintList => {
                        this.requirePaintList = dbRequirePaintList;
                        // Get RequirePaintingMaster
                        if (this.requirePaintList.RequirePaintingMasterId) {
                            this.serviceRequirePaintMaster.getOneKeyNumber(this.requirePaintList.RequirePaintingMasterId)
                                .subscribe(dbRequirePaintingMaster => {
                                    this.requirePaintMaster = dbRequirePaintingMaster;
                                });
                        }
                    });
            }

            if (value.PaintTaskMasterId) {
                this.service.getOneKeyNumber(value.PaintTaskMasterId)
                    .subscribe(dbData => {
                        this.editValue = dbData;
                        // Get TaskBlastDetails
                    }, error => console.error(error), () => this.defineData());
            } else {
                this.editValue = {
                    PaintTaskMasterId: 0,
                    PaintTaskStatus : 1,
                    MainProgress: 0,
                    RequirePaintingListId: value.RequirePaintingListId,
                    AssignDate: new Date,
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

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            PaintTaskMasterId: [this.editValue.PaintTaskMasterId],
            TaskPaintNo: [this.editValue.TaskPaintNo],
            AssignDate: [this.editValue.AssignDate],
            AssignBy: [this.editValue.AssignBy],
            MainProgress: [this.editValue.MainProgress],
            PaintTaskStatus: [this.editValue.PaintTaskStatus],
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
            PaintTaskDetails: [this.editValue.PaintTaskDetails],
            //ViewModel
            AssignByString: [this.editValue.AssignByString],
            ProjectCodeSubString: [this.editValue.ProjectCodeSubString]
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        this.onValueChanged();
    }

    onPaintTaskDetailsChange(paintTaskDetails?: Array<PaintTaskDetail>): void {
        if (paintTaskDetails) {
            this.editValueForm.patchValue({
                PaintTaskDetails: paintTaskDetails,
            });
        }
    }
}