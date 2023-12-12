import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '@app/_services';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { RoleDTO } from '@app/_models';
import { Subscription } from 'rxjs';

@Component({ templateUrl: 'register.component.html', styleUrls: [ './register.component.scss'] })
export class RegisterComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;
    error?: string;
    subscription?: Subscription;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private dialogRef: MatDialog,
        private snackBar: MatSnackBar,
        private translate: TranslateService
    ) {
        // redirect to home if already logged in
        if (this.accountService.userValue) {
            this.router.navigate(['/']);
        }
    }

    myOptionsTypeUSer: RoleDTO[] = [];
    selected?: any = {};

    ngOnInit() {
        this.form = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            username: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]],
            typeUser: [ null, Validators.required ]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    enCodeValue(val: any) {
        if (_.isEmpty(val)) return ''
        return JSON.stringify(val)
    }


    deCode(hashedValue: any) {
        if (_.isEmpty(hashedValue)) return {}
        return JSON.parse(hashedValue)
    }

    openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
          duration: 2000,
        });
    }

    onSubmit() {
        this.submitted = true;

        // reset alert on submit
        this.error = '';

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        const newUser = {...this.form.value, typeUser: this.deCode(this.f.typeUser.value)}
        console.log(newUser)
        this.accountService.register(newUser)
            .pipe(first())
            .subscribe({
                next: () => {
                    const msg = this.translate.instant('ActionEntityResult', { entityName: this.translate.instant('Account'), actionName: this.translate.instant('Create'), result:  this.translate.instant('Success') })
                    this.openSnackBar(msg, this.translate.instant('Success'))
                    this.dialogRef.closeAll()
                },
                error: error => {
                    this.error = error;
                    this.loading = false;
                    const msg = this.translate.instant('ActionEntityResultWithReason', { entityName: this.translate.instant('Account'), actionName: this.translate.instant('Create'), result:  this.translate.instant('Failed'), reason: error })
                    this.openSnackBar(msg, this.translate.instant('Failed'))
                }
            });
    }

    cancel() {
        this.dialogRef.closeAll()
    }
}