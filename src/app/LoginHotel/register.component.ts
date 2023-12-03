import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '@app/_services';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({ templateUrl: 'register.component.html', styleUrls: [ './register.component.scss'] })
export class RegisterComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;
    error?: string;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private dialogRef: MatDialog,
        private snackBar: MatSnackBar
    ) {
        // redirect to home if already logged in
        if (this.accountService.userValue) {
            this.router.navigate(['/']);
        }
    }

    myOptionsTypeUSer = [
        { id: 1, name: 'admin', isAdmin: true },
        { id: 2, name: 'userStaff', isAdmin: false }
    ];
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
                    this.openSnackBar('Thêm tài khoản thành công', 'OK')
                    this.dialogRef.closeAll()
                },
                error: error => {
                    this.error = error;
                    this.loading = false;
                    this.openSnackBar('Có lỗi' + error, 'ERROR')
                }
            });
    }

    cancel() {
        this.dialogRef.closeAll()
    }
}