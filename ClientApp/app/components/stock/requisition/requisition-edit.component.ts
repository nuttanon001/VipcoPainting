// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { RequisitionMaster } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { RequisitionMasterService,RequisitionMasterServiceCommunicate } from "../../../services/stock/requisition-master.service";

@Component({
    selector: "requisition-edit",
    templateUrl: "./requisition-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** requisition-edit component*/
export class RequisitionEditComponent extends BaseEditComponent<RequisitionMaster, RequisitionMasterService> {
    /** requisition-edit ctor */
    constructor(
        service: RequisitionMasterService,
        serviceCom: RequisitionMasterServiceCommunicate,
        private serviceAuth: AuthService,
        private serviceDialog: DialogsService,
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

    // on get data by key
    onGetDataByKey(value?: RequisitionMaster): void {
        if (value) {
            this.service.getOneKeyNumber(value.RequisitionMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                RequisitionMasterId: 0
            };

            if (this.serviceAuth.getAuth) {
                this.editValue.RequisitionBy = this.serviceAuth.getAuth.EmpCode || "";
                this.editValue.RequisitionByString = this.serviceAuth.getAuth.NameThai || "";
            }

            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            RequisitionMasterId: [this.editValue.RequisitionMasterId],
            RequisitionDate: [this.editValue.RequisitionDate,
                [
                    Validators.required,
                ]
            ],
            RequisitionBy: [this.editValue.RequisitionBy,
                [
                    Validators.required,
                ]
            ],
            Quantity: [this.editValue.Quantity,
                [
                    Validators.required,
                    Validators.min(0),
                ]
            ],
            Remark: [this.editValue.Remark],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //FK
            ColorItemId: [this.editValue.ColorItemId,
                [
                    Validators.required
                ]
            ],
            ColorMovementStockId: [this.editValue.ColorMovementStockId,],
            //ViewModel
            RequisitionByString: [this.editValue.RequisitionByString,
                [
                    Validators.required
                ]
            ],
            ColorNameString: [this.editValue.ColorNameString,
                [
                    Validators.required
                ]
            ],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }

    // on OpenDialogBox
    onOpenDialogBox(mode?: string): void {
        if (mode) {
            if (mode.indexOf("Color") !== -1) {
                this.serviceDialog.dialogSelectColorItem(this.viewContainerRef)
                    .subscribe(color => {
                        this.editValueForm.patchValue({
                            ColorItemId: color ? color.ColorItemId : undefined,
                            ColorNameString: color ? color.ColorName : undefined
                        });
                    });
            } else if (mode.indexOf("Employee") !== -1) {
                this.serviceDialog.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        this.editValueForm.patchValue({
                            RequisitionBy: emp ? emp.EmpCode : undefined,
                            RequisitionByString: emp ? emp.NameThai : undefined
                        });
                    });
            }
        }
    }
}