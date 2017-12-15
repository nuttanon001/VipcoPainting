import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { RequirePaintingCenterComponent } from "../../components/require-painting/require-painting-master/require-painting-center.component";
import { RequirePaintingMasterComponent } from "../../components/require-painting/require-painting-master/require-painting-master.component";

// service
import { AuthGuard } from "../../services/auth/auth-guard.service";

const requirePaintingRoutes: Routes = [
    {
        path: "require-painting",
        component: RequirePaintingCenterComponent,
        canActivate: [AuthGuard],
        children: [
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