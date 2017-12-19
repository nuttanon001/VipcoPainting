import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import {
    RequirePaintMaster, RequirePaintMasterHasList,
    Scroll, ScrollData, RequirePaintList
} from "../../../models/model.index";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
// timezone
import * as moment from "moment-timezone";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";

@Component({
    selector: "require-painting-master",
    templateUrl: "./require-painting-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
// require-painting-master component*/
export class RequirePaintingMasterComponent 
    extends BaseMasterComponent<RequirePaintMaster, RequirePaintMasterService> {

    columns: Array<TableColumn> = [
        { prop: "RequireNo", name: "Code", flexGrow: 1 },
        { prop: "MachineName", name: "Name", flexGrow: 2 },
        { prop: "TypeMachineString", name: "Group", flexGrow: 1 },
    ];

    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    onlyUser: boolean;
    requirePaintLists: Array<RequirePaintList>|undefined;
    /** require-painting-master ctor */
    constructor(
        service: RequirePaintMasterService,
        serviceCom: RequirePaintMasterServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<RequirePaintMaster>,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
        private serverAuth: AuthService,
        private servicePaintList: RequirePaintListService
    ) {
        super(
            service,
            serviceCom,
            serviceComDataTable,
            dialogsService,
            viewContainerRef
        );
    }

    // on init override
    ngOnInit(): void {

        this.ShowEdit = false;
        this.canSave = false;

        this.subscription1 = this.serviceCom.ToParent$.subscribe(
            (TypeValue: [RequirePaintMasterHasList, boolean]) => {
                this.editValue = TypeValue[0].RequirePaintMaster;
                this.requirePaintLists = TypeValue[0].RequirePaintLists;
                this.canSave = TypeValue[1];
            });

        this.subscription2 = this.dataTableServiceCom.ToParent$
            .subscribe((scroll: Scroll) => this.loadPagedData(scroll));
    }


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
            .subscribe((scrollData: ScrollData<RequirePaintMaster>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: RequirePaintMaster): RequirePaintMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.FinishDate !== null) {
                value.FinishDate = moment.tz(value.FinishDate, zone).toDate();
            }
            if (value.ReceiveDate !== null) {
                value.ReceiveDate = moment.tz(value.ReceiveDate, zone).toDate();
            }
            if (value.RequireDate !== null) {
                value.RequireDate = moment.tz(value.RequireDate, zone).toDate();
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: RequirePaintMaster): void {
        if (this.serverAuth.getAuth) {
            value.Creator = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // insert data
        this.service.post(value).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                if (this.requirePaintLists && complete) {
                    this.requirePaintLists.forEach(item => {
                        item.RequirePaintingMasterId = complete.RequirePaintingMasterId;
                        item.Creator = complete.Creator;
                    });

                    this.servicePaintList.postLists(this.requirePaintLists)
                        .subscribe((complate: any) => {
                            this.requirePaintLists = undefined;
                            this.onSaveComplete();
                        },
                        (error: any) => {
                            this.dialogsService.error("Failed !",
                                "Save failed with the following error: WorkItem has error !!!", this.viewContainerRef);
                        });
                } else {
                    this.onSaveComplete();
                }
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
    onUpdateToDataBase(value: RequirePaintMaster): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.RequirePaintingMasterId).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                if (this.requirePaintLists && complete) {
                    this.requirePaintLists.forEach(item => {
                        if (!item.RequirePaintingListId) {
                            item.RequirePaintingMasterId = complete.RequirePaintingMasterId;
                            item.Creator = complete.Creator;
                        } else {
                            item.Modifyer = complete.Modifyer;
                        }
                    });

                    this.servicePaintList.putLists(this.requirePaintLists)
                        .subscribe((complate: any) => {
                            this.requirePaintLists = undefined;
                            this.onSaveComplete();
                        },
                        (error: any) => {
                            this.dialogsService.error("Failed !",
                                "Save failed with the following error: WorkItem has error !!!", this.viewContainerRef);
                        });
                } else {
                    this.onSaveComplete();
                }
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
    onDetailView(value?: RequirePaintMaster): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingMasterId)
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
        this.requirePaintLists = undefined;
        this.canSave = false;
        this.ShowEdit = false;
        this.onDetailView(undefined);
    }
}