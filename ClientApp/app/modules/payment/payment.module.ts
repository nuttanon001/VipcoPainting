import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";
// modules
import { PaymentRoutingModule } from "./payment-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { PaymentCostHistoryService,PaymentCostHistoryServiceCommunicate } from "../../services/payment/payment-cost-history.service";
import { PaymentDetailService,PaymentDetailServiceCommunicate } from "../../services/payment/payment-detail.service";
import { SubpaymentDetailService,SubpaymentDetailServiceCommunicate } from "../../services/payment/subpayment-detail.service";
import { SubpaymentMasterService,SubpaymentMasterServiceCommunicate } from "../../services/payment/subpayment-master.service";
// components
import {
    PaymentDetailCenterComponent, PaymentDetailEditComponent,
    PaymentDetailMasterComponent, PaymentDetailViewComponent,
    SubpaymentCenterComponent, SubpaymentDetailEditComponent,
    SubpaymentEditComponent, SubpaymentMasterComponent,
    SubpaymentViewComponent,SubpaymentReportComponent
} from "../../components/payment/payment.index";

@NgModule({
    declarations: [
        PaymentDetailCenterComponent, PaymentDetailEditComponent,
        PaymentDetailMasterComponent, PaymentDetailViewComponent,
        SubpaymentCenterComponent, SubpaymentDetailEditComponent,
        SubpaymentEditComponent, SubpaymentMasterComponent,
        SubpaymentViewComponent, SubpaymentReportComponent
    ],
    imports: [
        // angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // custom
        CustomMaterialModule,
        ValidationModule,
        PaymentRoutingModule,
    ],
    providers: [
        PaymentCostHistoryService,
        PaymentCostHistoryServiceCommunicate,
        PaymentDetailService,
        PaymentDetailServiceCommunicate,
        SubpaymentDetailService,
        SubpaymentDetailServiceCommunicate,
        SubpaymentMasterService,
        SubpaymentMasterServiceCommunicate,
    ]
})
export class PaymentModule {
}