import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    ColorCenterComponent, ColorMasterComponent,
    StandardtimeCenterComponent, StandardtimeMasterComponent,
    SurfaceCenterComponent, SurfaceMasterComponent,
    ProjectCenterComponent,ProjectMasterComponent
} from "../../components/other-component/orther.index";
    // service
import { AuthGuard } from "../../services/auth/auth-guard.service";

const otherRoutes: Routes = [
    {
        path: "color-item",
        component: ColorCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: ColorMasterComponent,
            }
        ],
    },
    {
        path: "standard-time",
        component: StandardtimeCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: StandardtimeMasterComponent,
            }
        ],
    },
    {
        path: "surface-type",
        component: SurfaceCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: SurfaceMasterComponent,
            }
        ],
    },
    {
        path: "project",
        component: ProjectCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: ProjectMasterComponent,
            }
        ],
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(otherRoutes)
    ],
    exports: [
        RouterModule
    ]})
export class OtherRoutingModule {
}