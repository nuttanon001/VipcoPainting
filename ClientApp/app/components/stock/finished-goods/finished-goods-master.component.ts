﻿import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import { FinishedGoodsMaster, Scroll, ScrollData } from "../../../models/model.index";
// timezone
import * as moment from "moment-timezone";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { FinishedGoodsMasterService, FinishedGoodsMasterServiceCommunicate } from "../../../services/stock/finished-goods-master.service";

@Component({
    selector: "finished-goods-master",
    templateUrl: "./finished-goods-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
/** finished-goods-master component*/
export class FinishedGoodsMasterComponent 
    extends BaseMasterComponent<FinishedGoodsMaster, FinishedGoodsMasterService> {
    /** finished-goods-master ctor */
    constructor(
        service: FinishedGoodsMasterService,
        serviceCom: FinishedGoodsMasterServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<FinishedGoodsMaster>,
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

    //Parameter
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    columns: Array<TableColumn> = [
        { prop: "ColorNameString", name: "PaintName", flexGrow: 1 },
        { prop: "Quantity", name: "Quantity", flexGrow: 1 },
        { prop: "FinishedGoodsDate", name: "Date", flexGrow: 1, pipe: this.datePipe, },

    ];

    // on get data with lazy load
    loadPagedData(scroll: Scroll): void {
        if (this.scroll) {
            if (this.scroll.Filter && scroll.Reload) {
                scroll.Filter = this.scroll.Filter;
            }
        }

        this.scroll = scroll;
        this.service.getAllWithScroll(scroll)
            .subscribe((scrollData: ScrollData<FinishedGoodsMaster>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: FinishedGoodsMaster): FinishedGoodsMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.FinishedGoodsDate !== null) {
                value.FinishedGoodsDate = moment.tz(value.FinishedGoodsDate, zone).toDate();
            }
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: FinishedGoodsMaster): void {
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
    onUpdateToDataBase(value: FinishedGoodsMaster): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.FinishedGoodsMasterId).subscribe(
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
    onDetailView(value: FinishedGoodsMaster): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.FinishedGoodsMasterId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }
}