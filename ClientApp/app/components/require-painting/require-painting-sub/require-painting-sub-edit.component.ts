// angular
import { Component, ViewContainerRef,Input,Output,OnInit } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { RequirePaintList, RequirePaintSub } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintSubService } from "../../../services/require-paint/require-paint-sub.service";
// 3rd party
import { SelectItem } from "primeng/primeng";
@Component({
    selector: "require-painting-sub-edit",
    templateUrl: "./require-painting-sub-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** require-painting-sub-edit component*/
export class RequirePaintingSubEditComponent implements OnInit{
    reqList: RequirePaintList;
    reqSub: RequirePaintSub;
    // Array
    paintingAreas: Array<SelectItem>;
    paintingTypes: Array<SelectItem>;
    /** require-painting-sub-edit ctor */
    constructor() { }

    ngOnInit(): void {
        this.defineData();
    }

    // define data for edit form
    defineData(): void {
        // this.buildForm();
        this.reqSub = { 
            RequirePaintingSubId: 0,
            RequirePaintingListId: this.reqList ? this.reqList.RequirePaintingListId : 0
        };

        // PaintingAreas ComboBox
        if (!this.paintingAreas) {
            this.paintingAreas = new Array;
            this.paintingAreas.push({ label: "Selected Item", value: undefined });
            this.paintingAreas.push({ label: "Internal", value: 1 });
            this.paintingAreas.push({ label: "External", value: 2 });
        }

        // PaintingTypes ComboBox
        if (!this.paintingTypes) {
            this.paintingTypes = new Array;
            this.paintingTypes.push({ label: "Selected Item", value: undefined });
            this.paintingTypes.push({ label: "Blasting", value: 1 });
            this.paintingTypes.push({ label: "PrimerCoat", value: 2 });
            this.paintingTypes.push({ label: "MidCoat", value: 3 });
            this.paintingTypes.push({ label: "IntermediateCoat", value: 4 });
            this.paintingTypes.push({ label: "TopCoat", value: 5 });
        }
    }
}