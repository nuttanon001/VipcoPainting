// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { PaymentDetail,PaymentCostHistory } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
import { PaymentCostHistoryService } from "../../../services/payment/payment-cost-history.service";
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";

@Component({
    selector: "payment-detail-view",
    templateUrl: "./payment-detail-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** payment-detail-view component*/
export class PaymentDetailViewComponent extends BaseViewComponent<PaymentDetail> {
    /** payment-detail-view ctor */
    constructor(
       private service:PaymentCostHistoryService
    ) {
        super();
    }
    // Parameter
    paymentCostHis: Array<PaymentCostHistory>;
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    columns: Array<TableColumn> = [
        { prop: "StartDate", name: "Start", flexGrow: 1, pipe: this.datePipe },
        { prop: "EndDate", name: "End", flexGrow: 1, pipe: this.datePipe },
        { prop: "PaymentCost", name: "Cost", flexGrow: 1 },
    ];
    // load more data
    onLoadMoreData(value: PaymentDetail) {
        if (value) {
            if (value.PaymentDetailId) {
                this.service.getByMasterId(value.PaymentDetailId)
                    .subscribe(dbData => {
                        this.paymentCostHis = new Array;
                        this.paymentCostHis = dbData.slice();
                    });
            }
        }
    }
}