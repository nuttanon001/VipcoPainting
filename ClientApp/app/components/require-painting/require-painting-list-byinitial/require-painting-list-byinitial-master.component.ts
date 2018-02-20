import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import {
    AttachFile, RequirePaintList,
    Scroll, ScrollData, 
} from "../../../models/model.index";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { RequirePaintListService , RequirePaintListServiceCommunicate} from "../../../services/require-paint/require-paint-list.service";
// timezone
import * as moment from "moment-timezone";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";

@Component({
    selector: "require-painting-list-byinitial-master",
    templateUrl: "./require-painting-list-byinitial-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
/** require-painting-list-byinitial-master component*/
export class RequirePaintingListByinitialMasterComponent 
    extends BaseMasterComponent<RequirePaintList, RequirePaintListService> {
    /** require-painting-list-byinitial-master ctor */
    constructor(service: RequirePaintListService,
        serviceCom: RequirePaintListServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<RequirePaintList>,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
        private serverAuth: AuthService,
    ) {
        super(
            service,
            serviceCom,
            serviceComDataTable,
            dialogsService,
            viewContainerRef
        );
    }

    // Parameter
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    onlyUser: boolean;

    columns: Array<TableColumn> = [
        { prop: "Description", name: "WorkItem", width: 150 },
        { prop: "MarkNo", name: "MarkNo", width: 75 },
        { prop: "SendWorkItem", name: "SendDate", width: 80, pipe: this.datePipe },
    ];

    // on get data with lazy load
    loadPagedData(scroll: Scroll): void {
        if (this.onlyUser) {
            if (this.serverAuth.getAuth) {
                scroll.Where = this.serverAuth.getAuth.UserName || "";
            }
        } else {
            scroll.Where = "";
        }

        if (this.scroll) {
            if (this.scroll.Filter && scroll.Reload) {
                scroll.Filter = this.scroll.Filter;
            }
        }

        this.scroll = scroll;
        this.service.getAllWithScroll(scroll)
            .subscribe((scrollData: ScrollData<RequirePaintList>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
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
            if (value.SendWorkItem !== null) {
                value.SendWorkItem = moment.tz(value.SendWorkItem, zone).toDate();
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: RequirePaintList): void {
        if (this.serverAuth.getAuth) {
            value.Creator = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // insert data
        this.service.post(value).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.editValue.Creator = undefined;
                this.canSave = true;
                this.dialogsService.error("Failed !",
                    "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
            }
        );
    }

    // on update data
    onUpdateToDataBase(value: RequirePaintList): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.RequirePaintingListId).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.canSave = true;
                this.dialogsService.error("Failed !",
                    "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
            }
        );
    }

    // on detail view
    onDetailView(value?: RequirePaintList): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingListId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }

    // on cancel edit override
    onCancelEdit(): void {
        this.editValue = undefined;
        this.displayValue = undefined;
        this.canSave = false;
        this.ShowEdit = false;
        this.onDetailView(undefined);
    }

    // on detail edit OverRide
    onDetailEdit(editValue?: RequirePaintList): void {
        if (editValue) {
            if (editValue.RequirePaintingListStatus !== 1 && editValue.RequirePaintingListStatus !== 2) {
                this.dialogsService.error("Access Denied", "Status war not waited. you can't edit it.", this.viewContainerRef);
                return;
            }

            if (this.serverAuth.getAuth) {
                if (this.serverAuth.getAuth.LevelUser < 3) {
                    if (this.serverAuth.getAuth.UserName !== editValue.Creator) {
                        this.dialogsService.error("Access Denied", "You don't have permission to access.", this.viewContainerRef);
                        return;
                    }
                }
            }
        }
        super.onDetailEdit(editValue);
    }
}