// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { RequirePaintMaster, RequirePaintList,RequirePaintSub } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintSubService } from "../../../services/require-paint/require-paint-sub.service";

@Component({
    selector: "require-painting-edit",
    templateUrl: "./require-painting-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
// require-painting-edit component*/
export class RequirePaintingEditComponent 
    extends BaseEditComponent<RequirePaintMaster, RequirePaintMasterService> {
    /** require-painting-edit ctor */
    constructor(
        service: RequirePaintMasterService,
        serviceCom: RequirePaintMasterServiceCommunicate,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private serviceList: RequirePaintListService,
        private serviceSub: RequirePaintSubService,
        private serviceDialogs: DialogsService,
        private fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
        );
    }
    // Parameter
    requireLists: Array<RequirePaintList>;
    listItem: RequirePaintList | undefined;
    indexListItem: number;

    // on get data by key
    onGetDataByKey(value?: RequirePaintMaster): void {
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
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
                                this.requireLists = dbLists.slice();
                            });
                    }

                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                RequirePaintingMasterId: 0,
                FinishDate: new Date,
                ReceiveDate: new Date,
                RequireDate: new Date
            };

            if (this.serviceAuth.getAuth) {
                this.editValue.RequireEmp = this.serviceAuth.getAuth.EmpCode || "";
                this.editValue.RequireString = this.serviceAuth.getAuth.NameThai || "";
            }
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
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
        if (type) {
            if (type === 'Employee') {
                this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        console.log(emp);
                        if (emp) {
                            this.editValueForm.patchValue({
                                RequireEmp: emp.EmpCode,
                                ReceiveString: `คุณ${emp.NameThai}`,
                            });
                        }
                    });
            } else if (type === 'Project') {
                this.serviceDialogs.dialogSelectProject(this.viewContainerRef)
                    .subscribe(project => {
                        if (project) {
                            this.editValueForm.patchValue({
                                ProjectCodeSubId: project.ProjectCodeSubId,
                                ProjectCodeSubString: `${project.Code} ${project.Name}`,
                            });
                        }
                    });
            }
        }
    }

    // add list item
    addListItem(listItem?:RequirePaintList): void {
        if (listItem) {
            if (this.requireLists) {
                this.indexListItem = this.requireLists.indexOf(listItem);
            } else {
                this.indexListItem = -1;
            }
        } else {
            listItem = {
                RequirePaintingListId: 0,
            };
            this.indexListItem = -1;
        }
        this.listItem = listItem;
    }

    // edit list item
    onComplateOrCancel(listItem?: RequirePaintList): void {
        if (!this.requireLists) {
            this.requireLists = new Array;
        }

        if (listItem) {
            if (this.indexListItem > -1) {
                // remove item
                this.requireLists.splice(this.indexListItem, 1);
            }
            // cloning an object
            this.requireLists.push(Object.assign({}, listItem));
        }
        this.listItem = undefined;
    }
}