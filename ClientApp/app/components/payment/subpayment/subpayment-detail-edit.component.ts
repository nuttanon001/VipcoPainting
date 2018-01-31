// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { SubPaymentDetail } from "../../../models/model.index";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";

@Component({
    selector: "subpayment-detail-edit",
    templateUrl: "./subpayment-detail-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** subpayment-detail-edit component*/
export class SubpaymentDetailEditComponent {
    /** subpayment-detail-edit ctor */
    constructor(
        private fb: FormBuilder,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef,
    ) { }
    // Parameter
    // Input
    @Input() subPaymentDetail: SubPaymentDetail;
    // Output
    @Output() ComplateOrCancel = new EventEmitter<SubPaymentDetail | undefined>();
    // FormGroup
    subPaymentDetailForm: FormGroup;
    // OnInit
    ngOnInit(): void {
        this.buildForm();
    }
    // build Form
    buildForm(): void {
        this.subPaymentDetailForm = this.fb.group({
            SubPaymentDetailId: [this.subPaymentDetail.SubPaymentDetailId],
            AdditionArea:[this.subPaymentDetail.AdditionArea],
            AreaWorkLoad: [this.subPaymentDetail.AreaWorkLoad],
            CalcCost: [this.subPaymentDetail.CalcCost],
            AdditionCost: [this.subPaymentDetail.AdditionCost],
            PaymentDate: [this.subPaymentDetail.PaymentDate],
            Remark: [this.subPaymentDetail.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            // BaseModel
            Creator: [this.subPaymentDetail.Creator],
            CreateDate: [this.subPaymentDetail.CreateDate],
            Modifyer: [this.subPaymentDetail.Modifyer],
            ModifyDate: [this.subPaymentDetail.ModifyDate],
            //FK
            SubPaymentMasterId: [this.subPaymentDetail.SubPaymentMasterId],
            PaymentDetailId: [this.subPaymentDetail.PaymentDetailId],
            PaymentCostHistoryId:[this.subPaymentDetail.PaymentCostHistoryId],
            //ViewModel
            CurrentCost: [this.subPaymentDetail.CurrentCost],
            PaymentDetailString: [this.subPaymentDetail.PaymentDetailString],
        });
    }

    // on New/Update
    onUpdateClick(): void {
        if (this.subPaymentDetailForm) {
            let newOrUpdate: SubPaymentDetail = this.subPaymentDetailForm.value;
            this.ComplateOrCancel.emit(newOrUpdate);
        }
    }

    // on Cancel
    onCancelClick(): void {
        this.ComplateOrCancel.emit(undefined);
    }
}