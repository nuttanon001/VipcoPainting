// angular
import { Component, EventEmitter, Input,OnInit } from "@angular/core";
// models
import { RequirePaintList } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
// 3rd party
import { TableColumn } from "@swimlane/ngx-datatable";

@Component({
    selector: "require-painting-list-view",
    templateUrl: "./require-painting-list-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})

// require-painting-list-view component*/
export class RequirePaintingListViewComponent implements OnInit {
    /** require-painting-list-view ctor */
    constructor() {}
    @Input("requireLists") requireLists: Array<RequirePaintList>;

    ngOnInit(): void {

    }
}