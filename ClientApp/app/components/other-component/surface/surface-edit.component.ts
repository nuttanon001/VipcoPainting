// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { SurfaceType } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import {
    SurfaceTypeService, SurfaceTypeServiceCommunicate
} from "../../../services/surface-type/surface-type.service";
// 3rdParty
import { SelectItem } from "primeng/primeng";
@Component({
    selector: "surface-edit",
    templateUrl: "./surface-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** surface-edit component*/
export class SurfaceEditComponent extends BaseEditComponent<SurfaceType, SurfaceTypeService> {
    /** surface-edit ctor */
    constructor(service: SurfaceTypeService,
        serviceCom: SurfaceTypeServiceCommunicate,
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

    // on get data by key
    onGetDataByKey(value?: SurfaceType): void {
        if (value) {
            this.service.getOneKeyNumber(value.SurfaceTypeId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                SurfaceTypeId: 0,
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
            SurfaceTypeId: [this.editValue.SurfaceTypeId],
            SurfaceCode: [this.editValue.SurfaceCode,
                [
                    Validators.required,
                    Validators.maxLength(50)
                ]
            ],
            SurfaceName: [this.editValue.SurfaceName,
                [
                    Validators.required,
                    Validators.maxLength(250)
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