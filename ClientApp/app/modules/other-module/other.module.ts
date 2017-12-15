﻿import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";
// component
import {
    ColorCenterComponent, ColorEditComponent,
    ColorMasterComponent, ColorViewComponent,
    StandardtimeCenterComponent, StandardtimeEditComponent,
    StandardtimeMasterComponent, StandardtimeViewComponent,
    SurfaceCenterComponent, SurfaceEditComponent,
    SurfaceMasterComponent, SurfaceViewComponent
} from "../../components/other-component/orther.index";

// modules
import { OtherRoutingModule } from "./other-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { ColorService, ColorServiceCommunicate } from "../../services/color/color.service";
import { StandradTimeService, StandradTimeServiceCommunicate } from "../../services/standrad-time/standrad-time.service";
import { SurfaceTypeService, SurfaceTypeServiceCommunicate } from "../../services/surface-type/surface-type.service";
@NgModule({
    declarations: [
        ColorCenterComponent,
        ColorMasterComponent,
        ColorViewComponent,
        ColorEditComponent,
        StandardtimeCenterComponent,
        StandardtimeEditComponent,
        StandardtimeMasterComponent,
        StandardtimeViewComponent,
        SurfaceCenterComponent,
        SurfaceEditComponent,
        SurfaceMasterComponent,
        SurfaceViewComponent
    ],
    imports: [
        // angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // custom
        CustomMaterialModule,
        ValidationModule,
        OtherRoutingModule,
    ],
    providers: [
        ColorService,
        ColorServiceCommunicate,
        StandradTimeService,
        StandradTimeServiceCommunicate,
        SurfaceTypeService,
        SurfaceTypeServiceCommunicate
        // dataTableServiceCommunicate
    ]
})
export class OtherModule {
}