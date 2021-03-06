﻿// angular
import {
    Component, ViewContainerRef, OnInit,
    Input, Output, EventEmitter, OnChanges
} from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import {
    PaintWorkItem, BlastWorkItem, ListPaintBlastWorkItem
} from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { Calendar } from "primeng/components/calendar/calendar";
import { concat } from "rxjs/observable/concat";
import { reduce } from "rxjs/operator/reduce";

@Component({
    selector: "list-paint-blast-workitem",
    templateUrl: "./list-paint-blast-workitem.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** list-paint-blast-workitem component*/
export class ListPaintBlastWorkitemComponent implements OnInit, OnChanges {
    /** list-paint-blast-workitem ctor */
    constructor(
        private fb: FormBuilder
    ) { }

    // Parameter
    checkBoxs: Array<boolean>;
    layerPaints: Array<string>;
    _listPaintBlastWorks: ListPaintBlastWorkItem;
    @Input() noRequest: boolean = false;
    @Output() listPaintBlastWorksChange = new EventEmitter<ListPaintBlastWorkItem>();
    // Input - Output
    @Input()
    set ListPaintBlastWorks(value: ListPaintBlastWorkItem) {
        // debug here
        // console.log("set ListPaintBlastWorks");

        if (value !== this._listPaintBlastWorks) {
            this._listPaintBlastWorks = value;
            this.listPaintBlastWorksChange.emit(this._listPaintBlastWorks);
        }
    }
    get ListPaintBlastWorks(): ListPaintBlastWorkItem {
        return this._listPaintBlastWorks;
    }

    @Output() isValid = new EventEmitter<boolean>();
    @Output() paintCheckBox = new EventEmitter<Array<boolean>>();

    // On Intis
    ngOnInit(): void {
        /// Define array
        if (!this.layerPaints) {
            this.layerPaints = new Array;
        }
        if (!this.checkBoxs) {
            this.checkBoxs = new Array;
            // Blast
            this.checkBoxs.push(false);
            // PrimerCoat
            this.checkBoxs.push(false);
            // MidCoat
            this.checkBoxs.push(false);
            // IntermediateCoat
            this.checkBoxs.push(false);
            // TopCoat
            this.checkBoxs.push(false);
        }
    }

    // On Change
    ngOnChanges() {
        this.defineData();
    }

    // define parameter
    defineData(): void {
        // debug here
        //console.log("define Data !!!",);
        try {
            // Define BlastWorkItem
            if (this.ListPaintBlastWorks.BlastWorkItems) {
                if (this.ListPaintBlastWorks.BlastWorkItems.length > 0) {
                    if (this.ListPaintBlastWorks.BlastWorkItems[0]) {
                        this.checkBoxs[0] = true;
                        this.ListPaintBlastWorks.BlastWorkItems[0].IsValid = true;
                    }
                }
            }

            // Define PaintWorkItem
            if (this.ListPaintBlastWorks.PaintWorkItems) {
                this.layerPaints = new Array;
                // console.log("Layer", JSON.stringify(this.ListPaintBlastWorks.PaintWorkItems));

                if (this.ListPaintBlastWorks.PaintWorkItems.length > 0) {
                    let tempPaint: Array<PaintWorkItem> = new Array;

                    this.ListPaintBlastWorks.PaintWorkItems.forEach(item => {
                        if (item.PaintLevel) {
                            item.IsValid = true;
                            this.checkBoxs[item.PaintLevel] = true;
                            tempPaint[item.PaintLevel - 1] = item;
                            if (item.PaintLevel === 1) {
                                this.layerPaints.push("PrimerCoat");
                            } else if (item.PaintLevel === 2) {
                                this.layerPaints.push("MidCoat");
                            } else if (item.PaintLevel === 3) {
                                this.layerPaints.push("IntermediateCoat");
                            } else {
                                this.layerPaints.push("TopCoat");
                            }
                        }
                    });

                    this.ListPaintBlastWorks.PaintWorkItems = tempPaint.slice();
                }
            }
        }
        catch(error){
            console.log("Error", error);
        }
    }

    // on Add level of paint
    onShowOrDidNotLevelOfPaint(checkBox: boolean, index: number, paintWorkItem: PaintWorkItem) {
        this.ListPaintBlastWorks.PaintWorkItems[index - 1] =
            this.ListPaintBlastWorks.PaintWorkItems[index - 1] ?
                (checkBox ? this.ListPaintBlastWorks.PaintWorkItems[index - 1] : paintWorkItem) : paintWorkItem;
        this.checkBoxs[index] = checkBox;
    }

    // on Change
    checkBoxChange(isChange?: boolean, levelPaint?: string): void {
        if (isChange !== undefined && levelPaint) {
            let paintWorkItem: PaintWorkItem = {
                PaintWorkItemId: 0,
                IsValid: false
            };

            if (levelPaint.indexOf("PrimerCoat") !== -1) {
                paintWorkItem.PaintLevel = 1;
                paintWorkItem.PaintLevelString = "PrimerCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 1, paintWorkItem);
            } else if (levelPaint.indexOf("MidCoat") !== -1) {
                paintWorkItem.PaintLevel = 2;
                paintWorkItem.PaintLevelString = "MidCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 2, paintWorkItem);
            } else if (levelPaint.indexOf("IntermediateCoat") !== -1) {
                paintWorkItem.PaintLevel = 3;
                paintWorkItem.PaintLevelString = "IntermediateCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 3, paintWorkItem);
            } else if (levelPaint.indexOf("TopCoat") !== -1) {
                paintWorkItem.PaintLevel = 4;
                paintWorkItem.PaintLevelString = "TopCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 4, paintWorkItem);
            } else if (levelPaint.indexOf("BlastWork") !== -1) {
                if (!this.ListPaintBlastWorks.BlastWorkItems[0] || isChange === false) {
                    this.ListPaintBlastWorks.BlastWorkItems.push({
                        BlastWorkItemId: 0,
                    });
                }
                this.checkBoxs[0] = isChange;
            }

            // Update CheckBox List to Remove Item
            this.onHasChange(isChange);
        }
    }

    // on Has-Change
    onHasChange(hasChange: boolean) {
        if (this.ListPaintBlastWorks) {
            if (this.ListPaintBlastWorks.PaintWorkItems) {
                this.ListPaintBlastWorks.PaintWorkItems.forEach((item) => {
                    if (item) {
                        if (!item.IsValid) {
                            this.isValid.emit(false);
                            this.paintCheckBox.emit(this.checkBoxs);
                            return;
                        }
                    }
                });
            }

            if (this.ListPaintBlastWorks.BlastWorkItems) {
                this.ListPaintBlastWorks.BlastWorkItems.forEach((item) => {
                    if (item) {
                        if (!item.IsValid) {
                            this.isValid.emit(false);
                            this.paintCheckBox.emit(this.checkBoxs);
                            return;
                        }
                            
                    }
                });
            }

            // debug here
            // console.log("onHasChange_IsValid", hasChange);
            this.isValid.emit(true);
           
            // console.log("checkBox",JSON.stringify(this.checkBoxs));
        }
    }
}