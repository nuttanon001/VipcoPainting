import { Component,Input } from "@angular/core";
import { AttachFile } from "../../models/model.index";

@Component({
    selector: "attach-file-view",
    templateUrl: "./attach-file-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})
/** attach-file-view component*/
export class AttachFileViewComponent {
    /** attach-file-view ctor */
    constructor() {}
    
    // Parameter
    @Input() attachFiles: Array<AttachFile>;

    // open attact file
    onOpenNewLink(link: string): void {
        if (link) {
            window.open("paint/" + link, "_blank");
        }
    }
}