import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from '@app/_models';
import { AccountService } from '@app/_services';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs/operators';

@Component({ templateUrl: 'user-edit.component.html', styleUrls: [ './user-edit.component.scss'] })
export class UserEditComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;
    error?: string;
    user: User = {}

    constructor(
        private formBuilder: FormBuilder,
        private accountService: AccountService,
        private dialogRef: MatDialog,
        @Inject(MAT_DIALOG_DATA) private data: any,
        private snackBar: MatSnackBar,
        private translate: TranslateService
    ) {
        this.user = this.data
    }

    myOptionsTypeUSer = [
        { id: 1, name: 'admin', isAdmin: true },
        { id: 2, name: 'userStaff', isAdmin: false }
    ];
    selected?: any = {};

    ngOnInit() {
        this.form = this.formBuilder.group({
            firstName: [this.user?.firstName || '', Validators.required],
            lastName: [this.user?.lastName || '', Validators.required],
            username: [this.user?.username || '', Validators.required],
            password: [this.user?.password || '', [Validators.required, Validators.minLength(6)]],
            typeUser: [ this.enCodeValue(this.user?.typeUser) || null, Validators.required ]
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
        const newUser = {...this.user, ...this.form.value, typeUser: this.deCode(this.f.typeUser.value)}
        console.log(newUser)
        this.accountService.updateUser(newUser)
            .pipe(first())
            .subscribe({
                next: () => {
                    const msg = this.translate.instant('ActionEntityResult', { entityName: this.translate.instant('Account'), actionName: this.translate.instant('Edit'), result:  this.translate.instant('Success') })
                    this.openSnackBar(msg, this.translate.instant('Success'))
                    this.dialogRef.closeAll()
                },
                error: error => {
                    this.error = error;
                    this.loading = false;
                    const msg = this.translate.instant('ActionEntityResultWithReason', { entityName: this.translate.instant('Account'), actionName: this.translate.instant('Edit'), result:  this.translate.instant('Failed'), reason: error })
                    this.openSnackBar(msg, this.translate.instant('Failed'))
                }
            });
    }

    cancel() {
        this.dialogRef.closeAll()
    }
}