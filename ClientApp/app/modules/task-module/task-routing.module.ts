import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    BlastRoomCenterComponent, BlastRoomMasterComponent,
    PaintTeamCenterComponent, PaintTeamMasterComponent
} from "../../components/task-component/task.index";
// service
import { AuthGuard } from "../../services/auth/auth-guard.service";


const blastRoomRoutes: Routes = [
    {
        path: "blast-room",
        component: BlastRoomCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: BlastRoomMasterComponent,
            }
        ],
    },
    {
        path: "paint-team",
        component: PaintTeamCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: PaintTeamMasterComponent,
            }
        ],
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(blastRoomRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class TaskRoutingModule {
}