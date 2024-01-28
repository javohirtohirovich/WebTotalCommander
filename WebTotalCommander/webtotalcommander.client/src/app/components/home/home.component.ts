import { Component } from '@angular/core';
import { customers } from './customers';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  public gridData: any[] = customers;
}
