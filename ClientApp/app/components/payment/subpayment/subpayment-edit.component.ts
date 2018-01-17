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

    CanLoadSubPayment: boolean;

    // on get data by key
    onGetDataByKey(value?: SubPaymentMaster): void {
        if (!this.subPaymentDetails) {
            this.subPaymentDetails = new Array;
        }

        if (value) {
            // edit can load sub payment
            this.CanLoadSubPayment = false;

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
            // edit can load sub payment
            this.CanLoadSubPayment = false;

            this.editValue = {
                SubPaymentMasterId: 0,
                SubPaymentMasterStatus:1,
                PaintTeamId: undefined,
                SubPaymentDate: new Date(),
                EndDate: new Date(),
                StartDate: new Date(),
            };
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();

        if (!this.paintTeams) {
            this.paintTeams = new Array;
            this.paintTeams.push({ label: "-", value: undefined });
            this.servicePaintTeam.getAll()
                .subscribe(dbPaintTeam => {
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
            SubPaymentDate: [this.editValue.SubPaymentDate,
                [
                    Validators.required
                ]
            ],
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
    }

    // on value of form change
    // Override
    onValueChanged(data?: any): void {
        if (!this.editValueForm) { return; }
        const form = this.editValueForm;
        const SubPayIdControl: AbstractControl | null = this.editValueForm.get("SubPaymentMasterId");
        const paintTeamControl: AbstractControl | null = this.editValueForm.get("PaintTeamId"); 
        const projectControl: AbstractControl | null = this.editValueForm.get("ProjectCodeMasterId"); 
        const StartDateControl: AbstractControl | null = this.editValueForm.get("StartDate"); 
        const EndDateControl: AbstractControl | null = this.editValueForm.get("EndDate"); 

        if (paintTeamControl && projectControl && SubPayIdControl && StartDateControl && EndDateControl) {
            if (SubPayIdControl.value < 1) {
                if (paintTeamControl.value && projectControl.value &&
                    StartDateControl.value && EndDateControl.value) {
                    this.CanLoadSubPayment = true;
                } else {
                    this.CanLoadSubPayment = false;
                }
            } else {
                this.CanLoadSubPayment = false;
            }
        } else {
            this.CanLoadSubPayment = false;
        }

        // on form valid or not
        this.onFormValid(form.valid);
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
                                ProjectCodeMasterId: project.ProjectCodeMasterId,
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
                                    item.CalcCost = (item.AreaWorkLoad || 0) * (item.CurrentCost || 0);
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

            subPaymentDetail.CalcCost = (subPaymentDetail.AreaWorkLoad || 0) * (subPaymentDetail.CurrentCost || 0);
            // cloning an object
            if (subPaymentDetail.AdditionCost) {
                subPaymentDetail.CalcCost = (subPaymentDetail.CalcCost || 0) + subPaymentDetail.AdditionCost;
            }

            if (subPaymentDetail.AdditionArea) {
                subPaymentDetail.CalcCost = (subPaymentDetail.CalcCost || 0) + (subPaymentDetail.AdditionArea * (subPaymentDetail.CurrentCost || 0));
            }

            this.editValue.SubPaymentDetails.push(Object.assign({}, subPaymentDetail));
            this.editValueForm.patchValue({
                SubPaymentDetails: this.editValue.SubPaymentDetails.slice(),
            });
        }
        this.subPaymentDetail = undefined;
    }
}