// angular
import { Component, ViewContainerRef, OnInit, OnChanges, Input, Output, EventEmitter, PACKAGE_ROOT_URL } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import {
    RequirePaintMaster, RequirePaintList,
    InitialRequirePaint, InitialRequireWorkItem,
    BlastWorkItem, PaintWorkItem, AttachFile
} from "../../../models/model.index";
// 3rd patry
import { Calendar } from "primeng/components/calendar/calendar";
// timezone
import * as moment from "moment-timezone";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";
import { InitialRequirePaintService } from "../../../services/require-paint/initial-require-paint.service";
import { retry } from "rxjs/operator/retry";

@Component({
    selector: "initial-require-workitem",
    templateUrl: "./initial-require-workitem.component.html",
    styleUrls: ["../../../styles/edit.style.scss",]
})

/** initial-require-painting-list component */
export class InitialRequirePaintingListComponent implements OnInit {
    /** initial-require-painting-list ctor */
    constructor(
        private service: InitialRequirePaintService,
        private serviceRequireMaster: RequirePaintMasterService,
        private serviceRequireList: RequirePaintListService,
        private serviceAuth: AuthService,
        private serviceDialogs: DialogsService,
        private viewContainerRef: ViewContainerRef,
        private fb: FormBuilder,
    ) { }

    // Parameter
    _initialRequirePaintId: number | undefined;
    @Input()
    set initialRequirePaintId(_value: number) {
        if (_value !== this._initialRequirePaintId) {
            this._initialRequirePaintId = _value;
            if (_value) {
                if (_value > 0) {
                    this.defineData();
                }
            }
        }
    }
    get initialRequirePaintId(): number {
        return this._initialRequirePaintId || 0;
    }
    @Output() ComplateOrFailed = new EventEmitter<boolean>();
    // Value
    initialRequirePaint: InitialRequirePaint | undefined;
    requirePaintMaster: RequirePaintMaster | undefined;
    attachFiles: FileList | undefined;
    // Form
    initialWorkItemForm: FormGroup | undefined;
    initialWorkItemValue: InitialRequireWorkItem | undefined;
    get disabledSave(): boolean {
        if (this.initialWorkItemForm) {
            if (this.initialWorkItemForm.valid) {
                return false;
            }
        }
        return true;
    }

    // Init by angular hook
    ngOnInit(): void {
    }

    // on DefineData
    defineData(): void {
        if (this.initialRequirePaintId) {
            this.service.getOneKeyNumber(this.initialRequirePaintId)
                .subscribe(dbData => {
                    if (dbData) {
                        this.initialRequirePaint = dbData;
                        if (this.initialRequirePaint.RequirePaintingMasterId) {
                            this.serviceRequireMaster.getOneKeyNumber(this.initialRequirePaint.RequirePaintingMasterId)
                                .subscribe(dbRequireMaster => {
                                    this.requirePaintMaster = dbRequireMaster;
                                });
                        }
                        this.initialWorkItemValue = {};
                        this.buildForm();
                    }
                });
        }
    }

