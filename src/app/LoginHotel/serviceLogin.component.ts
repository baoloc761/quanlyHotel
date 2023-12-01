import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  dataUserInfo: any = {}

  constructor() { }

  setItem(key: string, value: any): void {
    localStorage.setItem(key, JSON.stringify(value));
  }

  getItem(key: string): any {
    const item = localStorage.getItem(key);
    return item ? JSON.parse(item) : null;
  }

  removeItem(key: string): void {
    localStorage.removeItem(key);
  }

  clear(): void {
    localStorage.clear();
  }

  fakeData() {
    this.dataUserInfo = {
      id: 1,
        name: 'Hoàng Bảo Lộc', 
        userName: 'admin',
        pass: '123456',
        DateCreated: new Date(),
        address: '491A/32 Lê Văn Sỹ, Phường 10, Quận 3, TPHCM',
        BirthDay: '05/05/1998',
        isAdmin: true
    }
    this.setItem('dataInfoUser', JSON.stringify(this.dataUserInfo))
  }

  loadDataUserNameLogin(): void {
    const datainfo = this.getItem('dataInfoUser')
    if (!_.isEmpty(datainfo)) {
      this.dataUserInfo = JSON.parse(datainfo)
    } else {
      this.fakeData()
    }
  }
}