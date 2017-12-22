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
    PaintTeamMasterComponent,PaintTeamViewComponent
} from "../../components/task-component/task.index";
// modules
import { TaskRoutingModule } from "./task-routing.module";
import {
    CustomMaterialModule, ValidationModule,
} from "../module.index";
// services
import { BlastRoomService,BlastRoomServiceCommunicate } from "../../services/task/blast-room.service";
import { PaintTeamService,PaintTeamServiceCommunicate } from "../../services/task/paint-team.service";

@NgModule({
    declarations: [
        BlastRoomCenterComponent,
        BlastRoomEditComponent,
        BlastRoomMasterComponent,
        BlastRoomViewComponent,
        PaintTeamCenterComponent,
        PaintTeamEditComponent,
        PaintTeamMasterComponent,
        PaintTeamViewComponent
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
        PaintTeamServiceCommunicate
        // dataTableServiceCommunicate
    ]
})
export class TaskModule {
}