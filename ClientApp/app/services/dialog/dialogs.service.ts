import { MatDialogRef, MatDialog, MatDialogConfig } from "@angular/material";
import { Injectable, ViewContainerRef } from "@angular/core";
import { Observable } from "rxjs/Rx";
// components
import {
    ConfirmDialog,ContextDialog,
    ErrorDialog,
    ColorItemDialogComponent,
    StandradTimeDialogComponent,
    SurfaceTypeDialogComponent,
    EmployeeDialogComponent,
    ProjectDialogComponent,
    RequirePaintingDialogComponent,
    RequirePaintingViewDialogComponent,
    ScheduleDialogComponent,
    RequisitionListDialogComponent
} from "../../components/dialog/dialog.index";
// models
import {
    Color, StandradTime,
    SurfaceType, Employee,
    ProjectSub,
    PaintTaskDetail,
    OptionTaskMasterSchedule,
} from "../../models/model.index";

@Injectable()
export class DialogsService {

    // width and height > width and height in scss master-dialog
    width: string = "950px";
    height: string = "500px";

    constructor(private dialog: MatDialog) { }

    public confirm(title: string, message: string, viewContainerRef: ViewContainerRef): Observable<boolean> {

        let dialogRef: MatDialogRef<ConfirmDialog>;
        let config: MatDialogConfig = new MatDialogConfig();
        config.viewContainerRef = viewContainerRef;

        dialogRef = this.dialog.open(ConfirmDialog, config);

        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.message = message;

        return dialogRef.afterClosed();
    }

    public context(title: string, message: string, viewContainerRef: ViewContainerRef): Observable<boolean> {

        let dialogRef: MatDialogRef<ContextDialog>;
        let config: MatDialogConfig = new MatDialogConfig();
        config.viewContainerRef = viewContainerRef;

        dialogRef = this.dialog.open(ContextDialog, config);

        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.message = message;

        return dialogRef.afterClosed();
    }

    public error(title: string, message: string, viewContainerRef: ViewContainerRef): Observable<boolean> {

        let dialogRef: MatDialogRef<ErrorDialog>;
        let config: MatDialogConfig  = new MatDialogConfig();
        config.viewContainerRef = viewContainerRef;

        dialogRef = this.dialog.open(ErrorDialog, config);

        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.message = message;

        return dialogRef.afterClosed();
    }

    public dialogSelectColorItem(viewContainerRef: ViewContainerRef, type: number = 0): Observable<Color> {
        let dialogRef: MatDialogRef<ColorItemDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = type;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(ColorItemDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogSelectStandradTime(viewContainerRef: ViewContainerRef, type: number = 0): Observable<StandradTime> {
        let dialogRef: MatDialogRef<StandradTimeDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = type;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(StandradTimeDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogSelectSurfaceType(viewContainerRef: ViewContainerRef, type: number = 0): Observable<SurfaceType> {
        let dialogRef: MatDialogRef<SurfaceTypeDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = type;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(SurfaceTypeDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogSelectEmployee(viewContainerRef: ViewContainerRef, type: number = 0): Observable<Employee> {
        let dialogRef: MatDialogRef<EmployeeDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = type;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(EmployeeDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogSelectProject(viewContainerRef: ViewContainerRef, type: number = 0): Observable<ProjectSub> {
        let dialogRef: MatDialogRef<ProjectDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = type;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(ProjectDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogRequestPaintList(viewContainerRef: ViewContainerRef, MasterId: number = 0): Observable<boolean> {
        let dialogRef: MatDialogRef<RequirePaintingDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = MasterId;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(RequirePaintingDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogRequestPaintView(viewContainerRef: ViewContainerRef, MasterId: number): Observable<number> {
        
        let dialogRef: MatDialogRef<RequirePaintingViewDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = MasterId;
        // config.height = this.height;
        // config.width= this.width;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(RequirePaintingViewDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogTaskPaintMasterScheduleView(viewContainerRef: ViewContainerRef,Option: OptionTaskMasterSchedule): Observable<boolean> {
        let dialogRef: MatDialogRef<ScheduleDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = Option;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(ScheduleDialogComponent, config);
        return dialogRef.afterClosed();
    }

    public dialogRequisitionListAddOrUpdate(viewContainerRef: ViewContainerRef, paintTaskDetail: PaintTaskDetail): Observable<boolean> {
        let dialogRef: MatDialogRef<RequisitionListDialogComponent>;
        let config: MatDialogConfig = new MatDialogConfig();

        // config
        config.viewContainerRef = viewContainerRef;
        config.data = paintTaskDetail;
        config.hasBackdrop = true;

        // open dialog
        dialogRef = this.dialog.open(RequisitionListDialogComponent, config);
        return dialogRef.afterClosed();
    }
}
