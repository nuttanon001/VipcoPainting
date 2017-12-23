import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    RequirePaintingCenterComponent, RequirePaintingMasterComponent,
    RequirePaintingScheduleComponent
} from "../../components/require-painting/require.index";

// service
import { AuthGuard } from "../../services/auth/auth-guard.service";

const requirePaintingRoutes: Routes = [
    {
        path: "require-painting",
        component: RequirePaintingCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "require-schedule",
                component: RequirePaintingScheduleComponent,
            },
            {
                path: "",
                component: RequirePaintingMasterComponent,
            }
        ],
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(requirePaintingRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class RequirePaintingRoutingModule { }