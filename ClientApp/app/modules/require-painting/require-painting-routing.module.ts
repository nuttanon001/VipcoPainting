import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    RequirePaintingCenterComponent, RequirePaintingMasterComponent,
    RequirePaintingScheduleComponent, InitialRequirePaintingCenterComponent,
    InitialRequirePaintingMasterComponent,
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
    },
    {
        path: "initial-painting",
        component: InitialRequirePaintingCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: InitialRequirePaintingMasterComponent,
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