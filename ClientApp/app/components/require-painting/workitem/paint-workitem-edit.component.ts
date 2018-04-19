// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { PaintWorkItem } from "../../../models/model.index";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";

@Component({
    selector: "paint-workitem-edit",
    templateUrl: "./paint-workitem-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})

// paint-workitem-edit component*/
export class PaintWorkItemEditComponent implements OnInit {
    /** paint-workitem-edit ctor */
    constructor(
        private fb: FormBuilder,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef,
    ) { }
    // Parameter
    // Two way binding
    _paintWorkItem: PaintWorkItem;
    @Output() paintWorkItemChange = new EventEmitter<PaintWorkItem>();
    @Input() noRequest: boolean = false;
    @Input()
    get paintWorkItem(): PaintWorkItem {
        return this._paintWorkItem;
    }
    set paintWorkItem(data: PaintWorkItem) {
        this._paintWorkItem = data;
        this.paintWorkItemChange.emit(this._paintWorkItem);
    }
    // Output
    @Output() hasChange = new EventEmitter<boolean>();
    //@Output() intAreaChange = new EventEmitter<number | undefined>();
    //@Output() ExtAreaChange = new EventEmitter<number | undefined>();
    // FormGroup
    paintWorkForm: FormGroup | undefined;

    // OnInit
    ngOnInit(): void {
        this.buildForm();
    }

    // build Form
    buildForm(): void {
        //debug here
        //console.log("PaintWorkItem: BuildForm");

        this.paintWorkForm = this.fb.group({
            PaintWorkItemId: [this.paintWorkItem.PaintWorkItemId],
            PaintLevel: [this.paintWorkItem.PaintLevel],
            IntArea: [this.paintWorkItem.IntArea],
            IntDFTMin: [this.paintWorkItem.IntDFTMin],
            IntDFTMax: [this.paintWorkItem.IntDFTMax],
            IntCalcColorUsage: [this.paintWorkItem.IntCalcColorUsage],
            IntCalcStdUsage: [this.paintWorkItem.IntCalcStdUsage],

            ExtArea: [this.paintWorkItem.ExtArea],
            ExtDFTMin: [this.paintWorkItem.ExtDFTMin],
            ExtDFTMax: [this.paintWorkItem.ExtDFTMax],
            ExtCalcColorUsage: [this.paintWorkItem.ExtCalcColorUsage],
            ExtCalcStdUsage: [this.paintWorkItem.ExtCalcStdUsage],
            Creator: [this.paintWorkItem.Creator],
            CreateDate: [this.paintWorkItem.CreateDate],
            Modifyer: [this.paintWorkItem.Modifyer],
            ModifyDate: [this.paintWorkItem.ModifyDate],
            //FK
            IntColorItemId: [this.paintWorkItem.IntColorItemId],
            ExtColorItemId: [this.paintWorkItem.ExtColorItemId],
            StandradTimeIntId: [this.paintWorkItem.StandradTimeIntId],
            StandradTimeExtId: [this.paintWorkItem.StandradTimeExtId],
            RequirePaintingListId: [this.paintWorkItem.RequirePaintingListId],
            //ViewModel
            IntColorString: [this.paintWorkItem.IntColorString],
            ExtColorString: [this.paintWorkItem.ExtColorString],
            IntStandradTimeString: [this.paintWorkItem.IntStandradTimeString],
            ExtStandradTimeString: [this.paintWorkItem.ExtStandradTimeString],
            PaintLevelString: [this.paintWorkItem.PaintLevelString],
            IsValid: [this.paintWorkItem.IsValid],
        });
        this.paintWorkForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        this.onValueChanged();
        if (this.noRequest) {
            // change validity control
            const IntAreaControl: AbstractControl | null = this.paintWorkForm.get("IntArea");
            if (IntAreaControl) {
                IntAreaControl.valueChanges.subscribe((IntArea: number) => {
                    if (!this.paintWorkForm) { return; }

                    const IntDFTMinControl: AbstractControl | null = this.paintWorkForm.get("IntDFTMin");
                    const IntDFTMaxControl: AbstractControl | null = this.paintWorkForm.get("IntDFTMax");
                    const IntColorControl: AbstractControl | null = this.paintWorkForm.get("IntColorString");
                    const IntStandradControl: AbstractControl | null = this.paintWorkForm.get("IntStandradTimeString");

                    if (IntDFTMinControl && IntDFTMaxControl &&
                        IntColorControl && IntStandradControl) {
                        if (IntArea) {
                            IntDFTMinControl.setValidators([
                                Validators.required,
                                Validators.minLength(1)
                            ]);
                            IntDFTMaxControl.setValidators([
                                Validators.required,
                                Validators.minLength(1)
                            ]);
                            IntColorControl.setValidators([
                                Validators.required,
                            ]);
                            IntStandradControl.setValidators([
                                Validators.required,
                            ]);
                        } else {
                            IntDFTMinControl.setValidators([]);
                            IntDFTMaxControl.setValidators([]);
                            IntColorControl.setValidators([]);
                            IntStandradControl.setValidators([]);
                        }

                        IntDFTMinControl.updateValueAndValidity();
                        IntDFTMaxControl.updateValueAndValidity();
                        IntColorControl.updateValueAndValidity();
                        IntStandradControl.updateValueAndValidity();
                    }
                });
            }

            const ExtAreaControl: AbstractControl | null = this.paintWorkForm.get("ExtArea");
            if (ExtAreaControl) {
                ExtAreaControl.valueChanges.subscribe((ExtArea: number) => {
                    if (!this.paintWorkForm) { return; }
                    const ExtDFTMinControl: AbstractControl | null = this.paintWorkForm.get("ExtDFTMin");
                    const ExtDFTMaxControl: AbstractControl | null = this.paintWorkForm.get("ExtDFTMax");
                    const ExtColorControl: AbstractControl | null = this.paintWorkForm.get("ExtColorString");
                    const ExtStandradControl: AbstractControl | null = this.paintWorkForm.get("ExtStandradTimeString");

                    if (ExtDFTMinControl && ExtDFTMaxControl &&
                        ExtColorControl && ExtStandradControl) {
                        if (ExtArea) {
                            ExtDFTMinControl.setValidators([
                                Validators.required,
                                Validators.minLength(1)
                            ]);
                            ExtDFTMaxControl.setValidators([
                                Validators.required,
                                Validators.minLength(1)
                            ]);
                            ExtColorControl.setValidators([
                                Validators.required,
                            ]);
                            ExtStandradControl.setValidators([
                                Validators.required,
                            ]);
                        } else {
                            ExtDFTMinControl.setValidators([]);
                            ExtDFTMaxControl.setValidators([]);
                            ExtColorControl.setValidators([]);
                            ExtStandradControl.setValidators([]);
                        }

                        ExtDFTMinControl.updateValueAndValidity();
                        ExtDFTMaxControl.updateValueAndValidity();
                        ExtColorControl.updateValueAndValidity();
                        ExtStandradControl.updateValueAndValidity();
                    }
                });
            }
        }
    }

    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.paintWorkForm) { return; }
        const form = this.paintWorkForm;

        this.paintWorkItem = form.value;
        this.paintWorkItem.IsValid = form.valid;

        this.hasChange.emit(this.paintWorkItem.IsValid);
    }

    // on OpenDialogBox
    onOpenDialogBox(mode?: string): void {
        if (mode) {
            if (mode.indexOf("StandardTime") !== -1) {
                this.dialogService.dialogSelectStandradTime(this.viewContainerRef, { StandardTimeWithOut: 0, TypeStandardTime: 1 })
                    .subscribe(standardTime => {
                        if (this.paintWorkForm) {
                            if (mode.indexOf("Int") !== -1) {
                                this.paintWorkForm.patchValue({
                                    StandradTimeIntId: standardTime ? standardTime.StandradTimeId : undefined,
                                    IntStandradTimeString: standardTime ? standardTime.Description : undefined
                                });
                            } else {
                                this.paintWorkForm.patchValue({
                                    StandradTimeExtId: standardTime ? standardTime.StandradTimeId : undefined,
                                    ExtStandradTimeString: standardTime ? standardTime.Description : undefined
                                });
                            }
                        }
                    });
            } else if (mode.indexOf("Color") !== -1) {
                this.dialogService.dialogSelectColorItem(this.viewContainerRef)
                    .subscribe(color => {
                        if (this.paintWorkForm) {
                            if (mode.indexOf("Int") !== -1) {
                                this.paintWorkForm.patchValue({
                                    IntColorItemId: color ? color.ColorItemId : undefined,
                                    IntColorString: color ? color.ColorName : undefined
                                });
                            } else {
                                this.paintWorkForm.patchValue({
                                    ExtColorItemId: color ? color.ColorItemId : undefined,
                                    ExtColorString: color ? color.ColorName : undefined
                                });
                            }
                        }
                    });
            }
        }
    }
}