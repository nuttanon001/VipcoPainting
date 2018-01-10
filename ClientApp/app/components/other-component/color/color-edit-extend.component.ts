// angular core
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder } from "@angular/forms";
// components
import { ColorEditComponent } from "./color-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { ColorService, ColorServiceCommunicate } from "../../../services/color/color.service";

@Component({
    selector: "color-edit-extend",
    templateUrl: "./color-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** color-edit-extend component*/
export class ColorEditExtendComponent extends ColorEditComponent {
    /** color-edit-extend ctor */
    constructor(
        service: ColorService,
        serviceCom: ColorServiceCommunicate,
        serviceAuth: AuthService,
        viewContainerRef: ViewContainerRef,
        serviceDialogs: DialogsService,
        fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
            serviceAuth,
            viewContainerRef,
            serviceDialogs,
            fb
        );
    }
}