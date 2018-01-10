// angular core
import { Component } from "@angular/core";
import { FormBuilder } from "@angular/forms";
// components
import { ProjectsubEditComponent } from "./projectsub-edit.component";
// services
import { ProjectSubService } from "../../../services/project/project-sub.service";

@Component({
    selector: "projectsub-edit-extend",
    templateUrl: "./projectsub-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** projectsub-edit-extend component*/
export class ProjectsubEditExtendComponent extends ProjectsubEditComponent {
    /** projectsub-edit-extend ctor */
    constructor(
        service: ProjectSubService,
        fb: FormBuilder
    ) {
        super(service,fb);
    }
}