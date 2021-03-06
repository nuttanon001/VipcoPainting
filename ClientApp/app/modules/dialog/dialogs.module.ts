﻿import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import "rxjs/Rx";
import "hammerjs";
// services
import { DialogsService } from "../../services/dialog/dialogs.service";
import { RequirePaintListService } from "../../services/require-paint/require-paint-list.service";
// components
import {
    ConfirmDialog, ContextDialog,
    ErrorDialog,
    ColorItemDialogComponent,
    StandradTimeDialogComponent,
    SurfaceTypeDialogComponent,
    EmployeeDialogComponent,
    ProjectDialogComponent,
    RequirePaintingDialogComponent,
    RequirePaintingViewDialogComponent,
    ScheduleDialogComponent,
    RequisitionListDialogComponent,
    RequirePaintingBydetailViewComponent
} from "../../components/dialog/dialog.index";

import { RequirePaintingViewScheduleComponent } from "../../components/require-painting/require-painting-master/require-painting-view-schedule.component";
import {
    ProjectsubEditExtendComponent,ColorEditExtendComponent
} from "../../components/other-component/orther.index";
// modules
import { CustomMaterialModule } from "../customer-material/customer-material.module";
import { ValidationModule } from "../validation/validation.module";

@NgModule({
    imports: [
        // angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // customer Module
        ValidationModule,
        CustomMaterialModule,
        // mark JobCardModule,
    ],
    exports: [
        ErrorDialog,
        ConfirmDialog,
        ContextDialog,
        ColorItemDialogComponent,
        StandradTimeDialogComponent,
        SurfaceTypeDialogComponent,
        EmployeeDialogComponent,
        ProjectDialogComponent,
        RequirePaintingDialogComponent,
        RequirePaintingViewDialogComponent,
        RequirePaintingViewScheduleComponent,
        ScheduleDialogComponent,
        RequisitionListDialogComponent,
        ProjectsubEditExtendComponent,
        ColorEditExtendComponent,
        RequirePaintingBydetailViewComponent
    ],
    declarations: [
        ErrorDialog,
        ConfirmDialog,
        ContextDialog,
        ColorItemDialogComponent,
        StandradTimeDialogComponent,
        SurfaceTypeDialogComponent,
        EmployeeDialogComponent,
        ProjectDialogComponent,
        RequirePaintingDialogComponent,
        RequirePaintingViewDialogComponent,
        RequirePaintingViewScheduleComponent,
        ScheduleDialogComponent,
        RequisitionListDialogComponent,
        ProjectsubEditExtendComponent,
        ColorEditExtendComponent,
        RequirePaintingBydetailViewComponent
    ],
    providers: [
        DialogsService,
        RequirePaintListService,
    ],
    // a list of components that are not referenced in a reachable component template.
    // doc url is :https://angular.io/guide/ngmodule-faq
    entryComponents: [
        ErrorDialog,
        ConfirmDialog,
        ContextDialog,
        ColorItemDialogComponent,
        StandradTimeDialogComponent,
        SurfaceTypeDialogComponent,
        EmployeeDialogComponent,
        ProjectDialogComponent,
        RequirePaintingDialogComponent,
        RequirePaintingViewDialogComponent,
        RequirePaintingViewScheduleComponent,
        ScheduleDialogComponent,
        RequisitionListDialogComponent,
        ProjectsubEditExtendComponent,
        ColorEditExtendComponent,
        RequirePaintingBydetailViewComponent
    ],
})
export class DialogsModule { }
