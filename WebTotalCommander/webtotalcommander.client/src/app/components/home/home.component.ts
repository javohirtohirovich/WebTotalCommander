import { Component, OnInit, inject } from '@angular/core';
import { customers } from './customers';
import { FolderService } from '../../services/folder.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  private _service:FolderService=inject(FolderService);
  ngOnInit(): void {
    this.getAll();
  }
  public getAll():void{
    this._service.getFolder().subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        console.log(error);
      }
    );

  }
  public gridData: any[] = customers;

}
