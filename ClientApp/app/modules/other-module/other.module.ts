import { NgModule } from "@angular/core";
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
    SurfaceMasterComponent, SurfaceViewComponent,
    ProjectCenterComponent, ProjectEditComponent,
    ProjectMasterComponent, ProjectsubEditComponent,
    ProjectViewComponent
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
import { ProjectMasterService,ProjectMasterServiceCommunicate } from "../../services/project/project-master.service";
import { ProjectSubService } from "../../services/project/project-sub.service";
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
        SurfaceViewComponent,
        ProjectCenterComponent,
        ProjectEditComponent,
        ProjectMasterComponent,
        ProjectsubEditComponent,
        ProjectViewComponent
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
        SurfaceTypeServiceCommunicate,
        ProjectMasterService,
        ProjectMasterServiceCommunicate,
        ProjectSubService
        // dataTableServiceCommunicate
    ]
})
export class OtherModule {
}