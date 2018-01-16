import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    PaymentDetailCenterComponent, PaymentDetailMasterComponent,
    SubpaymentCenterComponent, SubpaymentMasterComponent
} from "../../components/payment/payment.index";
// service
import { AuthGuard } from "../../services/auth/auth-guard.service";

const paymentRoutes: Routes = [
    {
        path: "payment-detail",
        component: PaymentDetailCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: PaymentDetailMasterComponent,
            }
        ],
    },
    {
        path: "sub-payment",
        component: SubpaymentCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: SubpaymentMasterComponent,
            }
        ],
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(paymentRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class PaymentRoutingModule {
}