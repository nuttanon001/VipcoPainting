import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";

import { PaintTaskDetailService } from "../../../services/paint-task/paint-task-detail.service";

@Component({
    selector: "paint-task-detail-paint-report",
    templateUrl: "./paint-task-detail-paint-report.component.html",
    styleUrls: ["../../../styles/report.style.scss"],
})
/** paint-task-detail-paint-report component*/
export class PaintTaskDetailPaintReportComponent implements OnInit {
    @Input() PaintTaskDetailId: number;
    @Output() Back = new EventEmitter<boolean>();
    PaintTask: any;
    /** paint-task-detail-paint-report ctor */
    constructor(
        private service: PaintTaskDetailService
    ) { }

    // called by Angular after aint-task-detail-paint-report component initialized */
    ngOnInit(): void {
        if (this.PaintTaskDetailId) {
            this.service.getReportPaintTaskDetailWorkItemPdf(this.PaintTaskDetailId,"GetReportPatinTaskDetailPaintPdf/")
                .subscribe(dbReport => {
                    this.PaintTask = dbReport;
                    //if (!this.previewOnly) {
                    //    setTimeout(() => {
                    //        this.onPrintOverTimeMaster();
                    //    }, 1500);
                    //    // this.onPrintOverTimeMaster();
                    //}
                });
        } else {
            this.onBackToMaster();
        }
    }

    // on Print OverTimeMaster
    onPrintOverTimeMaster(): void {
        window.print();
    }

    // on Back
    onBackToMaster(): void {
        this.Back.emit(true);
    }
}