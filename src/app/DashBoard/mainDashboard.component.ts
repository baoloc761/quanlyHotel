import { CommonModule } from '@angular/common';
import { Component, ViewChild} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'dashboard',
  templateUrl: './mainDashboard.component.html',
  styleUrls: [ './mainDashBoard.component.scss' ]
})
export class Dashboard {
  title = 'hihihi'
}
