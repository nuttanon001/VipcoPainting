// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { PaymentDetail,PaymentCostHistory } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
import { TableColumn } from "@swimlane/ngx-datatable";
// pipe
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaymentCostHistoryService } from "../../../services/payment/payment-cost-history.service";
import { PaymentDetailService,PaymentDetailServiceCommunicate } from "../../../services/payment/payment-detail.service";

@Component({
    selector: "payment-detail-edit",
    templateUrl: "./payment-detail-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** payment-detail-edit component*/
export class PaymentDetailEditComponent 
    extends BaseEditComponent<PaymentDetail, PaymentDetailService> {
    /** payment-detail-edit ctor */
    constructor(
        service: PaymentDetailService,
        serviceCom: PaymentDetailServiceCommunicate,
        private servicePaymentCost: PaymentCostHistoryService,
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
    paymentCostHis: Array<PaymentCostHistory>;
    paymentType: Array<SelectItem>;
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    columns: Array<TableColumn> = [
        { prop: "StartDate", name: "Start", flexGrow: 1, pipe: this.datePipe },
        { prop: "EndDate", name: "End", flexGrow: 1, pipe: this.datePipe },
        { prop: "PaymentCost", name: "Cost", flexGrow: 1 },
    ];

    // on get data by key
    onGetDataByKey(value?: PaymentDetail): void {
        if (!this.paymentCostHis) {
            this.paymentCostHis = new Array;
        }

        if (value) {
            this.service.getOneKeyNumber(value.PaymentDetailId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                    this.servicePaymentCost.getByMasterId(value.PaymentDetailId)
                        .subscribe(dbPaymentCost => {
                            this.paymentCostHis = dbPaymentCost.slice();
                        });
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                PaymentDetailId: 0,
                LastCost: 0,
                PaymentType: 1
            };
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();

        if (!this.paymentType) {
            this.paymentType = new Array;
            this.paymentType.push({ label: "Blast", value: 1 });
            this.paymentType.push({ label: "Paint", value: 2 });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            PaymentDetailId: [this.editValue.PaymentDetailId],
            Description: [this.editValue.Description,
                [
                    Validators.required,
                    Validators.maxLength(200)
                ]
            ],
            LastCost: [this.editValue.LastCost,
                [
                    Validators.required,
                    Validators.min(0),
                ]
            ],
            PaymentType: [this.editValue.PaymentType],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //ViewModel
            PaymentTypeString: [this.editValue.PaymentTypeString,
                [
                    Validators.required
                ]
            ],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }
}