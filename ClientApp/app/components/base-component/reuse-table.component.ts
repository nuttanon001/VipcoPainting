import {
    Component, Input, Output,
    EventEmitter, OnInit,
    ElementRef, ViewChild
} from "@angular/core";
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "reuse-table",
    template: `
    <div class="view-container">
        <ngx-datatable
            class="material"
            [rows]="rows"
            [columns]="columns"
            [columnMode]="columnMode"
            [headerHeight]="50"
            [footerHeight]="0"
            [rowHeight]="50"
            [scrollbarV]="true"
            [scrollbarH]="true"
            [selectionType]="'single'"
            [rowClass]="getRowClass"
            (select)="onSelect($event)"
            [style.height]="height">
        </ngx-datatable>
    </div>
  `,
    styleUrls: ["./data-table.style.scss"],
})

export class ReuseTableComponent implements OnInit {
    // input and output
    @Input("columns") columns: any;
    @Input("rows") rows: Array<any>;
    @Input("columnMode") columnMode: string = "flex";
    @Input("height") height: string = "calc(100vh - 184px)";
    @Output("selected") selected = new EventEmitter<any>();
    // height: string;
    constructor() { }
    // angular hook init
    ngOnInit(): void {
        // this.height = "calc(100vh - 184px)";

        // console.log(window);
    }
    // emit row selected to output
    onSelect(selected: any): void {
        if (selected) {
            this.selected.emit(selected.selected[0]);
        }
    }

    // row class
    getRowClass(row?: any): any {
        if (row) {
            // debug 
            // console.log("On row");
            if (row["RequirePaintingListStatus"]) {
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