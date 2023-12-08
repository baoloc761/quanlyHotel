import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '@app/_services';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'authorizastion-user',
  templateUrl: './authorrizationUser.component.html',
  styleUrls: ['./authorrizationUser.component.scss']
})
export class authorrizationUserComponent implements OnInit {
  selectedUserId: string = '';
  users: any[] = [];
  listPages: any[] = [];
  pageIdRole: any[] = [];
  pageStates: {[pageId: string]: boolean} = {};
  
  constructor(private snackBar: MatSnackBar, 
    private accountService: AccountService, 
    private route: ActivatedRoute,
    private translate: TranslateService) {}

  ngOnInit() {
    this.users = this.accountService.getAllUsers();
    this.listPages = this.accountService.getListPages();
    this.listPages.forEach(page => {
      this.pageStates[page.id] = false
    })
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  handleCheckboxChange(pageId: any[]) {
    const formData = this.selectedUserId;
    const getStringIdPageRole = pageId || ''
    if(_.isEmpty(formData)) return
    console.log('getStringIdPageRole', getStringIdPageRole);
    return this.pageIdRole = getStringIdPageRole || undefined
  }

  handleAuthorizationAccount() {
    const UserId = this.selectedUserId;
    const checkedPageRole = Object.values(this.pageStates).some(value => value === true)
    const isCheckedPageUserRole = this.handleCheckboxChange
    const msg = this.translate.instant('ActionEntityResultAuthorization', 
      { actionName: !_.isEmpty(isCheckedPageUserRole) ? this.translate.instant('Please') : this.translate.instant('PleaseChooseOptionPage'), 
      result:  this.translate.instant(!_.isEmpty(UserId) ? 'Success' : 'Error') })
    if (_.isEmpty(UserId)) {
      this.openSnackBar(msg, this.translate.instant('Empty'))
      return
    }
    if (!isCheckedPageUserRole) {
      this.openSnackBar(msg, this.translate.instant('Empty'))
      return
    }
    const getUserId = _.filter(this.users, {id: UserId})
    const getPageRole = _.filter(this.listPages, (page) => page.id === this.pageIdRole)
    const addUserIntoPageRole = _.forEach(this.listPages, (page) => {
      page.roleUser = _.isEqual(page.id, this.pageIdRole) 
        ? getUserId 
        : (checkedPageRole ? page.roleUser : page.roleUser = [])

    })
    console.log(checkedPageRole)
  }
}
