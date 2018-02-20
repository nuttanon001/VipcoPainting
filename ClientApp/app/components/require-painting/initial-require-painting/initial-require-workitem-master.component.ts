// angular
import { Component, ViewContainerRef, OnInit } from "@angular/core";

@Component({
    selector: "initial-require-workitem-master",
    templateUrl: "./initial-require-workitem-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
})
/** initial-require-workitem-master component*/
export class InitialRequireWorkitemMasterComponent implements OnInit{
    /** initial-require-workitem-master ctor */
    constructor() { }

    // Parameter
    showSchedule: boolean;
    initialRequirePaintId: number;
    // On Init Angular Hook
    ngOnInit(): void {
        this.showSchedule = true;
        this.initialRequirePaintId = 0;
    }

    // On schedule send initialRequirePaintId to initial-require-workitem
    onScheduleSendId(initialRequireId?:number):void {
        if (initialRequireId) {
            if (initialRequireId > 0) {
                this.initialRequirePaintId = initialRequireId;
                this.showSchedule = false;
            }
        }
    }

    // On initialRequirePaint Work-Item send status
    onInitialWorkItemSendStatus(ComplateOrFailed?:number): void {
        if (ComplateOrFailed !== undefined) {
            this.initialRequirePaintId = 0;
            this.showSchedule = true;
        }
    }
}