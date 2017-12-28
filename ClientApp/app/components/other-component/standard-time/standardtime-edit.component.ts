// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators, AbstractControl } from "@angular/forms";
// models
import { StandradTime } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import {
    StandradTimeService, StandradTimeServiceCommunicate
} from "../../../services/standrad-time/standrad-time.service";
// 3rdParty
import { SelectItem } from "primeng/primeng";

@Component({
    selector: "standardtime-edit",
    templateUrl: "./standardtime-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** standardtime-edit component*/
export class StandardtimeEditComponent extends BaseEditComponent<StandradTime, StandradTimeService> {
    /** standardtime-edit ctor */
    constructor(
        service: StandradTimeService,
        serviceCom: StandradTimeServiceCommunicate,
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
    unitRates: Array<SelectItem>;
    typeStdTimes: Array<SelectItem>;

    // on get data by key
    onGetDataByKey(value?: StandradTime): void {
        if (value) {
            this.service.getOneKeyNumber(value.StandradTimeId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                StandradTimeId: 0,
                TypeStandardTime: 1
            };
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
        if (!this.unitRates) {
            this.unitRates = new Array;
            this.unitRates.push({ label: "Select UnitRate", value: undefined });
            this.unitRates.push({ label: "m\xB2/HR", value: "m\xB2/HR" });
            this.unitRates.push({ label: "m\xB2/MH", value: "m\xB2/MH" });
        }
        if (!this.typeStdTimes) {
            this.typeStdTimes = new Array;
            this.typeStdTimes.push({ label: "Paint", value: 1 });
            this.typeStdTimes.push({ label: "Blast", value: 2 });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            StandradTimeId: [this.editValue.StandradTimeId],
            Code: [this.editValue.Code,
                [
                    Validators.required,
                    Validators.maxLength(150)
                ]
            ],
            Description: [this.editValue.Description,
                [
                    Validators.required,
                    Validators.maxLength(250)
                ]
            ],
            Rate: [this.editValue.Rate,
                [
                    Validators.required,
                    Validators.minLength(1)
                ]
            ],
            RateUnit: [this.editValue.RateUnit,
                [
                    Validators.required,
                ]
            ],
            PercentLoss: [this.editValue.PercentLoss,
                [
                    Validators.minLength(1),
                    Validators.maxLength(100),
                ]
            ],
            TypeStandardTime: [this.editValue.TypeStandardTime],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            // ViewModel
            RateWithUnit: [this.editValue.RateWithUnit],
            PercentLossString: [this.editValue.PercentLossString],
            TypeStandardTimeString: [this.editValue.TypeStandardTimeString]
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }
}