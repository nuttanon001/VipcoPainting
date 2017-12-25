// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { TaskBlastDetail, BlastWorkItem } from "../../../models/model.index";
// 3rd-patry
import { SelectItem } from "primeng/primeng";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { BlastRoomService } from "../../../services/task/blast-room.service";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";

@Component({
    selector: "task-blast-edit",
    templateUrl: "./task-blast-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** task-blast-edit component*/
export class TaskBlastEditComponent implements OnInit {
    /** task-blast-edit ctor */
    constructor(
        private service: BlastRoomService,
        private serviceBlastWork: BlastWorkitemService,
        private fb: FormBuilder,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef, ) { }

    // Parameter
    // Two way binding
    _taskBlastDetail: TaskBlastDetail;
    @Output() taskBlastDetailChange = new EventEmitter<TaskBlastDetail>();
    @Input()
    get taskBlastDetail(): TaskBlastDetail {
        return this._taskBlastDetail;
    }
    set taskBlastDetail(data: TaskBlastDetail) {
        this._taskBlastDetail = data;
        this.taskBlastDetailChange.emit(this._taskBlastDetail);
    }
    // Value
    @Input() blastWorkItem: BlastWorkItem;
    @Output() hasChange = new EventEmitter<boolean>();
    // FormGroup
    taskBlastDetailForm: FormGroup;
    // ComboBox
    blastRooms: Array<SelectItem>;
    // OnInit
    ngOnInit(): void {
        this.buildForm();
        this.getBlastRoomCombobox();
    }
    // get BlastRoom Array
    getBlastRoomCombobox(): void {
        if (!this.blastRooms) {
            // BlastRoom ComboBox
            this.service.getAll()
                .subscribe(dbPatinTeam => {
                    this.blastRooms = new Array;
                    for (let item of dbPatinTeam) {
                        this.blastRooms.push({ label: `${(item.BlastRoomName || "")}/${(item.TeamBlastString || "")}`, value: item.BlastRoomId });
                    }
                }, error => console.error(error));
        }
    }
    // build Form
    buildForm(): void {
        this.taskBlastDetailForm = this.fb.group({
            TaskBlastDetailId: [this.taskBlastDetail.TaskBlastDetailId],
            Remark: [this.taskBlastDetail.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            Creator: [this.taskBlastDetail.Creator],
            CreateDate: [this.taskBlastDetail.CreateDate],
            Modifyer: [this.taskBlastDetail.Modifyer],
            ModifyDate: [this.taskBlastDetail.ModifyDate],
            //FK
            TaskMasterId: [this.taskBlastDetail.TaskMasterId],
            BlastRoomId: [this.taskBlastDetail.BlastRoomId,
                [
                    Validators.required
                ]
            ],
            BlastWorkItemId: [this.taskBlastDetail.BlastWorkItemId,
                [
                    Validators.required
                ]
            ],
            //ViewModel
            BlastRoomString: [this.taskBlastDetail.BlastRoomString],
            BlastWorkItem: [this.taskBlastDetail.BlastWorkItem],
        });
        this.taskBlastDetailForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }
    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.taskBlastDetailForm) { return; }
        const form = this.taskBlastDetailForm;
        this.hasChange.emit(form.valid);
        if (form.valid) {
            this.taskBlastDetail = form.value;
        }
    }
}