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
    RequirePaintingScheduleComponent, RequirePaintingViewComponent
} from "../../components/require-painting/require.index";
// modules
import { RequirePaintingRoutingModule } from "./require-painting-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { RequirePaintListService } from "../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../services/require-paint/require-paint-master.service";
import { BlastWorkitemService } from "../../services/require-paint/blast-workitem.service";
import { PaintWorkitemService } from "../../services/require-paint/paint-workitem.service";

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
        RequirePaintMasterService,
        RequirePaintMasterServiceCommunicate,
        BlastWorkitemService,
        PaintWorkitemService,
        // dataTableServiceCommunicate
    ]
})

export class RequirePaintingModule {
}