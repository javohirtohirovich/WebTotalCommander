import { Component, OnInit, inject } from '@angular/core';
import { FolderService } from '../../services/folder.service';
import { FolderCreateViewModel } from '../../services/models/folder/folder.view-create.model';
import { FileViewCreateModel } from '../../services/models/file/file.view-create.model';
import { FileService } from '../../services/file.service';
import { ToastrService } from 'ngx-toastr';
import { FolderGetAllViewModel } from '../../services/models/common/folder.getall.view-model';
import { BreadCrumbItem } from "@progress/kendo-angular-navigation";
import {arrowRotateCcwIcon,homeIcon,SVGIcon} from "@progress/kendo-svg-icons";
import { CellClickEvent } from '@progress/kendo-angular-grid';
import { Title } from '@angular/platform-browser';

 
@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
    //Konstruktor
    constructor(private toastr: ToastrService) { }

    //Inject FolderService
    private _serviceFolder: FolderService = inject(FolderService);
    private _serviceFile: FileService = inject(FileService);

    //Variables folder name
    public folderName: string = '';
    public fileSource: File | null = null;

    //Variables Modal error folder name
    public folderNameError: string = '';
    public fileSourceError: string = '';

    //Variables Array Folder GetALL
    public fileData:Array<FolderGetAllViewModel>=[];
    public path:string="";

    //BreadCrumb+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
    private defaultItems: BreadCrumbItem[] = [
        {
          text: "Home",
          title: "Home",
          svgIcon: homeIcon,
        }       
      ];

    public items: BreadCrumbItem[] = [...this.defaultItems];
    public homeIcon: SVGIcon = homeIcon;
    public rotateIcon: SVGIcon = arrowRotateCcwIcon;
    public onItemClick(item: BreadCrumbItem): void {
      const index = this.items.findIndex((e) => e.text === item.text);
      this.items = this.items.slice(0, index + 1);
      this.defaultItems.pop();
      this.getAll();
    }
  
    public refreshBreadCrumb(): void {
        this.path="";
        this.getAll();
      this.items = [...this.defaultItems];
    }

    public toFillBreadCrumb(folderName:string,folderPath:string):void{
        for(let i=0;i<this.fileData.length;i++){
            this.defaultItems.push({text:folderPath,title:folderName})
        }
    }
    
    
  public cellClickHandler(args: CellClickEvent): void {
    if( args.dataItem.extension === "folder" )
    {
      this.defaultItems.push({text:args.dataItem.name,title:args.dataItem.name})
      this.refreshBreadCrumb();
      this.getAll();
    }
    
  }

  public toCollectPath():string{
    let result:string="";
    if(this.defaultItems.length===1){
        return result;
    }
    else{
        for(let i=1;i<this.defaultItems.length;i++)
        {
            result+=`${this.defaultItems[i].text}/`
        }
        return result;
    }
  }
    //BreadCrumb+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=


    //Function NgOnit
    ngOnInit(): void {
        this.getAll();
    }

     //Function (ngOnInit) GetAll Folders and Files
     public getAll(): void {
        const path:string=this.toCollectPath();
        this._serviceFolder.getFolder(path).subscribe({
            next: (response) => {
                this.fileData=response;
            },
            error: (err) => {
                this.toastr.warning('Get all warning!');
            },
        });
    }

    //Function Get select file in fileSource
    public onChange(event: any) {
        if (event.target.files.length > 0) {
            const file: File = event.target.files[0];
            this.fileSource = file;
        }
    }

    //Function (Button) Create Folder
    public saveAddFolder(): void {
        const folderViewCreateModel = new FolderCreateViewModel();
        folderViewCreateModel.folderName = this.folderName;
        folderViewCreateModel.folderPath = "";
        this._serviceFolder.addFolder(folderViewCreateModel).subscribe({
            next: (response) => {
                this.toastr.success('Folder success created!');
            },
            error: (err) => {
                if (err.status == 409) {
                    this.toastr.warning('Folder already exists!');
                } else if (err.status == 404) {
                    this.toastr.warning('Folder path not found!');
                } else {
                    this.toastr.warning('Error during folder create!');
                }
            },
        });
    }

    //Function (Button) Upload file
    public saveUploadFile(): void {
        const fileViewCreateModel = new FileViewCreateModel();
        fileViewCreateModel.file = this.fileSource;
        fileViewCreateModel.filePath = "";
        this._serviceFile.addFile(fileViewCreateModel).subscribe({
            next: (response) => {
                this.toastr.success('File success upload!');
            },
            error: (err) => {
                if (err.status == 409) {
                    this.toastr.warning('File already exists!');
                } else if (err.status == 404) {
                    this.toastr.warning('Folder not found!');
                } else {
                    this.toastr.warning('Error during file upload!');
                };

            },
        });
    }

}
