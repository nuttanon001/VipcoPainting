import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";

// components
import {
    BlastWorkitemEditComponent, PaintWorkItemEditComponent,
    RequireListEditComponent, RequirePaintingCenterComponent,
    RequirePaintingEditComponent, RequirePaintingMasterComponent,
    RequirePaintingScheduleComponent, RequirePaintingViewComponent,
    RequirePaintingListWorkitemComponent, ListPaintBlastWorkitemComponent,
    InitialRequirePaintingCenterComponent, InitialRequirePaintingEditComponent,
    InitialRequirePaintingMasterComponent, InitialRequirePaintingViewComponent,
    InitialRequirePaintingListComponent, InitialRequirePaintingScheduleComponent,
    InitialRequireWorkitemMasterComponent, RequirePaintingListByinitialCenterComponent,
    RequirePaintingListByinitialEditComponent, RequirePaintingListByinitialMasterComponent,
    RequirePaintingListByinitialViewComponent
} from "../../components/require-painting/require.index";
// modules
import { RequirePaintingRoutingModule } from "./require-painting-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { RequirePaintListService, RequirePaintListServiceCommunicate } from "../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate, RequirePaintMasterHasInitialServiceCommunicate } from "../../services/require-paint/require-paint-master.service";
import { BlastWorkitemService } from "../../services/require-paint/blast-workitem.service";
import { PaintWorkitemService } from "../../services/require-paint/paint-workitem.service";
import { InitialRequirePaintService, InitialRequirePaintServiceCommunicate } from "../../services/require-paint/initial-require-paint.service";

@NgModule({
    declarations: [
        RequirePaintingCenterComponent,
        RequirePaintingEditComponent,
        RequirePaintingMasterComponent,
        RequirePaintingViewComponent,
        RequireListEditComponent,
        PaintWorkItemEditComponent,
        BlastWorkitemEditComponent,
        RequirePaintingScheduleComponent,
        RequirePaintingListWorkitemComponent,
        ListPaintBlastWorkitemComponent,
        InitialRequirePaintingCenterComponent,
        InitialRequirePaintingEditComponent,
        InitialRequirePaintingMasterComponent,
        InitialRequirePaintingViewComponent,
        InitialRequirePaintingListComponent,
        InitialRequirePaintingScheduleComponent,
        InitialRequireWorkitemMasterComponent,
        RequirePaintingListByinitialCenterComponent,
        RequirePaintingListByinitialEditComponent,
        RequirePaintingListByinitialMasterComponent,
        RequirePaintingListByinitialViewComponent
    ],
    imports: [
        // angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // custom
        CustomMaterialModule,
        ValidationModule,
        RequirePaintingRoutingModule,
    ],
    providers: [
        RequirePaintListService,
        RequirePaintListServiceCommunicate,
        RequirePaintMasterService,
        RequirePaintMasterServiceCommunicate,
        BlastWorkitemService,
        PaintWorkitemService,
        InitialRequirePaintService,
        InitialRequirePaintServiceCommunicate,
        RequirePaintMasterHasInitialServiceCommunicate
        // dataTableServiceCommunicate
    ]
})

export class RequirePaintingModule {
}