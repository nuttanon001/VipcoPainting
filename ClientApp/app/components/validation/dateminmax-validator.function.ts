import { ValidatorFn, AbstractControl, Validators } from "@angular/forms";


/** A hero's name can't match the given regular expression */
export function DateMinValidator(datemin: Date): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        const validator = new Date(control.value) < datemin;
        return { 'date-min': { value: validator ? control.value : null } };
    };
}

export function DateMaxValidator(datemax: Date): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        const validator = new Date(control.value) > datemax;
        return { 'date-max': { value: validator ? control.value : null } };
    };
}