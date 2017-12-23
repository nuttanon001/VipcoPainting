// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { TaskPaintDetail,PaintWorkItem } from "../../../models/model.index";
// 3rd-patry
import { SelectItem } from "primeng/primeng";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";

@Component({
    selector: "task-paint-edit",
    templateUrl: "./task-paint-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** task-paint-edit component*/
export class TaskPaintEditComponent implements OnInit {
    /** task-paint-edit ctor */
    constructor(
        private service: PaintTeamService,
        private servicePaintWork:PaintWorkitemService,
        private fb: FormBuilder,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef, )
    { }

    // Parameter
    // Two way binding
    _taskPaintDetail: TaskPaintDetail;
    @Output() taskPaintDetailChange = new EventEmitter<TaskPaintDetail>();
    @Input()
    get taskPaintDetail(): TaskPaintDetail {
        return this._taskPaintDetail;
    }
    set taskPaintDetail(data: TaskPaintDetail) {
        this._taskPaintDetail = data;
        this.taskPaintDetailChange.emit(this._taskPaintDetail);
    }
    // Value
    @Input() paintWorkItem: PaintWorkItem;
    // FormGroup
    taskPaintDetailForm: FormGroup;
    // ComboBox
    paintTeams: Array<SelectItem>;
    // OnInit
    ngOnInit(): void {
        this.buildForm();
        this.getPaintTeamCombobox();

        if (this.paintWorkItem) {
            this.paintWorkItem.IntColorString += `: ${this.paintWorkItem.IntCalcColorUsage}`;
            this.paintWorkItem.ExtColorString += `: ${this.paintWorkItem.ExtCalcColorUsage}`;
        }
    }
    // get PaintTeam Array
    getPaintTeamCombobox(): void {
        if (!this.paintTeams) {
            // paintTeam ComboBox
            this.service.getAll()
                .subscribe(dbPatinTeam => {
                    this.paintTeams = new Array;
                    for (let item of dbPatinTeam) {
                        this.paintTeams.push({ label: `${(item.TeamName || "")}`, value: item.PaintTeamId });
                    }
                }, error => console.error(error));
        }
    }
    // build Form
    buildForm(): void {
        this.taskPaintDetailForm = this.fb.group({
            TaskPaintDetailId: [this.taskPaintDetail.TaskPaintDetailId],
            Remark: [this.taskPaintDetail.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            Creator: [this.taskPaintDetail.Creator],
            CreateDate: [this.taskPaintDetail.CreateDate],
            Modifyer: [this.taskPaintDetail.Modifyer],
            ModifyDate: [this.taskPaintDetail.ModifyDate],
            //FK
            TaskMasterId: [this.taskPaintDetail.TaskMasterId],
            PaintTeamId: [this.taskPaintDetail.PaintTeamId,
                [
                    Validators.required
                ]
            ],
            PaintWorkItemId: [this.taskPaintDetail.PaintWorkItemId,
                [
                    Validators.required
                ]
            ],
            //ViewModel
            PaintTeamString: [this.taskPaintDetail.PaintTeamString],

        });
        this.taskPaintDetailForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }
    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.taskPaintDetailForm) { return; }
        const form = this.taskPaintDetailForm;
        if (form.valid) {
            this.taskPaintDetail = form.value;
        }
    }
}