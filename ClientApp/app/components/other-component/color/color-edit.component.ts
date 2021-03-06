﻿// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { Color } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { ColorService, ColorServiceCommunicate } from "../../../services/color/color.service";

@Component({
    selector: "color-edit",
    templateUrl: "./color-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** color-edit component*/
export class ColorEditComponent 
    extends BaseEditComponent<Color, ColorService> {
    /** color-edit ctor */
    constructor(
        service: ColorService,
        serviceCom: ColorServiceCommunicate,
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
    tempAutoComplates: Array<string>;
    AutoComplates: Array<string>;

    // on get data by key
    onGetDataByKey(value?: Color): void {
        if (value) {
            this.service.getOneKeyNumber(value.ColorItemId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                ColorItemId:0
            };
           
            this.defineData();
        }

        // Get AutoComplate
        if (!this.tempAutoComplates) {
            this.tempAutoComplates = new Array;
            this.service.getAutoComplate()
                .subscribe(dbColorItem => {
                    this.tempAutoComplates = dbColorItem;
                });
        }
        if (!this.AutoComplates) {
            this.AutoComplates = new Array;
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            ColorItemId: [this.editValue.ColorItemId],
            ColorCode: [this.editValue.ColorCode,
                [
                    Validators.maxLength(150)
                ]
            ],
            ColorName: [this.editValue.ColorName,
                [
                    Validators.required,
                    Validators.maxLength(150)
                ]
            ],
            VolumeSolids: [this.editValue.VolumeSolids,
                [
                    Validators.min(1),
                    Validators.max(100),
                    Validators.required
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            // ViewModel
            VolumeSolidsString: [this.editValue.VolumeSolidsString]
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
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