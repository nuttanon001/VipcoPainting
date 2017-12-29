import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";
// component
import {
    BlastRoomCenterComponent, BlastRoomEditComponent,
    BlastRoomMasterComponent, BlastRoomViewComponent,
    PaintTeamCenterComponent, PaintTeamEditComponent,
    PaintTeamMasterComponent, PaintTeamViewComponent,
    TaskBlastEditComponent, TaskPaintEditComponent,
    TaskEditComponent, TaskMasterComponent,
    TaskCenterComponent, TaskViewComponent,
    TaskScheduleComponent,
    PaintTaskCenterComponent, PaintTaskDetailBlastComponent,
    PaintTaskDetailListComponent, PaintTaskDetailPaintComponent,
    PaintTaskEditComponent, PaintTaskMasterComponent,
    PaintTaskViewComponent
} from "../../components/task-component/task.index";
// modules
import { TaskRoutingModule } from "./task-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { TaskMasterService,TaskMasterServiceCommunicate } from "../../services/task/task-master.service";
import { BlastRoomService,BlastRoomServiceCommunicate } from "../../services/task/blast-room.service";
import { PaintTeamService, PaintTeamServiceCommunicate } from "../../services/task/paint-team.service";
import { TaskBlastDetailService } from "../../services/task/task-blast-detail.service";
import { TaskPaintDetailService } from "../../services/task/task-paint-detail.service";
import { RequirePaintMasterService } from "../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../services/require-paint/require-paint-list.service";
import { PaintTaskDetailService } from "../../services/paint-task/paint-task-detail.service";
import { PaintTaskMasterService,PaintTaskMasterServiceCommunicate } from "../../services/paint-task/paint-task-master.service";

@NgModule({
    declarations: [
        BlastRoomCenterComponent,
        BlastRoomEditComponent,
        BlastRoomMasterComponent,
        BlastRoomViewComponent,
        PaintTeamCenterComponent,
        PaintTeamEditComponent,
        PaintTeamMasterComponent,
        PaintTeamViewComponent,
        TaskBlastEditComponent,
        TaskPaintEditComponent,
        TaskEditComponent,
        TaskMasterComponent,
        TaskCenterComponent,
        TaskViewComponent,
        TaskScheduleComponent,
        PaintTaskCenterComponent,
        PaintTaskDetailBlastComponent,
        PaintTaskDetailListComponent,
        PaintTaskDetailPaintComponent,
        PaintTaskEditComponent,
        PaintTaskMasterComponent,
        PaintTaskViewComponent
    ],
    imports: [
        // angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // custom
        CustomMaterialModule,
        ValidationModule,
        TaskRoutingModule,
    ],
    providers: [
        BlastRoomService,
        BlastRoomServiceCommunicate,
        PaintTeamService,
        PaintTeamServiceCommunicate,
        TaskMasterService,
        TaskMasterServiceCommunicate,
        RequirePaintMasterService,
        RequirePaintListService,
        TaskBlastDetailService,
        TaskPaintDetailService,
        PaintTaskDetailService,
        PaintTaskMasterService,
        PaintTaskMasterServiceCommunicate
        // dataTableServiceCommunicate
    ]
})
export class TaskModule {
}