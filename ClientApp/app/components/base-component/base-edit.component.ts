﻿// angular
import { OnInit, OnDestroy, ViewContainerRef } from "@angular/core";
import { FormGroup } from "@angular/forms";
// rxjs
import { Subscription } from 'rxjs/Subscription';
import { Calendar } from "primeng/components/calendar/calendar";

export abstract class BaseEditComponent<Model,Service> implements OnInit, OnDestroy {
    
    constructor(
        protected service: Service,
        protected communicateService: any,
    ) { }
    // parameter
    editValue: Model;
    editValueForm: FormGroup;
    subscription: Subscription;
    // on hook component
    ngOnInit(): void {
        this.subscription = this.communicateService.toChildEdit$.subscribe(
            (editValue: Model) => this.onGetDataByKey(editValue));
    }
    // on hook component
    ngOnDestroy(): void {
        // prevent memory leak when component destroyed
        this.subscription.unsubscribe();
    }
    // on get data by key
    abstract onGetDataByKey(value: Model): void;
    // define data for edit form
    abstract defineData(): void;
    // build form
    abstract buildForm(): void;
    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.editValueForm) { return; }
        const form = this.editValueForm;
        // on form valid or not
        this.onFormValid(form.valid);
    }
    // on valid data
    onFormValid(isValid: boolean): void {
        this.editValue = this.editValueForm.value;
        this.communicateService.toParent([this.editValue, isValid]);
    }
    // bug calendar not update min-max
    // update CakenderUi
    updateCalendarUI(calendar: Calendar) {
        calendar.updateUI();
    }
}