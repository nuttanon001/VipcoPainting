import { ValidatorFn, AbstractControl, Validators } from "@angular/forms";


/** date min validator */
export function DateMinValidator(datemin: Date): ValidatorFn {
    // console.log("MinDate:",JSON.stringify(datemin));
    return (control: AbstractControl): { [key: string]: any } => {
        const validator = new Date(control.value) < datemin;
        return { 'date-min': { value: validator ? control.value : null } };
    };
}

/** date max validator */
export function DateMaxValidator(datemax: Date): ValidatorFn {
    // console.log("MinDate:", JSON.stringify(datemax));
    return (control: AbstractControl): { [key: string]: any } => {
        const validator = new Date(control.value) > datemax;
        return { 'date-max': { value: validator ? control.value : null } };
    };
}