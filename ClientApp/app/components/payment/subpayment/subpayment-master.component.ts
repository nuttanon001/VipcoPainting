import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import {
    SubPaymentMaster, Scroll, ScrollData, SubPaymentDetail
} from "../../../models/model.index";
// timezone
import * as moment from "moment-timezone";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { SubpaymentMasterService,SubpaymentMasterServiceCommunicate } from "../../../services/payment/subpayment-master.service";

@Component({
    selector: "subpayment-master",
    templateUrl: "./subpayment-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
/** subpayment-master component*/
export class SubpaymentMasterComponent 
    extends BaseMasterComponent<SubPaymentMaster, SubpaymentMasterService> {
    /** subpayment-master ctor */
    constructor(
        service: SubpaymentMasterService,
        serviceCom: SubpaymentMasterServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<SubPaymentMaster>,
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
        { prop: "SubPaymentNo", name: "SubPaymentMaster", flexGrow: 1 },
        { prop: "PaintTeamString", name: "PaymentType", flexGrow: 1 },
        { prop: "SubPaymentDate", name: "Cost", flexGrow: 1, pipe: this.datePipe },
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
            .subscribe((scrollData: ScrollData<SubPaymentMaster>) => {
                // console.log(scrollData);
                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: SubPaymentMaster): SubPaymentMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.EndDate !== null) {
                value.EndDate = moment.tz(value.EndDate, zone).toDate();
            }
            if (value.StartDate !== null) {
                value.StartDate = moment.tz(value.StartDate, zone).toDate();
            }
            if (value.SubPaymentDate !== null) {
                value.SubPaymentDate = moment.tz(value.SubPaymentDate, zone).toDate();
            }

            // TaskBlastDetail
            if (value.SubPaymentDetails) {
                value.SubPaymentDetails.forEach((item, index) => {
                    if (item.CreateDate !== null && item.CreateDate !== undefined) {
                        item.CreateDate = moment.tz(item.CreateDate, zone).toDate();
                    }
                    if (item.ModifyDate !== null && item.ModifyDate !== undefined) {
                        item.ModifyDate = moment.tz(item.ModifyDate, zone).toDate();
                    }
                    if (item.PaymentDate !== null && item.PaymentDate !== undefined) {
                        item.PaymentDate = moment.tz(item.PaymentDate, zone).toDate();
                    }

                    if (value.SubPaymentDetails) {
                        let newData: SubPaymentDetail = {
                            SubPaymentDetailId: item.SubPaymentDetailId,
                            AreaWorkLoad: item.AreaWorkLoad,
                            CalcCost: item.CalcCost,
                            PaymentDate: item.PaymentDate,
                            Remark: item.Remark,
                            //BaseModel
                            Creator: item.Creator,
                            CreateDate: item.CreateDate,
                            Modifyer: item.Modifyer,
                            ModifyDate: item.ModifyDate,
                            //FK
                            SubPaymentMasterId: item.SubPaymentMasterId,
                            PaintTaskDetailId: item.PaintTaskDetailId,
                            PaymentCostHistoryId: item.PaymentCostHistoryId,
                        };
                        value.SubPaymentDetails[index] = newData;
                    }
                });
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: SubPaymentMaster): void {
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
    onUpdateToDataBase(value: SubPaymentMaster): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.SubPaymentMasterId).subscribe(
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
    onDetailView(value: SubPaymentMaster): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.SubPaymentMasterId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }
}