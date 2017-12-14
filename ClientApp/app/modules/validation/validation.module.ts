import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { ValidationComponent } from "../../components/validation/validation.componet";
import { ValidationService } from "../../services/validation/validation.service";

@NgModule({
    declarations: [
        ValidationComponent,
    ],
    imports: [
        CommonModule,
    ],
    exports: [
        ValidationComponent,
    ],
    providers: [ValidationService],
    entryComponents: [
        ValidationComponent,
    ]
})
export class ValidationModule { }