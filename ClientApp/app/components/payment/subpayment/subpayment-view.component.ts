// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { SubPaymentMaster, SubPaymentDetail } from "../../../models/model.index";
// 3rd party
import { TableColumn } from "@swimlane/ngx-datatable";
// pipe
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { SubpaymentDetailService } from "../../../services/payment/subpayment-detail.service";

@Component({
    selector: "subpayment-view",
    templateUrl: "./subpayment-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** subpayment-view component*/
export class SubpaymentViewComponent extends BaseViewComponent<SubPaymentMaster> {
    /** subpayment-view ctor */
    constructor(
        private service: SubpaymentDetailService
    ) {
        super();
    }

    // Parameter
    subPaymentDetails: Array<SubPaymentDetail>;
    @Input("height") height: string = "calc(100vh - 184px)";
    columns: Array<TableColumn> = [
        { prop: "PaymentDetailString", name: "PaymentInfo", flexGrow: 2 },
        { prop: "AreaWorkLoad", name: "Area", flexGrow: 1 },
        { prop: "CurrentCost", name: "Cost", flexGrow: 1 },
        { prop: "CalcCost", name: "Total", flexGrow: 1 },
    ];

    // load more data
    onLoadMoreData(value: SubPaymentMaster) {
        if (value) {
            if (value.SubPaymentMasterId) {
                this.service.getByMasterId(value.SubPaymentMasterId)
                    .subscribe(dbData => {
                        this.subPaymentDetails = new Array;
                        this.subPaymentDetails = dbData.slice();
                    });
            }
        }
    }
}