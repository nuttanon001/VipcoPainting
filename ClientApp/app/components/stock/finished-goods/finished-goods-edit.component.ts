// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { FinishedGoodsMaster } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { FinishedGoodsMasterService,FinishedGoodsMasterServiceCommunicate } from "../../../services/stock/finished-goods-master.service";

@Component({
    selector: "finished-goods-edit",
    templateUrl: "./finished-goods-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** finished-goods-edit component*/
export class FinishedGoodsEditComponent extends BaseEditComponent<FinishedGoodsMaster, FinishedGoodsMasterService> {
    /** finished-goods-edit ctor */
    constructor(
        service: FinishedGoodsMasterService,
        serviceCom: FinishedGoodsMasterServiceCommunicate,
        private serviceAuth: AuthService,
        private serviceDialog: DialogsService,
        private viewContainerRef: ViewContainerRef,
        private fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
        );
    }

    // Parameter

    // on get data by key
    onGetDataByKey(value?: FinishedGoodsMaster): void {
        if (value) {
            this.service.getOneKeyNumber(value.FinishedGoodsMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                FinishedGoodsMasterId: 0
            };

            if (this.serviceAuth.getAuth) {
                this.editValue.ReceiveBy = this.serviceAuth.getAuth.EmpCode || "";
                this.editValue.ReceiveByString = this.serviceAuth.getAuth.NameThai || "";
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
            FinishedGoodsMasterId: [this.editValue.FinishedGoodsMasterId],
            FinishedGoodsDate: [this.editValue.FinishedGoodsDate,
                [
                    Validators.required,
                ]
            ],
            ReceiveBy: [this.editValue.ReceiveBy,
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
            ReceiveByString: [this.editValue.ReceiveByString,
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
                            ReceiveBy: emp ? emp.EmpCode : undefined,
                            ReceiveByString: emp ? emp.NameThai : undefined
                        });
                    });
            }
        }
    }
}