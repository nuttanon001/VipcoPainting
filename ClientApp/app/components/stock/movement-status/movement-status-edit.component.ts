// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { MovementStockStatus } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { MovementStockStatusService,MovementStockStatusServiceCommunicate } from "../../../services/stock/movement-stock-status.service";

@Component({
    selector: "movement-status-edit",
    templateUrl: "./movement-status-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** movement-status-edit component*/
export class MovementStatusEditComponent extends BaseEditComponent<MovementStockStatus, MovementStockStatusService> {
    /** movement-status-edit ctor */
    constructor(
        service: MovementStockStatusService,
        serviceCom: MovementStockStatusServiceCommunicate,
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
    movementStatus: Array<SelectItem>;
    typeMovementStatus: Array<SelectItem>;
    // on get data by key
    onGetDataByKey(value?: MovementStockStatus): void {
        if (value) {
            this.service.getOneKeyNumber(value.MovementStockStatusId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                MovementStockStatusId: 0
            };

            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();

        if (!this.movementStatus) {
            this.movementStatus = new Array;
            this.movementStatus.push({ label: "-", value: undefined });
            this.movementStatus.push({ label: "Stock", value: 1 });
            this.movementStatus.push({ label: "Requsition", value: 2 });
            this.movementStatus.push({ label: "AdjustIncreased", value: 3 });
            this.movementStatus.push({ label: "AdjustDecreased", value: 4 });
            this.movementStatus.push({ label: "Cancel", value: 5 });
        }

        if (!this.typeMovementStatus) {
            this.typeMovementStatus = new Array;
            this.typeMovementStatus.push({ label: "-", value: undefined });
            this.typeMovementStatus.push({ label: "Increased", value: 1 });
            this.typeMovementStatus.push({ label: "Decreased", value: 2 });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            MovementStockStatusId: [this.editValue.MovementStockStatusId],
            StatusName: [this.editValue.StatusName,
                [
                    Validators.required,
                    Validators.maxLength(50)
                ]
            ],
            StatusMovement: [this.editValue.StatusMovement],
            TypeStatusMovement: [this.editValue.TypeStatusMovement],
            //BaseModel
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }
}