﻿import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import {
    BlastRoomCenterComponent, BlastRoomMasterComponent,
    PaintTeamCenterComponent, PaintTeamMasterComponent,
    TaskCenterComponent, TaskMasterComponent,
    TaskScheduleComponent
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
    {
        path: "task",
        component: TaskCenterComponent,
        children: [
            {
                path: "task-master/:condition",
                component: TaskMasterComponent,
                canActivate: [AuthGuard],
            },
            {
                path: "task-master-schedule",
                component: TaskScheduleComponent,
            },
            {
                path: "task-master-schedule/:condition",
                component: TaskScheduleComponent,
            },
            {
                path: "",
                component: TaskMasterComponent,
                canActivate: [AuthGuard],
            }
        ]
    }
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