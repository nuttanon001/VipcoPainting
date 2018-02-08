// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import {
    trigger, state, style,
    animate, transition
} from "@angular/animations";
// models
import { RequirePaintMaster, RequirePaintList, RequirePaintMasterHasList } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";

@Component({
    selector: "require-painting-edit",
    templateUrl: "./require-painting-edit.component.html",
    styleUrls: [
        "../../../styles/edit.style.scss",
        "../../base-component/data-table.style.scss"
    ],
    animations: [
        trigger("flyInOut", [
            state("in", style({ transform: "translateX(0)" })),
            transition("void => *", [
                style({ transform: "translateX(100%)" }),
                animate(400)
            ]),
            transition("* => void", [
                animate("0.5s 0.4s ease-out", style({ opacity: 0, transform: "translateX(100%)" }))
            ])
        ])
    ]
})
// require-painting-edit component*/
export class RequirePaintingEditComponent 
    extends BaseEditComponent<RequirePaintMaster, RequirePaintMasterService> {
    /** require-painting-edit ctor */
    constructor(
        service: RequirePaintMasterService,
        serviceCom: RequirePaintMasterServiceCommunicate,
        private blastService: BlastWorkitemService,
        private paintService: PaintWorkitemService,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private serviceList: RequirePaintListService,
        private serviceDialogs: DialogsService,
        private fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
        );
    }
    // Parameter
    levelPaints: Array<string>;
    requirePaintLists: Array<RequirePaintList>;
    newRequirePaintList: RequirePaintList | undefined;
    indexListItem: number;
    selectedIndex: number;
    maxDate:Date = new Date;
    readOnly: boolean;

    // on get data by key
    onGetDataByKey(value?: RequirePaintMaster): void {
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    this.readOnly = this.editValue.RequirePaintingStatus !== 1;
                    // set Date
                    if (this.editValue.FinishDate) {
                        this.editValue.FinishDate = this.editValue.FinishDate != null ?
                            new Date(this.editValue.FinishDate) : new Date();
                    }

                    if (this.editValue.ReceiveDate) {
                        this.editValue.ReceiveDate = this.editValue.ReceiveDate != null ?
                            new Date(this.editValue.ReceiveDate) : new Date();
                    }

                    if (this.editValue.RequireDate) {
                        this.editValue.RequireDate = this.editValue.RequireDate != null ?
                            new Date(this.editValue.RequireDate) : new Date();
                    }

                    if (this.editValue.RequirePaintingMasterId) {
                        this.serviceList.getByMasterId(this.editValue.RequirePaintingMasterId)
                            .subscribe(dbLists => {
                                this.requirePaintLists = dbLists.slice();
                                if (this.requirePaintLists) {
                                    this.requirePaintLists.forEach((item, index) => {
                                        // get BlastWorkItem
                                        this.blastService.getByMasterId(item.RequirePaintingListId)
                                            .subscribe(dbData => item.BlastWorkItems = dbData.slice());
                                        // get PaintWorkItem
                                        this.paintService.getByMasterId(item.RequirePaintingListId)
                                            .subscribe(dbData => item.PaintWorkItems = dbData.slice());
                                    });
                                }
                            });
                    }

                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                RequirePaintingMasterId: 0,
                RequirePaintingStatus : 1,
                RequireDate: new Date,
                ReceiveDate: new Date,
            };

            if (this.serviceAuth.getAuth) {
                this.editValue.RequireEmp = this.serviceAuth.getAuth.EmpCode || "";
                this.editValue.RequireString = this.serviceAuth.getAuth.NameThai || "";
            }

            this.readOnly = false;
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        if (!this.levelPaints) {
            this.levelPaints = new Array;
        }

        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            RequirePaintingMasterId: [this.editValue.RequirePaintingMasterId],
            ReceiveDate: [this.editValue.ReceiveDate],
            RequireDate: [this.editValue.RequireDate,
                [
                    Validators.required,
                ]
            ],
            FinishDate: [this.editValue.FinishDate],
            PaintingSchedule: [this.editValue.PaintingSchedule,
                [
                    Validators.maxLength(150)
                ]
            ],
            RequireNo: [this.editValue.RequireNo],
            RequirePaintingStatus: [this.editValue.RequirePaintingStatus],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            // FK
            RequireEmp: [this.editValue.RequireEmp],
            ReceiveEmp: [this.editValue.ReceiveEmp],
            ProjectCodeSubId: [this.editValue.ProjectCodeSubId],
            // ViewModel
            RequireString: [this.editValue.RequireString],
            ReceiveString: [this.editValue.ReceiveString],
            ProjectCodeSubString: [this.editValue.ProjectCodeSubString]
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }

    // open dialog
    openDialog(type?: string): void {
        if (this.readOnly) {
            this.serviceDialogs.error("Error Message",
                "This Requist-Painting was tasking. Only can input work item.",
                this.viewContainerRef);
            return;
        }

        if (type) {
            if (type === "Employee") {
                this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        if (emp) {
                            this.editValueForm.patchValue({
                                RequireEmp: emp.EmpCode,
                                RequireString: `คุณ${emp.NameThai}`,
                            });
                        }
                    });
            } else if (type === "Project") {
                this.serviceDialogs.dialogSelectProject(this.viewContainerRef,2)
                    .subscribe(project => {
                        if (project) {
                            this.editValueForm.patchValue({
                                ProjectCodeSubId: project.ProjectCodeSubId,
                                ProjectCodeSubString: `${project.ProjectMasterString}/${project.Code}`,
                            });
                        }
                    });
            }
        }
    }

    // add list item
    addOrEditListItem(listItem?: RequirePaintList, mode: number = 0): void {
        if (mode === 1) {
            if (listItem) {
                if (listItem.RequirePaintingListStatus === 1) {
                    listItem.RequirePaintingListStatus = 4;
                    this.onValueChanged();
                } else if (listItem.RequirePaintingListStatus !== 4) {
                    this.serviceDialogs.error("Error Message",
                        "This Requist-Painting-List was tasking. Can't cancel work item.",
                        this.viewContainerRef);
                    return;
                } else {
                    return;
                }
            }
        } else {
            if (listItem) {
                if (listItem.RequirePaintingListStatus === 1) {
                    if (this.requirePaintLists) {
                        this.indexListItem = this.requirePaintLists.indexOf(listItem);
                        // Set Date
                        if (listItem.PlanStart) {
                            listItem.PlanStart = listItem.PlanStart != null ?
                                new Date(listItem.PlanStart) : new Date();
                        }
                        if (listItem.PlanEnd) {
                            listItem.PlanEnd = listItem.PlanEnd != null ?
                                new Date(listItem.PlanEnd) : new Date();
                        }
                        if (listItem.SendWorkItem) {
                            listItem.SendWorkItem = listItem.SendWorkItem != null ?
                                new Date(listItem.SendWorkItem) : new Date();
                        }
                    } else {
                        this.indexListItem = -1;
                    }
                } else {
                    this.serviceDialogs.error("Error Message",
                        "This Requist-Painting-List was tasking. Can't edit work-item.",
                        this.viewContainerRef);
                    return;
                }
            } else {
                listItem = {
                    RequirePaintingListId: 0,
                    RequirePaintingListStatus: 1
                };
                this.indexListItem = -1;
            }
            this.newRequirePaintList = listItem;
            // set save buttom diable
            this.onValueChanged();
        }
    }

    // edit list item
    onComplateOrCancel(requirePaintList?: RequirePaintList): void {
        if (!this.requirePaintLists) {
            this.requirePaintLists = new Array;
        }

        if (requirePaintList) {
            if (this.indexListItem > -1) {
                // remove item
                this.requirePaintLists.splice(this.indexListItem, 1);
            }
            // cloning an object
            this.requirePaintLists.push(Object.assign({}, requirePaintList));
            this.onValueChanged();
        }
        this.newRequirePaintList = undefined;
        this.selectedIndex = 2;
        this.onValueChanged();
    }

    // on valid data
    onFormValid(isValid: boolean): void {
        if (!this.requirePaintLists) {
            isValid = false;
        } else if (this.requirePaintLists.length < 1) {
            isValid = false;
        } else if (this.newRequirePaintList) {
            isValid = false;
        }

        this.editValue = this.editValueForm.value;
        let editComplate: RequirePaintMasterHasList = {
            RequirePaintLists : this.requirePaintLists,
            RequirePaintMaster : this.editValue
        };
        this.communicateService.toParent([editComplate, isValid]);
    }

    // row class
    getRowClass(row?: any): any {
        if (row) {
            if (row["RequirePaintingListStatus"]) {
                console.log("Data is: RequirePaintingListStatus", row.RequirePaintingListStatus)
                if (row.RequirePaintingListStatus === 1) {
                    return { "is-require": true };
                } else if (row.RequirePaintingListStatus === 2) {
                    return { "is-wait": true };
                } else if (row.RequirePaintingListStatus === 3) {
                    return { "is-complate": true };
                } else if (row.RequirePaintingListStatus === 4) {
                    return { "is-cancel": true };
                } else {
                    return { "is-all": true };
                }
            } 
        }
    }
}