import { CommonModule } from '@angular/common';
import { Component, ViewChild} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, FormsModule],
  templateUrl: './DashBo',
  styleUrl: './DashBoard/mainDashBoard.component.scss'
})
export class mainDashboard {
  title = 'hihihi'
}
