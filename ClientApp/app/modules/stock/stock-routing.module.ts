import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    FinishedGoodsCenterComponent, MovementStatusCenterComponent,
    MovementStockCenterComponent, RequisitionCenterComponent,
    FinishedGoodsMasterComponent, MovementStatusMasterComponent,
    MovementStockMasterComponent, RequisitionMasterComponent
} from "../../components/stock/stock.index";
// service
import { AuthGuard } from "../../services/auth/auth-guard.service";

const stockRoutes: Routes = [
    {
        path: "finished-goods",
        component: FinishedGoodsCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: FinishedGoodsMasterComponent,
            }
        ],
    },
    {
        path: "movement-stock",
        component: MovementStockCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: MovementStockMasterComponent,
            }
        ],
    },
    {
        path: "movement-status",
        component: MovementStatusCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: MovementStatusMasterComponent,
            }
        ],
    },
    {
        path: "requisition",
        component: RequisitionCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: RequisitionMasterComponent,
            }
        ],
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(stockRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class StockRoutingModule {
}