import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import { MovementStockStatus, Scroll, ScrollData } from "../../../models/model.index";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
// pipes
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// timezone
import * as moment from "moment-timezone";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { MovementStockStatusService, MovementStockStatusServiceCommunicate } from "../../../services/stock/movement-stock-status.service";

@Component({
    selector: "movement-status-master",
    templateUrl: "./movement-status-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
/** movement-status-master component*/
export class MovementStatusMasterComponent 
    extends BaseMasterComponent<MovementStockStatus, MovementStockStatusService> {
    /** movement-status-master ctor */
    constructor(
        service: MovementStockStatusService,
        serviceCom: MovementStockStatusServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<MovementStockStatus>,
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
        { prop: "StatusName", name: "Status", flexGrow: 1 },
        { prop: "CreateDate", name: "Date", flexGrow: 1, pipe: this.datePipe, },

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
            .subscribe((scrollData: ScrollData<MovementStockStatus>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: MovementStockStatus): MovementStockStatus {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
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
    onInsertToDataBase(value: MovementStockStatus): void {
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
    onUpdateToDataBase(value: MovementStockStatus): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.MovementStockStatusId).subscribe(
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
    onDetailView(value: MovementStockStatus): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.MovementStockStatusId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }
}