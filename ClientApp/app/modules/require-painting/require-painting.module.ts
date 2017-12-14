import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";

// components
import { RequirePaintingCenterComponent } from "../../components/require-painting/require-painting-master/require-painting-center.component";
import { RequirePaintingEditComponent } from "../../components/require-painting/require-painting-master/require-painting-edit.component";
import { RequirePaintingMasterComponent } from "../../components/require-painting/require-painting-master/require-painting-master.component";
import { RequirePaintingViewComponent } from "../../components/require-painting/require-painting-master/require-painting-view.component";
import { RequirePaintingListEditComponent } from "../../components/require-painting/require-painting-list/require-painting-list-edit.component";
import { RequirePaintingListViewComponent } from "../../components/require-painting/require-painting-list/require-painting-list-view.component";
import { RequirePaintingSubEditComponent } from "../../components/require-painting/require-painting-sub/require-painting-sub-edit.component";
// modules
import { RequirePaintingRoutingModule } from "./require-painting-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { RequirePaintListService } from "../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService,RequirePaintMasterServiceCommunicate } from "../../services/require-paint/require-paint-master.service";
import { RequirePaintSubService } from "../../services/require-paint/require-paint-sub.service";


@NgModule({
    declarations: [
        RequirePaintingCenterComponent,
        RequirePaintingEditComponent,
        RequirePaintingMasterComponent,
        RequirePaintingViewComponent,
        RequirePaintingListEditComponent,
        RequirePaintingListViewComponent,
        RequirePaintingSubEditComponent,
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
        RequirePaintSubService
        // dataTableServiceCommunicate
    ]
})

export class RequirePaintingModule {
}