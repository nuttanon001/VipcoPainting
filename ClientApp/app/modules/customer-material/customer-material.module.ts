import { NgModule } from "@angular/core";
import {
    MatProgressSpinnerModule,
    MatButtonModule,
    MatCheckboxModule,
    MatProgressBarModule,
    MatTooltipModule,
    MatSidenavModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatTabsModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatStepperModule,
    MatSliderModule
} from "@angular/material";

import {
    DataTableModule,
    DialogModule,
    SharedModule,
    CalendarModule,
    DropdownModule,
    InputMaskModule,
    TreeModule,
    TreeTableModule,
    AccordionModule,
    AutoCompleteModule,
    RadioButtonModule,
    CheckboxModule,
} from "primeng/primeng";
// chart
import { ChartsModule } from "ng2-charts/ng2-charts";

import { AngularSplitModule } from "angular-split";
import { NgxDatatableModule } from "@swimlane/ngx-datatable";
// component
import { DataTableComponent } from "../../components/base-component/data-table.component";
import { SearchBoxComponent } from "../../components/base-component/search-box.component";
import { AttactFileComponent } from "../../components/base-component/attact-file.component";
import { AttachFileViewComponent } from "../../components/base-component/attach-file-view.component";
import { ReuseTableComponent } from "../../components/base-component/reuse-table.component";
import { BaseChartComponent } from "../../components/base-component/base-chart.component";
import { DateOnlyPipe } from "../../pipes/date-only.pipe";

@NgModule({
    declarations: [
        // component
        DataTableComponent,
        SearchBoxComponent,
        AttactFileComponent,
        ReuseTableComponent,
        BaseChartComponent,
        // pipe
        DateOnlyPipe,
    ],
    imports: [
        // material
        MatButtonModule,
        MatCheckboxModule,
        MatProgressBarModule,
        MatTooltipModule,
        MatSidenavModule,
        MatInputModule,
        MatIconModule,
        MatMenuModule,
        MatDialogModule,
        MatTabsModule,
        MatCardModule,
        MatProgressSpinnerModule,
        MatStepperModule,
        MatSliderModule,
        // angularSplit
        AngularSplitModule,
        // ngxDataTable
        NgxDatatableModule,
        // primeNg
        DataTableModule,
        DialogModule,
        SharedModule,
        CalendarModule,
        DropdownModule,
        InputMaskModule,
        TreeModule,
        TreeTableModule,
        AccordionModule,
        AutoCompleteModule,
        RadioButtonModule,
        CheckboxModule,
        // chart
        ChartsModule
    ],
    exports: [
        // material
        MatButtonModule,
        MatCheckboxModule,
        MatProgressBarModule,
        MatTooltipModule,
        MatSidenavModule,
        MatInputModule,
        MatIconModule,
        MatMenuModule,
        MatDialogModule,
        MatTabsModule,
        MatCardModule,
        MatProgressSpinnerModule,
        MatStepperModule,
        MatSliderModule,
        // angularSplit
        AngularSplitModule,
        // ngxDataTable
        NgxDatatableModule,
        // primeNg
        DataTableModule,
        DialogModule,
        SharedModule,
        CalendarModule,
        DropdownModule,
        InputMaskModule,
        TreeModule,
        TreeTableModule,
        AccordionModule,
        AutoCompleteModule,
        RadioButtonModule,
        CheckboxModule,
        // component
        SearchBoxComponent,
        DataTableComponent,
        AttactFileComponent,
        ReuseTableComponent,
        BaseChartComponent,
        // pipe
        DateOnlyPipe,
        // chart
        ChartsModule
    ],
    entryComponents: [
        SearchBoxComponent,
        DataTableComponent,
        AttactFileComponent,
        ReuseTableComponent,
        BaseChartComponent,
    ]
})
export class CustomMaterialModule { }