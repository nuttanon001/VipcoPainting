import { Component } from "@angular/core";

@Component({
    selector: "home",
    templateUrl: "./home.component.html"
})
export class HomeComponent {
    onOpenNewLink(): void {
        let link: string = "files/painting_doc.pdf";
        if (link) {
            window.open(link, "_blank");
        }
    }
}
