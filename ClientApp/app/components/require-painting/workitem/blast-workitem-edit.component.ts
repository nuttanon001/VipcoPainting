// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { BlastWorkItem } from "../../../models/model.index";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";

@Component({
    selector: "blast-workitem-edit",
    templateUrl: "./blast-workitem-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})

// blast-workitem-edit component*/
export class BlastWorkitemEditComponent implements OnInit {
    /** blast-workitem-edit ctor */
    constructor(
        private fb: FormBuilder,
        private dialogService: DialogsService,
        private viewContainerRef:ViewContainerRef,
    ) { }

    // Parameter
    // Two way binding
    _blastWorkItem: BlastWorkItem;
    @Output() blastWorkItemChange = new EventEmitter<BlastWorkItem>();
    @Input()
    get blastWorkItem(): BlastWorkItem {
        return this._blastWorkItem;
    }
    set blastWorkItem(data: BlastWorkItem) {
        this._blastWorkItem = data;
        this.blastWorkItemChange.emit(this._blastWorkItem);
    }
    // Output
    @Output() hasChange = new EventEmitter<boolean>();
    // FormGroup
    blastWorkForm: FormGroup;
    
    // OnInit
    ngOnInit(): void {
        this.buildForm();
    }

    // build Form
    buildForm(): void {
        this.blastWorkForm = this.fb.group({
            BlastWorkItemId: [this.blastWorkItem.BlastWorkItemId],
            IntArea: [this.blastWorkItem.IntArea],
            IntCalcStdUsage: [this.blastWorkItem.IntCalcStdUsage],
            ExtArea: [this.blastWorkItem.ExtArea],
            ExtCalcStdUsage: [this.blastWorkItem.ExtCalcStdUsage],
            Creator: [this.blastWorkItem.Creator],
            CreateDate: [this.blastWorkItem.CreateDate],
            Modifyer: [this.blastWorkItem.Modifyer],
            ModifyDate: [this.blastWorkItem.ModifyDate],
            //FK
            SurfaceTypeIntId: [this.blastWorkItem.SurfaceTypeIntId],
            SurfaceTypeExtId: [this.blastWorkItem.SurfaceTypeExtId],
            StandradTimeIntId: [this.blastWorkItem.StandradTimeIntId],
            StandradTimeExtId: [this.blastWorkItem.StandradTimeExtId],
            RequirePaintingListId: [this.blastWorkItem.RequirePaintingListId],
            //ViewModel
            IntSurfaceTypeString: [this.blastWorkItem.IntSurfaceTypeString],
            ExtSurfaceTypeString: [this.blastWorkItem.ExtSurfaceTypeString],
            IntStandradTimeString: [this.blastWorkItem.IntStandradTimeString],
            ExtStandradTimeString: [this.blastWorkItem.ExtStandradTimeString],
            IsValid:[this.blastWorkItem.IsValid]
        });
        this.blastWorkForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        this.onValueChanged();
        // change validity control
        const IntAreaControl: AbstractControl | null = this.blastWorkForm.get("IntArea");
        if (IntAreaControl) {
            IntAreaControl.valueChanges.subscribe((IntArea: number) => {
                const IntSurfaceControl: AbstractControl | null = this.blastWorkForm.get("IntSurfaceTypeString");
                const INtStandradControl: AbstractControl | null = this.blastWorkForm.get("IntStandradTimeString");

                if (IntSurfaceControl && INtStandradControl) {
                    if (IntArea) {
                        IntSurfaceControl.setValidators([
                            Validators.required,
                        ]);
                        INtStandradControl.setValidators([
                            Validators.required,
                        ]);
                    } else {
                        IntSurfaceControl.setValidators([]);
                        INtStandradControl.setValidators([]);
                    }
                    IntSurfaceControl.updateValueAndValidity();
                    INtStandradControl.updateValueAndValidity();
                }
            });
        }

        const ExtAreaControl: AbstractControl | null = this.blastWorkForm.get("ExtArea");
        if (ExtAreaControl) {
            ExtAreaControl.valueChanges.subscribe((ExtArea: number) => {
                const ExtSurfaceControl: AbstractControl | null = this.blastWorkForm.get("ExtSurfaceTypeString");
                const ExtStandradControl: AbstractControl | null = this.blastWorkForm.get("ExtStandradTimeString");

                if (ExtSurfaceControl && ExtStandradControl) {
                    if (ExtArea) {
                        ExtSurfaceControl.setValidators([
                            Validators.required,
                        ]);
                        ExtStandradControl.setValidators([
                            Validators.required,
                        ]);
                    } else {
                        ExtSurfaceControl.setValidators([]);
                        ExtStandradControl.setValidators([]);
                    }
                    ExtSurfaceControl.updateValueAndValidity();
                    ExtStandradControl.updateValueAndValidity();
                }
            });
        }
    }

    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.blastWorkForm) { return; }
        const form = this.blastWorkForm;

        this.blastWorkItem = form.value;
        this.blastWorkItem.IsValid = form.valid;

        this.hasChange.emit(this.blastWorkItem.IsValid);
    }

    // on OpenDialogBox
    onOpenDialogBox(mode?: string): void {
        if (mode) {
            if (mode.indexOf("StandardTime") !== -1) {
                this.dialogService.dialogSelectStandradTime(this.viewContainerRef, 2)
                    .subscribe(standardTime => {
                        if (mode.indexOf("Int") !== -1) {
                            this.blastWorkForm.patchValue({
                                StandradTimeIntId: standardTime ? standardTime.StandradTimeId : undefined,
                                IntStandradTimeString: standardTime ? standardTime.Description : undefined
                            });
                        } else {
                            this.blastWorkForm.patchValue({
                                StandradTimeExtId: standardTime ? standardTime.StandradTimeId : undefined,
                                ExtStandradTimeString: standardTime ? standardTime.Description : undefined
                            });
                        }
                    });
            } else if (mode.indexOf("SurfaceType") !== -1) {
                this.dialogService.dialogSelectSurfaceType(this.viewContainerRef)
                    .subscribe(surface => {
                        if (mode.indexOf("Int") !== -1) {
                            this.blastWorkForm.patchValue({
                                SurfaceTypeIntId: surface ? surface.SurfaceTypeId : undefined,
                                IntSurfaceTypeString: surface ? surface.SurfaceName : undefined
                            });
                        } else {
                            this.blastWorkForm.patchValue({
                                SurfaceTypeExtId: surface ? surface.SurfaceTypeId : undefined,
                                ExtSurfaceTypeString: surface ? surface.SurfaceName : undefined
                            });
                        }
                    });
            }
        }
    }
}