    // build form
    buildForm(): void {
        if (!this.initialWorkItemValue) { return; }

        this.initialWorkItemForm = this.fb.group({
            ExtArea: [this.initialWorkItemValue.ExtArea],
            IntArea: [this.initialWorkItemValue.IntArea],
            Description: [this.initialWorkItemValue.Description,
                [
                    Validators.required,
                    Validators.maxLength(250)
                ]
            ],
            PartNo: [this.initialWorkItemValue.PartNo,
                [
                    Validators.maxLength(250)
                ]
            ],
            MarkNo: [this.initialWorkItemValue.MarkNo,
                [
                    Validators.required,
                    Validators.maxLength(250)
                ]
            ],
            Quantity: [this.initialWorkItemValue.Quantity,
                [
                    Validators.required,
                    Validators.min(0)
                ]
            ],
            FieldWeld: [this.initialWorkItemValue.FieldWeld],
            Insulation: [this.initialWorkItemValue.Insulation],
            ITP: [this.initialWorkItemValue.ITP],
            SizeL: [this.initialWorkItemValue.SizeL],
            SizeW: [this.initialWorkItemValue.SizeW],
            SizeH: [this.initialWorkItemValue.SizeH],
            Weight: [this.initialWorkItemValue.Weight],
            SendWorkItem: [this.initialWorkItemValue.SendWorkItem,
                [
                    Validators.required
                ]
            ]
        });
        // on form value change
        this.initialWorkItemForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));

        // change validity control
        const QuantityControl: AbstractControl | null = this.initialWorkItemForm.get("Quantity");
        if (QuantityControl) {
            QuantityControl.valueChanges.subscribe((Qty: number) => {

                if (!this.initialWorkItemForm) { return; }

                const SizeLControl: AbstractControl | null = this.initialWorkItemForm.get("SizeL");
                const SizeWControl: AbstractControl | null = this.initialWorkItemForm.get("SizeW");
                const SizeHControl: AbstractControl | null = this.initialWorkItemForm.get("SizeH");

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
        const IntControl: AbstractControl | null = this.initialWorkItemForm.get("IntArea");
        const ExtControl: AbstractControl | null = this.initialWorkItemForm.get("ExtArea");

        if (this.initialRequirePaint) {
            if (this.initialRequirePaint.NeedExternal) {
                if (ExtControl) {
                    ExtControl.setValidators([
                        Validators.required,
                        Validators.min(1)
                    ]);
                }
            }
            if (this.initialRequirePaint.NeedInternal) {
                if (IntControl) {
                    IntControl.setValidators([
                        Validators.required,
                        Validators.min(1)
                    ]);
                }
            }
        }

        //if (IntControl) {
        //    IntControl.valueChanges.subscribe((IntArea: number) => {
        //        const ExtControl: AbstractControl | null = this.initialWorkItemForm.get("ExtArea");
        //        if (ExtControl) {
        //            if (IntArea < 1) {
        //                ExtControl.setValidators([
        //                    Validators.required,
        //                    Validators.min(1)
        //                ]);
        //                IntControl.setValidators([
        //                    Validators.required,
        //                    Validators.min(1)
        //                ]);
        //            } else {
        //                ExtControl.setValidators([]);
        //            }
        //            IntControl.updateValueAndValidity();
        //            ExtControl.updateValueAndValidity();
        //        }
        //    });
        //}
    }

    // on form value change
    onValueChanged(data?: any): void {
        if (!this.initialWorkItemForm) { return; }

        if (this.initialWorkItemForm.valid) {
            this.initialWorkItemValue = this.initialWorkItemForm.value;
        }
    }

    // on change time zone befor update to webapi
    changeTimezone(value: RequirePaintList): RequirePaintList {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.PlanEnd !== null) {
                value.PlanEnd = moment.tz(value.PlanEnd, zone).toDate();
            }
            if (value.PlanStart !== null) {
                value.PlanStart = moment.tz(value.PlanStart, zone).toDate();
            }
            if (value.SendWorkItem !== null) {
                value.SendWorkItem = moment.tz(value.SendWorkItem, zone).toDate();
            }
        }
        return value;
    }

    // on submitted data
    onSubmitted(): void {
        if (!this.initialWorkItemForm) { return; }
        if (!this.initialWorkItemForm.valid) { return; }

        this.onInsertToDataBase(this.initialWorkItemForm.value);
    }

    // on cancel data
    onCancel(): void {
        this.ComplateOrFailed.emit(false);
    }

    // on submite data
    onInsertToDataBase(initialRequire: InitialRequireWorkItem): void {
        if (!this.initialRequirePaint) { return; }

        // Set initialRequire
        let newRequirePaintingList: RequirePaintList = {
            RequirePaintingListId: 0,
            RequirePaintingListStatus: 1,
            Description: initialRequire.Description,
            PartNo: initialRequire.PartNo,
            MarkNo: initialRequire.MarkNo,
            DrawingNo: this.initialRequirePaint.DrawingNo,
            UnitNo: this.initialRequirePaint.UnitNo,
            Quantity: initialRequire.Quantity,
            FieldWeld: initialRequire.FieldWeld,
            Insulation: initialRequire.Insulation,
            ITP: initialRequire.ITP,
            SizeL: initialRequire.SizeL,
            SizeW: initialRequire.SizeW,
            SizeH: initialRequire.SizeH,
            Weight: initialRequire.Weight,
            PlanStart: this.initialRequirePaint.PlanStart,
            PlanEnd: this.initialRequirePaint.PlanEnd,
            SendWorkItem: initialRequire.SendWorkItem,
            RequirePaintingMasterId: this.initialRequirePaint.RequirePaintingMasterId,
            BlastWorkItems: new Array,
            PaintWorkItems: new Array,
        };

        // Who Create
        if (this.serviceAuth.getAuth) {
            newRequirePaintingList.Creator = this.serviceAuth.getAuth.UserName || "";
        }

        // changetimezone
        newRequirePaintingList = this.changeTimezone(newRequirePaintingList);

        // InitialRequirePaint BlastWorkItem to RequirePaintingList
        if (this.initialRequirePaint) {
            if (this.initialRequirePaint.BlastWorkItems) {
                this.initialRequirePaint.BlastWorkItems.forEach((blastWork, index) => {
                    if (this.initialRequirePaint) {
                        if (newRequirePaintingList.BlastWorkItems) {
                            let newBlastWork: BlastWorkItem = {
                                BlastWorkItemId: 0,
                                IntArea: blastWork.StandradTimeIntId && this.initialRequirePaint.NeedInternal ? initialRequire.IntArea : undefined,
                                ExtArea: blastWork.StandradTimeExtId && this.initialRequirePaint.NeedExternal ? initialRequire.ExtArea : undefined,
                                // StandradTimeInt
                                StandradTimeIntId: blastWork.StandradTimeIntId,
                                // StandradTimeExt
                                StandradTimeExtId: blastWork.StandradTimeExtId,
                                // SurfaceTypeInt
                                SurfaceTypeIntId: blastWork.SurfaceTypeIntId,
                                // SurfaceTypeExt
                                SurfaceTypeExtId: blastWork.SurfaceTypeExtId,
                                // InitialRequire Don't add InitialRequireId
                                // InitialRequireId: blastWork.InitialRequireId
                            };
                            newRequirePaintingList.BlastWorkItems.push(newBlastWork);
                        }
                    }
                });
            }

            if (this.initialRequirePaint.PaintWorkItems) {
                // debug here
                // console.log("PaintWorkItems", JSON.stringify(this.initialRequirePaint.PaintWorkItems));

                this.initialRequirePaint.PaintWorkItems.forEach((paintWork, index) => {

                    // debug here
                    // console.log("paintWork", JSON.stringify(paintWork));

                    // can't update FromBody with same data from webapi try new object and send back update
                    if (this.initialRequirePaint) {
                        if (newRequirePaintingList.PaintWorkItems) {
                            let newPaintWork: PaintWorkItem = {
                                PaintWorkItemId: 0,
                                PaintLevel: paintWork.PaintLevel,

                                IntArea: paintWork.IntDFTMin && paintWork.IntDFTMax && this.initialRequirePaint.NeedInternal ? initialRequire.IntArea : undefined,
                                ExtArea: paintWork.ExtDFTMin && paintWork.ExtDFTMax && this.initialRequirePaint.NeedExternal ? initialRequire.ExtArea : undefined,

                                IntDFTMin: paintWork.IntDFTMin,
                                IntDFTMax: paintWork.IntDFTMax,
                                ExtDFTMin: paintWork.ExtDFTMin,
                                ExtDFTMax: paintWork.ExtDFTMax,
                                // ColorItemInt
                                IntColorItemId: paintWork.IntColorItemId,
                                // ColorItemExt
                                ExtColorItemId: paintWork.ExtColorItemId,
                                // StandradTimeInt
                                StandradTimeIntId: paintWork.StandradTimeIntId,
                                // StandradTimeExt
                                StandradTimeExtId: paintWork.StandradTimeExtId,
                                // InitialRequire Don't add InitialRequireId
                                // InitialRequireId: paintWork.InitialRequireId
                            };
                            // debug here
                            // console.log("newPaintWork", JSON.stringify(newPaintWork));
                            newRequirePaintingList.PaintWorkItems.push(newPaintWork);
                        }
                    }
                });
            }
        }

        // post to api
        this.serviceRequireList.postWithInitialRequire(newRequirePaintingList)
            .subscribe(complate => {
                if (complate) {
                    if (this.attachFiles) {
                        this.onAttactFileToDataBase(complate.RequirePaintingListId, this.attachFiles, complate.Creator || "Someone");
                    }

                    this.serviceDialogs.context("System message", "Save completed.", this.viewContainerRef)
                        .subscribe(result => {
                            this.ComplateOrFailed.emit(true);
                        });
                }
            }, error => {
                this.serviceDialogs.error("Failed !",
                    "Save failed with the following error: WorkItem has error !!!", this.viewContainerRef);
            });
    }

    ///
    // Module
    ///
    // on attact file
    onAttactFileToDataBase(RequirePaintingListId: number, Attacts: FileList,CreateBy:string): void {
        this.serviceRequireList.postAttactFile(RequirePaintingListId, Attacts, CreateBy)
            .subscribe(complate => console.log("Upload Complate"), error => console.error(error));
    }

    // on attach update List
    onUpdateAttachResults(results: FileList): void {
        this.attachFiles = results;
    }

    // open file attach
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }
}