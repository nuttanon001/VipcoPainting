// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators, AbstractControl } from "@angular/forms";
import {
    trigger, state, style,
    animate, transition
} from "@angular/animations";
// models
import { SubPaymentMaster,SubPaymentDetail } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
import { TableColumn } from "@swimlane/ngx-datatable";
//vaildator
import { DateMaxValidator,DateMinValidator } from "../../validation/dateminmax-validator.function";
// pipe
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { SubpaymentMasterService,SubpaymentMasterServiceCommunicate } from "../../../services/payment/subpayment-master.service";
import { SubpaymentDetailService } from "../../../services/payment/subpayment-detail.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";

@Component({
    selector: "subpayment-edit",
    templateUrl: "./subpayment-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
    animations: [
        trigger("flyInOut", [
            state("in", style({ transform: "translateY(0)" })),
            transition("void => *", [
                style({ transform: "translateY(-100%)" }),
                animate(250)
            ]),
            transition("* => void", [
                animate("0.2s 0.1s ease-out", style({ opacity: 0, transform: "translateY(100%)" }))
            ])
        ])
    ]
})
/** subpayment-edit component*/
export class SubpaymentEditComponent 
    extends BaseEditComponent<SubPaymentMaster, SubpaymentMasterService> {
    /** subpayment-edit ctor */
    constructor(
        service: SubpaymentMasterService,
        serviceCom: SubpaymentMasterServiceCommunicate,
        private serviceSubpaymentDetail: SubpaymentDetailService,
        private servicePaintTeam:PaintTeamService,
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
    subPaymentDetails: Array<SubPaymentDetail>;
    subPaymentDetail: SubPaymentDetail | undefined;

    indexSubPayDetail: number;
    paintTeams: Array<SelectItem>;
    columns: Array<TableColumn> = [
        { prop: "PaymentDetailString", name: "PaymentInfo", flexGrow: 2 },
        { prop: "AreaWorkLoad", name: "Area", flexGrow: 1 },
        { prop: "CurrentCost", name: "Cost", flexGrow: 1 },
        { prop: "CalcCost", name: "Total", flexGrow: 1 },
    ];

    get CanLoadSubPayment(): boolean {
        if (this.editValue) {
            return this.editValue.SubPaymentMasterId > 0;
        }
        return false;
    }
    // on get data by key
    onGetDataByKey(value?: SubPaymentMaster): void {
        if (!this.subPaymentDetails) {
            this.subPaymentDetails = new Array;
        }

        if (value) {
            this.service.getOneKeyNumber(value.SubPaymentMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                    if (this.editValue.StartDate) {
                        this.editValue.StartDate = this.editValue.StartDate != null ?
                            new Date(this.editValue.StartDate) : new Date();
                    }

                    if (this.editValue.EndDate) {
                        this.editValue.EndDate = this.editValue.EndDate != null ?
                            new Date(this.editValue.EndDate) : new Date();
                    }

                    if (this.editValue.SubPaymentDate) {
                        this.editValue.SubPaymentDate = this.editValue.SubPaymentDate != null ?
                            new Date(this.editValue.SubPaymentDate) : new Date();
                    }
                    // get Detail
                    this.serviceSubpaymentDetail.getByMasterId(value.SubPaymentMasterId)
                        .subscribe(dbSubPaymentDetail => {
                            this.subPaymentDetails = dbSubPaymentDetail.slice();
                        });
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                SubPaymentMasterId: 0,
                StartDate: new Date,
                EndDate: new Date,
            };
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();

        if (!this.paintTeams) {
            this.paintTeams = new Array;
            this.servicePaintTeam.getAll()
                .subscribe(dbPaintTeam => {
                    this.paintTeams.push({ label: "-", value: undefined });
                    dbPaintTeam.forEach(item => {
                        if (item.TeamName) {
                            if (item.TeamName.indexOf("vipco") === -1 &&
                                item.TeamName.indexOf("VIPCO") === -1 &&
                                item.TeamName.indexOf("Vipco") === -1) {
                                this.paintTeams.push({ label: item.TeamName, value: item.PaintTeamId });
                            }
                        }
                    });
                });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            SubPaymentMasterId: [this.editValue.SubPaymentMasterId],
            SubPaymentNo: [this.editValue.SubPaymentNo],
            SubPaymentDate: [this.editValue.SubPaymentDate],
            StartDate: [this.editValue.StartDate,
                [
                    Validators.required
                ]
            ],
            EndDate: [this.editValue.EndDate,
                [
                    Validators.required
                ]
            ],
            SubPaymentMasterStatus: [this.editValue.SubPaymentMasterStatus],
            Remark: [this.editValue.Remark],
            EmpApproved1: [this.editValue.EmpApproved1],
            EmpApproved2: [this.editValue.EmpApproved2],
            //BaseModel
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //FK
            PrecedingSubPaymentId: [this.editValue.PrecedingSubPaymentId],
            ProjectCodeMasterId: [this.editValue.ProjectCodeMasterId],
            PaintTeamId: [this.editValue.PaintTeamId,
                [
                    Validators.required
                ]
            ],
            SubPaymentDetails: [this.editValue.SubPaymentDetails],
            //ViewModel
            EmpApproved1String: [this.editValue.EmpApproved1String,
                [
                    Validators.required
                ]
            ],
            EmpApproved2String: [this.editValue.EmpApproved2String,
                [
                    Validators.required
                ]
            ],
            ProjectCodeMasterString: [this.editValue.ProjectCodeMasterString,
                [
                    Validators.required
                ]
            ],
            PaintTeamString: [this.editValue.PaintTeamString],
            SubPaymentMasterStatusString: [this.editValue.SubPaymentMasterStatusString],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();

        const sDateControl: AbstractControl | null = this.editValueForm.get("StartDate");
        if (sDateControl) {
            sDateControl.valueChanges.subscribe((sDate: Date) => {
                const eDateControl: AbstractControl | null = this.editValueForm.get("EndDate");

                if (eDateControl) {
                    if (sDate) {
                        eDateControl.setValidators([
                            Validators.required,
                            DateMinValidator(sDate)
                        ]);
                    } else {
                        eDateControl.setValidators([
                            Validators.required
                        ]);
                    }
                    eDateControl.updateValueAndValidity();
                }
            });
        }

        const eDateControl: AbstractControl | null = this.editValueForm.get("EndDate");
        if (eDateControl) {
            eDateControl.valueChanges.subscribe((eDate: Date) => {
                const sDateControl: AbstractControl | null = this.editValueForm.get("StartDate");

                if (sDateControl) {
                    if (eDate) {
                        eDateControl.setValidators([
                            Validators.required,
                            DateMaxValidator(eDate)
                        ]);
                    } else {
                        sDateControl.setValidators([
                            Validators.required
                        ]);
                    }
                    sDateControl.updateValueAndValidity();
                }
            });
        }
    }

    // open dialog
    openDialog(type?: string): void {
        if (type) {
            if (type === "EmpApproved1") {
                this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        if (emp) {
                            this.editValueForm.patchValue({
                                EmpApproved1: emp.EmpCode,
                                EmpApproved1String: `คุณ${emp.NameThai}`,
                            });
                        }
                    });
            } else if (type === "EmpApproved2") {
                this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        if (emp) {
                            this.editValueForm.patchValue({
                                EmpApproved2: emp.EmpCode,
                                EmpApproved2String: `คุณ${emp.NameThai}`,
                            });
                        }
                    });
            } else if (type === "Project") {
                this.serviceDialogs.dialogSelectProject(this.viewContainerRef,1)
                    .subscribe(project => {
                        if (project) {
                            this.editValueForm.patchValue({
                                ProjectCodeMasterId: project.ProjectCodeSubId,
                                ProjectCodeMasterString: `${project.ProjectMasterString}`,
                            });
                        }
                    });
            }
        }
    }

    // get-subpayment detail
    getSubPaymentDetail(): void {
        // only new subpayment can get
        if (this.editValue.SubPaymentMasterId < 1) {
            if (this.editValueForm) {
                let value: SubPaymentMaster = this.editValueForm.value;

                if (value.PaintTeamId && value.ProjectCodeMasterId) {
                    this.service.postCalclateSubPaymentMaster(value)
                        .subscribe(newSubPaymentDetail => {
                            if (newSubPaymentDetail) {
                                if (!this.editValue.SubPaymentDetails) {
                                    this.editValue.SubPaymentDetails = new Array;
                                }
                                newSubPaymentDetail.forEach((item,index) => {
                                    item.PaymentDate = new Date;
                                });
                                this.editValue.SubPaymentDetails = newSubPaymentDetail.slice();
                                this.editValueForm.patchValue({
                                    SubPaymentDetails: this.editValue.SubPaymentDetails.slice(),
                                });
                            }
                        });
                    return;
                }
            }
        }
        this.serviceDialogs.context("Warning Message",
            "Select job-number and paint team befor get subpayment detail.", this.viewContainerRef);
    }

    //update subpayment-detail
    onEditSubPaymentDetail(subPaymentDetail?: SubPaymentDetail): void {
        if (subPaymentDetail) {
            if (this.editValue.SubPaymentDetails) {
                this.indexSubPayDetail = this.editValue.SubPaymentDetails.indexOf(subPaymentDetail);
            } else {
                this.indexSubPayDetail = -1;
            }
            this.subPaymentDetail = Object.assign({}, subPaymentDetail);
        }
    }

    // edit Detail
    onComplateOrCancel(subPaymentDetail?: SubPaymentDetail): void {
        if (!this.editValue.SubPaymentDetails) {
            this.editValue.SubPaymentDetails = new Array;
        }

        if (subPaymentDetail && this.editValue.SubPaymentDetails) {
            if (this.indexSubPayDetail > -1) {
                // remove item
                this.editValue.SubPaymentDetails.splice(this.indexSubPayDetail, 1);
            }
            // cloning an object
            this.editValue.SubPaymentDetails.push(Object.assign({}, subPaymentDetail));
            this.editValueForm.patchValue({
                SubPaymentDetails: this.editValue.SubPaymentDetails.slice(),
            });
        }
        this.subPaymentDetail = undefined;
    }
}