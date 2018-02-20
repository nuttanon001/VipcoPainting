// angular core
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { RequirePaintMaster, RequirePaintList, AttachFile } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// service
import { RequirePaintListService,RequirePaintListServiceCommunicate } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";
import { DialogsService } from "../../../services/dialog/dialogs.service"
import { AuthService } from "../../../services/auth/auth.service";

@Component({
    selector: "require-painting-list-byinitial-edit",
    templateUrl: "./require-painting-list-byinitial-edit.component.html",
    styleUrls: [
        "../../../styles/edit.style.scss",
        "../../base-component/data-table.style.scss"
    ],
})

/** require-painting-list-byinitial-edit component*/
export class RequirePaintingListByinitialEditComponent 
    extends BaseEditComponent<RequirePaintList, RequirePaintListService> {
    /** require-painting-list-byinitial-edit ctor */
    constructor(
        service: RequirePaintListService,
        serviceCom: RequirePaintListServiceCommunicate,
        private serviceRequireMaster: RequirePaintMasterService,
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
    // Value
    requirePaintMaster: RequirePaintMaster;
    attachFiles: Array<AttachFile> = new Array;
    needInt: boolean;
    needExt: boolean;

    // on get data by key
    onGetDataByKey(value?: RequirePaintList): void {
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingListId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    if (this.editValue.RequirePaintingMasterId) {
                        this.serviceRequireMaster.getOneKeyNumber(this.editValue.RequirePaintingMasterId)
                            .subscribe(dbRequireMaster => this.requirePaintMaster = dbRequireMaster);
                    }
                    // set Date
                }, error => console.error(error), () => this.defineData());
        } 
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            RequirePaintingListId: [this.editValue.RequirePaintingListId],
            RequirePaintingListStatus: [this.editValue.RequirePaintingListStatus],
            Description: [this.editValue.Description,
                [
                    Validators.required,
                    Validators.maxLength(250)
                ]
            ],
            PartNo: [this.editValue.PartNo],
            MarkNo: [this.editValue.MarkNo,
                [
                    Validators.required
                ]
            ],
            DrawingNo: [this.editValue.DrawingNo],
            UnitNo: [this.editValue.UnitNo],
            Quantity: [this.editValue.Quantity,
                [
                    Validators.required,
                    Validators.min(0)
                ]
            ],
            FieldWeld: [this.editValue.FieldWeld],
            Insulation: [this.editValue.Insulation],
            ITP: [this.editValue.ITP],
            SizeL: [this.editValue.SizeL],
            SizeW: [this.editValue.SizeW],
            SizeH: [this.editValue.SizeH],
            Weight: [this.editValue.Weight],
            PlanStart: [this.editValue.PlanStart],
            PlanEnd: [this.editValue.PlanEnd],
            SendWorkItem: [this.editValue.SendWorkItem,
                [
                    Validators.required
                ]
            ],

            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //FK
            RequirePaintingMasterId: [this.editValue.RequirePaintingMasterId],
            // ViewModel
            IsReceive: [this.editValue.IsReceive],
            IntArea: [this.editValue.IntArea],
            ExtArea: [this.editValue.ExtArea],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();

        // change validity control
        const QuantityControl: AbstractControl | null = this.editValueForm.get("Quantity");
        if (QuantityControl) {
            QuantityControl.valueChanges.subscribe((Qty: number) => {
                const SizeLControl: AbstractControl | null = this.editValueForm.get("SizeL");
                const SizeWControl: AbstractControl | null = this.editValueForm.get("SizeW");
                const SizeHControl: AbstractControl | null = this.editValueForm.get("SizeH");

                if (SizeLControl && SizeWControl && SizeHControl) {
                    if (Qty <= 1) {
                        SizeLControl.setValidators([
                            Validators.required,
                            Validators.min(1)
                        ]);
                        SizeWControl.setValidators([
                            Validators.required,
                            Validators.min(1)
                        ]);
                        SizeHControl.setValidators([
                            Validators.required,
                            Validators.min(1)
                        ]);
                    } else {
                        SizeLControl.setValidators([]);
                        SizeWControl.setValidators([]);
                        SizeHControl.setValidators([]);
                    }
                    SizeLControl.updateValueAndValidity();
                    SizeWControl.updateValueAndValidity();
                    SizeHControl.updateValueAndValidity();
                }
            });
        }

        // Require some Area
        const IntControl: AbstractControl | null = this.editValueForm.get("IntArea");
        const ExtControl: AbstractControl | null = this.editValueForm.get("ExtArea");

        if (this.editValue.ExtArea) {
            this.needExt = true;
            if (ExtControl) {
                ExtControl.setValidators([
                    Validators.required,
                    Validators.min(1)
                ]);
            }
        } else {
            this.needExt = false;
        }

        if (this.editValue.IntArea) {
            this.needInt = true;
            if (IntControl) {
                IntControl.setValidators([
                    Validators.required,
                    Validators.min(1)
                ]);
            }
        } else {
            this.needInt = false;
        }
    }

    ////////////
    // Module //
    ////////////

    // get attact file
    getAttach(): void {
        if (this.editValue && this.editValue.RequirePaintingListId > 0) {
            this.service.getAttachFile(this.editValue.RequirePaintingListId)
                .subscribe(dbAttach => {
                    this.attachFiles = dbAttach.slice();
                }, error => console.error(error));
        }
    }

    // on Attach Update List
    onUpdateAttachResults(results: FileList): void {
        // debug here
        // console.log("File: ", results);
        this.editValue.AttachFile = results;
        // debug here
        // console.log("Att File: ", this.editValue.AttachFile);
        this.editValueForm.patchValue({
            AttachFile: this.editValue.AttachFile
        });
        this.onValueChanged(undefined);
    }

    // on Attach delete file
    onDeleteAttachFile(attach: AttachFile): void {
        if (attach) {
            if (!this.editValue.RemoveAttach) {
                this.editValue.RemoveAttach = new Array;
            }

            // remove
            this.editValue.RemoveAttach.push(attach.AttachFileId);
            // debug here
            // console.log("Remove :",this.editValue.RemoveAttach);

            this.editValueForm.patchValue({
                RemoveAttach: this.editValue.RemoveAttach
            });
            let template: Array<AttachFile> =
                this.attachFiles.filter((e: AttachFile) => e.AttachFileId !== attach.AttachFileId);

            this.attachFiles = new Array();
            setTimeout(() => this.attachFiles = template.slice(), 50);

            this.onValueChanged(undefined);
        }
    }

    // open file attach
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }
}