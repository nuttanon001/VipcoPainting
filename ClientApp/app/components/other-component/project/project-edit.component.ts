// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import {
    trigger, state, style,
    animate, transition
} from "@angular/animations";
// models
import { ProjectMaster,ProjectSub } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import {
    ProjectMasterService, ProjectMasterServiceCommunicate
} from "../../../services/project/project-master.service";
import { ProjectSubService } from "../../../services/project/project-sub.service";

@Component({
    selector: "project-edit",
    templateUrl: "./project-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
    animations: [
        trigger("flyInOut", [
            state("in", style({ transform: "translateX(0)" })),
            transition("void => *", [
                style({ transform: "translateX(-100%)" }),
                animate(250)
            ]),
            transition("* => void", [
                animate("0.2s 0.1s ease-out", style({ opacity: 0, transform: "translateX(100%)" }))
            ])
        ])
    ]
})
/** project-edit component*/
export class ProjectEditComponent 
    extends BaseEditComponent<ProjectMaster, ProjectMasterService> {
    /** project-edit ctor */
    constructor(
        service: ProjectMasterService,
        serviceCom: ProjectMasterServiceCommunicate,
        private serviceSub: ProjectSubService,
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
    index: number = -1;
    projectSub: ProjectSub|undefined;
    // on get data by key
    onGetDataByKey(value?: ProjectMaster): void {
        if (value) {
            this.service.getOneKeyNumber(value.ProjectCodeMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                    if (this.editValue.StartDate) {
                        this.editValue.StartDate = this.editValue.StartDate != null ?
                            new Date(this.editValue.StartDate) : new Date();
                    }
                    // set Date
                    if (this.editValue.EndDate) {
                        this.editValue.EndDate = this.editValue.EndDate != null ?
                            new Date(this.editValue.EndDate) : new Date();
                    }
                    // set ProjectSub
                    if (this.editValue.ProjectCodeMasterId) {
                        this.serviceSub.getByMasterId(this.editValue.ProjectCodeMasterId)
                            .subscribe(dbDetail => {
                                this.editValue.ProjectSubs = dbDetail.slice();
                                this.editValueForm.patchValue({
                                    ProjectSubs: this.editValue.ProjectSubs.slice(),
                                });
                            }, error => {
                                this.editValue.ProjectSubs = new Array;
                                this.editValueForm.patchValue({
                                    ProjectSubs: this.editValue.ProjectSubs.slice(),
                                });
                            });
                    }
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                ProjectCodeMasterId: 0,
                StartDate: new Date,
                ProjectSubs: new Array
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
            ProjectCodeMasterId: [this.editValue.ProjectCodeMasterId],
            ProjectCode: [this.editValue.ProjectCode,
                [
                    Validators.required,
                    Validators.maxLength(50),
                ]
            ],
            ProjectName: [this.editValue.ProjectName,
                [
                    Validators.maxLength(200),
                ]
            ],
            StartDate: [this.editValue.StartDate],
            EndDate: [this.editValue.EndDate],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            ProjectSubs: [this.editValue.ProjectSubs]
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }

    // new Detail
    onNewOrEditProjectSub(projectSub: ProjectSub): void {
        if (!projectSub) {
            projectSub = {
                ProjectCodeSubId: 0,
                ProjectSubStatus: 1,
                Name:"-"
            };
            this.index = -1;
        } else {
            this.index = this.editValue.ProjectSubs ? this.editValue.ProjectSubs.indexOf(projectSub) : -1;
        }
        this.projectSub = projectSub;
        // Disable Save Buttom
        this.communicateService.toParent([this.editValueForm.value, false]);
    }

    // remove Detail
    onRemoveDetail(projectSub?: ProjectSub): void {
        if (projectSub) {
            // console.log("Project:", projectSub.ProjectCodeSubId);

            if (projectSub.ProjectCodeSubId > 0) {
                // console.log("Number:", projectSub.ProjectCodeSubId);
                this.serviceSub.CanRemoveProjectSub(projectSub.ProjectCodeSubId)
                    .subscribe(result => {
                        if (this.editValue.ProjectSubs) {
                            let index = this.editValue.ProjectSubs.indexOf(projectSub);
                            this.editValue.ProjectSubs.splice(index, 1);
                        }
                    }, error => {
                        this.serviceDialogs.error("Warning Message", "Can't remove job-level2/3 if already use data !!!",
                        this.viewContainerRef);
                    });
            } else {
                // console.log("Error:", projectSub.ProjectCodeSubId);
                if (this.editValue.ProjectSubs) {
                    let index = this.editValue.ProjectSubs.indexOf(projectSub);
                    this.editValue.ProjectSubs.splice(index, 1);
                }
                //this.serviceDialogs.error("Warning Message", "Can't remove job-level2/3 if already use data !!!",
                //    this.viewContainerRef);
            }
        }
    }

    // on Complate or Cancel ProjectSub
    onProjectSubComplate(projectSub?: ProjectSub): void {
        if (projectSub && this.editValue.ProjectSubs) {
            if (this.index > -1) {
                // remove item
                this.editValue.ProjectSubs.splice(this.index, 1);
                this.index = -1;
            }

            // cloning an object
            this.editValue.ProjectSubs.push(Object.assign({}, projectSub));
            this.editValueForm.patchValue({
                ProjectSubs: this.editValue.ProjectSubs.slice(),
            });
        }
        this.projectSub = undefined;
    }
}