// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { PaintTeam } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService, PaintTeamServiceCommunicate } from "../../../services/task/paint-team.service";

@Component({
    selector: 'paint-team-edit',
    templateUrl: './paint-team-edit.component.html',
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** paint-team-edit component*/
export class PaintTeamEditComponent extends BaseEditComponent<PaintTeam, PaintTeamService> {
    /** paint-team-edit ctor */
    constructor(
        service: PaintTeamService,
        serviceCom: PaintTeamServiceCommunicate,
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
    onGetDataByKey(value?: PaintTeam): void {
        if (value) {
            this.service.getOneKeyNumber(value.PaintTeamId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                PaintTeamId: 0
            };

            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            PaintTeamId: [this.editValue.PaintTeamId],
            TeamName: [this.editValue.TeamName,
                [
                    Validators.maxLength(50),
                    Validators.required,
                ]
            ],
            Ramark: [this.editValue.Ramark,
                [
                    Validators.maxLength(200),
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }
}