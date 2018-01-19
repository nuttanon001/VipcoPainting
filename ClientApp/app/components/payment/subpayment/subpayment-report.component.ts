//angular core
import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
//service
import { SubpaymentMasterService } from "../../../services/payment/subpayment-master.service";

@Component({
    selector: "subpayment-report",
    templateUrl: "./subpayment-report.component.html",
    styleUrls: ["../../../styles/report.style.scss"],
})
/** subpayment-report component*/
export class SubpaymentReportComponent {
    /** subpayment-report ctor */
    constructor(
        private service: SubpaymentMasterService
    ) { }

    //Parameter
    @Input() SubPaymentMasterId: number;
    @Output() Back = new EventEmitter<boolean>();
    SubPayment: any;

    // called by Angular after aint-task-detail-paint-report component initialized */
    ngOnInit(): void {
        if (this.SubPaymentMasterId) {
            this.service.getReportSubPaymentToPdf(this.SubPaymentMasterId)
                .subscribe(dbReport => {
                    console.log("Data is",JSON.stringify(dbReport));
                    this.SubPayment = dbReport;
                });
        } else {
            this.onBackToMaster();
        }
    }

    // on Print OverTimeMaster
    onPrintToPaper(): void {
        window.print();
    }

    // on Back
    onBackToMaster(): void {
        this.Back.emit(true);
    }
}