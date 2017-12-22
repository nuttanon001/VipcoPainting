import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { RouterModule } from "@angular/router";
// components
import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/login/register.component";
import { NavMenuComponent } from './components/navmenu/navmenu.component';
// modules
import {
    CustomMaterialModule, ValidationModule,
    DialogsModule, RequirePaintingModule,
    OtherModule, TaskModule
} from "./modules/module.index";
// services
import { AuthGuard } from "./services/auth/auth-guard.service";
import { AuthService } from "./services/auth/auth.service";

// 3rd party
import { NgxDatatableModule } from "@swimlane/ngx-datatable";
import "hammerjs";
import "popper.js";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
    ],
    imports: [
        HttpModule,
        FormsModule,
        CommonModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: HomeComponent },
            { path: "login", component: LoginComponent },
            { path: "register/:condition", component: RegisterComponent },
            { path: "register", component: RegisterComponent },
            // { path: "project", component: ProjectMasterComponent },
            { path: "**", redirectTo: "home" }
        ]),
        // module
        ValidationModule,
        CustomMaterialModule,
        DialogsModule,
        RequirePaintingModule,
        OtherModule,
        TaskModule,
        // 3rd party
        // mark NgxDatatableModule,
    ],
    providers: [
        AuthGuard,
        AuthService
    ]
})
export class AppModuleShared {
}
