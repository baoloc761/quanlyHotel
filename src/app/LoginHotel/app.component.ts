import { CommonModule } from '@angular/common';
import { Component, ViewChild} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LocalStorageService } from './serviceLogin.component';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  dataUserInfo: any = {}
  userInfo: any = {
    username: '',
    password: ''    
  }
  
  constructor(private localStorageService : LocalStorageService) {}
  ngOnInit() {
    this.localStorageService.loadDataUserNameLogin()
  }

  onLogin() {
    let datainfo = JSON.parse(this.localStorageService.getItem('dataInfoUser'))
    if (_.isNil(datainfo)) {
      this.localStorageService.loadDataUserNameLogin()
      datainfo = JSON.parse(this.localStorageService.getItem('dataInfoUser'))
    }
    const getUserNameData = _.get(datainfo, 'userName')
    const getPass = _.get(datainfo, 'pass')
    if (getUserNameData === this.userInfo.username && getPass === this.userInfo.password) {
      console.log('Đăng nhập thành công')
    } else {
      console.log('Sai thông tin đăng nhập')

    }
  }
}
