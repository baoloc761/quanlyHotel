import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormBuilder, FormGroup, FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '@app/_services';

@Component({
  selector: 'users-list',
  templateUrl: './users.component.html',
  styleUrls: [ './users.component.scss' ]
})
export class UsersComponent implements OnInit {
  form!: FormGroup;
  title = 'Users Management'
  users: any[] = []

  constructor(private formBuilder: FormBuilder, private accountService: AccountService) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
        keyword: ['', null]
    });
    
    this.users = this.accountService.getUsers(this.keyword)
  }

  get keyword(): string {
    return this.form.get('keyword')?.value
  }

  search() {
    this.users = this.accountService.getUsers(this.keyword)
  }
}
