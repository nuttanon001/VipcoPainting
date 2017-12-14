import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import "rxjs/Rx";
import "hammerjs";
// services
import { DialogsService } from "../../services/dialog/dialogs.service";
// components
import {
    ConfirmDialog, ContextDialog,
    ErrorDialog,
    ColorItemDialogComponent,
    StandradTimeDialogComponent,
    SurfaceTypeDialogComponent,
    EmployeeDialogComponent,
    ProjectDialogComponent
} from "../../components/dialog/dialog.index";
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
        ProjectDialogComponent
    ],
    declarations: [
        ErrorDialog,
        ConfirmDialog,
        ContextDialog,
        ColorItemDialogComponent,
        StandradTimeDialogComponent,
        SurfaceTypeDialogComponent,
        EmployeeDialogComponent,
        ProjectDialogComponent
    ],
    providers: [
        DialogsService,
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
        ProjectDialogComponent
    ],
})
export class DialogsModule { }
