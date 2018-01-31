// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { ColorMovementStock } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { ColorMovementStockService, ColorMovementStockServiceCommunicate } from "../../../services/stock/color-movement-stock.service";
import { MovementStockStatusService } from "../../../services/stock/movement-stock-status.service";

@Component({
    selector: "movement-stock-edit",
    templateUrl: "./movement-stock-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** movement-stock-edit component*/
export class MovementStockEditComponent extends BaseEditComponent<ColorMovementStock, ColorMovementStockService> {
    /** movement-stock-edit ctor */
    constructor(
        service: ColorMovementStockService,
        serviceCom: ColorMovementStockServiceCommunicate,
        private serviceAuth: AuthService,
        private serviceDialog: DialogsService,
        private serviceMovementStatus: MovementStockStatusService,
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

    // on get data by key
    onGetDataByKey(value?: ColorMovementStock): void {
        if (value) {
            this.service.getOneKeyNumber(value.ColortMovementStockId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                    if (this.editValue.MovementStockDate) {
                        this.editValue.MovementStockDate = this.editValue.MovementStockDate != null ?
                            new Date(this.editValue.MovementStockDate) : new Date();
                    }

                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                ColortMovementStockId: 0
            };

            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();

        if (!this.movementStatus) {
            this.serviceMovementStatus.getAll()
                .subscribe(dbMovementStatus => {
                    this.movementStatus = new Array;
                    this.movementStatus.push({ label: "-", value: undefined });
                    for (let item of dbMovementStatus) {
                        this.movementStatus.push({ label: `${(item.StatusName || "")}`, value: item.MovementStockStatusId });
                    }
                });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            ColortMovementStockId: [this.editValue.ColortMovementStockId],
            MovementStockDate: [this.editValue.MovementStockDate,
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
            MovementStockStatusId: [this.editValue.MovementStockStatusId,
                [
                    Validators.required
                ]
            ],
            //ViewModel
            StatusNameString: [this.editValue.StatusNameString],
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
        this.serviceDialog.dialogSelectColorItem(this.viewContainerRef)
            .subscribe(color => {
                this.editValueForm.patchValue({
                        ColorItemId: color ? color.ColorItemId : undefined,
                        ColorNameString: color ? color.ColorName : undefined
                    });
            });
    }
}