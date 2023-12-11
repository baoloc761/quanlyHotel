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
  pageIdRole: { id: string }[] = [];
  pageStates: {[pageId: string]: boolean} = {};
  
  constructor(private snackBar: MatSnackBar, 
    private accountService: AccountService, 
    private route: ActivatedRoute,
    private translate: TranslateService) {}

  ngOnInit() {
    this.accountService.getUsers().subscribe(data => {
      this.users = data;
    })

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

  handleCheckboxChange(pageId: any) {
    const isChecked = this.pageStates[pageId]
    const userId = this.selectedUserId
    const user = this.users.find(u => u.id === this.selectedUserId);
    if (!user) return
    type Claim = {
      pageId: string
    }
    const existingClaimIndex = user.claims?.findIndex((c: Claim) => c.pageId === pageId)

    if (isChecked) {
      if(_.isNil(existingClaimIndex)) {
        const newClaim = {
          pageId: pageId
        }
        if (_.isNil(user.claims)) {
          user.claims = newClaim
        } else {
            user.claims = [newClaim];
        }
      }
      const getListUser = _.forEach(this.users, (x) => x.claims === pageId)
        const existingItem = this.pageIdRole.find(item => item.id === pageId)
        if (!existingItem) {
            this.pageIdRole.push({ id: pageId })
        }
    } else {
        const index = this.pageIdRole.findIndex(item => item.id === pageId)
        if (index !== -1) {
            this.pageIdRole.splice(index, 1)
            user.claims.splice(index, 1);
        }
    }
    console.log('this.users', this.users)
    
    return this.pageIdRole || []
  }

  handleAuthorizationAccount() {
    const UserId = this.selectedUserId;
    const findpageIdRole = _.filter(this.pageIdRole, x => typeof _.isEqual(x, 'object') && 'id' in x)
    const getListPage = this.listPages
    const msg = this.translate.instant('ActionEntityResultAuthorization', {
      actionName: this.translate.instant('PleaseChooseOptionPage'),
      result: this.translate.instant(!_.isEmpty(UserId) ? 'Success' : 'Error')
    })
  
    if (_.isEmpty(UserId)) {
      this.openSnackBar(msg, this.translate.instant('Empty'));
      return;
    }
    _.forEach(getListPage, (page) => {
      const isPageSelected = findpageIdRole.some(role => role.id === page.id)
      if (isPageSelected) {
        if (!page.roleUser.includes(UserId)) {
          page.roleUser.push(UserId)
        }
      } else {
        const index = page.roleUser.indexOf(UserId)
        if (index !== -1) {
          page.roleUser.splice(index, 1)
        }
      }
    })
    this.accountService.auThorrizationUser(getListPage)
  }
}