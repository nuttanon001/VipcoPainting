import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";
// modules
import { StockRoutingModule } from "./stock-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { ColorMovementStockService,ColorMovementStockServiceCommunicate } from "../../services/stock/color-movement-stock.service";
import { FinishedGoodsMasterService,FinishedGoodsMasterServiceCommunicate } from "../../services/stock/finished-goods-master.service";
import { MovementStockStatusService,MovementStockStatusServiceCommunicate } from "../../services/stock/movement-stock-status.service";
import { RequisitionMasterService,RequisitionMasterServiceCommunicate } from "../../services/stock/requisition-master.service";
// components
import {
    FinishedGoodsCenterComponent, FinishedGoodsEditComponent,
    FinishedGoodsMasterComponent, FinishedGoodsViewComponent,
    MovementStatusCenterComponent, MovementStatusEditComponent,
    MovementStatusMasterComponent, MovementStatusViewComponent,
    MovementStockCenterComponent, MovementStockEditComponent,
    MovementStockMasterComponent, MovementStockViewComponent,
    RequisitionCenterComponent, RequisitionEditComponent,
    RequisitionMasterComponent, RequisitionViewComponent,
} from "../../components/stock/stock.index";

@NgModule({
    declarations: [
        FinishedGoodsCenterComponent, FinishedGoodsEditComponent,
        FinishedGoodsMasterComponent, FinishedGoodsViewComponent,
        MovementStatusCenterComponent, MovementStatusEditComponent,
        MovementStatusMasterComponent, MovementStatusViewComponent,
        MovementStockCenterComponent, MovementStockEditComponent,
        MovementStockMasterComponent, MovementStockViewComponent,
        RequisitionCenterComponent, RequisitionEditComponent,
        RequisitionMasterComponent, RequisitionViewComponent,
    ],
    imports: [
        // angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // custom
        CustomMaterialModule,
        ValidationModule,
        StockRoutingModule,
    ],
    providers: [
        ColorMovementStockService,
        ColorMovementStockServiceCommunicate,
        FinishedGoodsMasterService,
        FinishedGoodsMasterServiceCommunicate,
        MovementStockStatusService,
        MovementStockStatusServiceCommunicate,
        RequisitionMasterService,
        RequisitionMasterServiceCommunicate
    ]
})

export class StockModule {
}