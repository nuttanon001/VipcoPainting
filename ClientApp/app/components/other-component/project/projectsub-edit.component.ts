// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup } from "@angular/forms";
// models
import { ProjectSub } from "../../../models/model.index";
// services
import { ProjectSubService } from "../../../services/project/project-sub.service";

@Component({
    selector: "projectsub-edit",
    templateUrl: "./projectsub-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** projectsub-edit component*/
export class ProjectsubEditComponent implements OnInit  {
    /** projectsub-edit ctor */
    constructor(
        private service: ProjectSubService,
        private fb: FormBuilder
    ) { }
    // Parameter
    @Input("editValue") editValue: ProjectSub;
    @Output("complateValue") complateValue = new EventEmitter<ProjectSub | undefined>();
    editValueForm: FormGroup;

    tempAutoComplates: Array<string>;
    AutoComplates: Array<string>;
    // on Init
    ngOnInit(): void {
        // Get AutoComplate
        if (!this.tempAutoComplates) {
            this.tempAutoComplates = new Array;
            this.service.getAutoComplate()
                .subscribe(dbProjectSubCode => {
                    this.tempAutoComplates = dbProjectSubCode;
                });
        }
        if (!this.AutoComplates) {
            this.AutoComplates = new Array;
        }
        // Create Form
        this.editValueForm = this.fb.group({
            ProjectCodeSubId: [this.editValue.ProjectCodeSubId],
            Code: [this.editValue.Code,
                [
                    Validators.required,
                ]
            ],
            Name: [this.editValue.Name,
                [
                    Validators.maxLength(200)
                ]
            ],
            ProjectSubParentId: [this.editValue.ProjectSubParentId],
            ProjectCodeMasterId: [this.editValue.ProjectCodeMasterId],
            ProjectMasterString: [this.editValue.ProjectMasterString],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
        });
    }
    // emit value to Parent or undefined to Parent
    onComplateOrCancel(complate: boolean): void {
        if (complate) {
            if (this.editValueForm) {
                if (this.editValueForm.valid) {
                    this.complateValue.emit(this.editValueForm.value);
                }
            }
        } else {
            this.complateValue.emit(undefined);
        }
    }

    // on search autocomplate
    onSearchAutoComplate(event: any): void {
        this.AutoComplates = new Array;

        for (let i: number = 0; i < this.tempAutoComplates.length; i++) {
            let autoComplate: string = this.tempAutoComplates[i];

            if (autoComplate.toLowerCase().indexOf(event.query.toLowerCase()) === 0) {
                this.AutoComplates.push(autoComplate);
            }
        }
    }
}