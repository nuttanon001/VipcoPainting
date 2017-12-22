// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { BlastRoom } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { BlastRoomService, BlastRoomServiceCommunicate } from "../../../services/task/blast-room.service";
// 3rd party
import { SelectItem } from "primeng/primeng";

@Component({
    selector: "blast-room-edit",
    templateUrl: "./blast-room-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** blast-room-edit component*/
export class BlastRoomEditComponent extends BaseEditComponent<BlastRoom, BlastRoomService> {
    /** blast-room-edit ctor */
    constructor(
        service: BlastRoomService,
        serviceCom: BlastRoomServiceCommunicate,
        private servicePaintTeam: PaintTeamService,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private serviceDialogs: DialogsService,
        private fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
        );
    }

    // Parameter
    paintTeams: Array<SelectItem>;

    // on get data by key
    onGetDataByKey(value?: BlastRoom): void {
        if (value) {
            this.service.getOneKeyNumber(value.BlastRoomId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                BlastRoomId: 0
            };

            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();

        if (!this.paintTeams) {
            this.servicePaintTeam.getAll()
                .subscribe(dbPaintTeam => {
                    this.paintTeams = new Array;
                    this.paintTeams.push({ label: "-", value: undefined });
                    for (let item of dbPaintTeam) {
                        this.paintTeams.push({ label: `${(item.TeamName || "")}`, value: item.PaintTeamId });
                    }
                });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            BlastRoomId: [this.editValue.BlastRoomId],
            BlastRoomName: [this.editValue.BlastRoomName,
                [
                    Validators.maxLength(50),
                    Validators.required,
                ]
            ],
            BlastRoomNumber: [this.editValue.BlastRoomNumber],
            Remark: [this.editValue.Remark,
                [
                    Validators.maxLength(200),
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //FK
            PaintTeamId: [this.editValue.PaintTeamId,
                [
                    Validators.required
                ]
            ],
            //ViewModel
            TeamBlastString: [this.editValue.TeamBlastString]
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }
}