import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { MatMenuTrigger } from "@angular/material";
// service
// unmark this if AuthService complete
import { AuthService } from "../../services/auth/auth.service";
// model
import { User } from "../../models/model.index";
@Component({
    selector: "nav-menu",
    templateUrl: "./navmenu.component.html",
    styleUrls: ["../../styles/navmenu.style.scss"],
})
export class NavMenuComponent implements OnInit {
    @ViewChild("mainMenu") mainMenu: MatMenuTrigger;
    @ViewChild("subMenu") subMenu: MatMenuTrigger;
    @ViewChild("subMenu2") subMenu2: MatMenuTrigger;

    constructor(
        // unmark this if AuthService complete
        private authService: AuthService,
        private router: Router
    ) { }

    // propertires
    // =============================================\\
    get GetLevel3(): boolean {
        if (this.authService.getAuth) {
            return (this.authService.getAuth.LevelUser || 0) > 3;
        } else {
            return false;
        }
    }

    get GetLevel2(): boolean {
        if (this.authService.getAuth) {
            return (this.authService.getAuth.LevelUser || 0) > 1;
        } else {
            return false;
        }
    }

    get GetLevel1(): boolean {
         if (this.authService.getAuth) {
            return (this.authService.getAuth.LevelUser || 0) > 0;
         } else {
            return false;
         }
        // return true;
    }

    ngOnInit(): void {
        // reset login status
        this.authService.logout();
    }

    get showLogin(): boolean {
        // return false;
        // unmark this if AuthService complete
        if (this.authService) {
            if (this.authService.isLoggedIn) {
                return !this.authService.isLoggedIn;
            }
        }
        return true;
    }

    get userName(): string {
        if (this.authService.getAuth) {
            if (this.authService.getAuth.NameThai) {
                return " " + this.authService.getAuth.NameThai + " ";
            }
        }
        return "";
    }

    // on menu close
    // =============================================\\
    menuOnCloseMenu(except?:number): void {
        if (this.mainMenu && this.subMenu && this.subMenu2 && except) {

            if (except === 1) {
                this.subMenu.closeMenu();
                this.subMenu2.closeMenu();
            } else if (except === 2) {
                this.mainMenu.closeMenu();
                this.subMenu2.closeMenu();
            } else if (except === 3) {
                this.mainMenu.closeMenu();
                this.subMenu.closeMenu();
            }
        }
    }

    // =============================================\\
    // on menu open
    // =============================================\\
    menuOnOpenMenu(include?:number): void {
        if (this.mainMenu && this.subMenu && this.subMenu2 && include) {
            if (include === 1) {
                this.mainMenu.openMenu();
            } else if (include === 2) {
                this.subMenu.openMenu();
            } else if (include === 3) {
                this.subMenu2.openMenu();
            }
        }
    }
    // =============================================\\
    onLogOut(): void {
        this.authService.logout();
        this.router.navigate(["login"]);
    }
}