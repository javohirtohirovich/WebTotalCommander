import { Component, OnInit, inject } from '@angular/core';
import { FolderService } from '../../services/folder.service';
import { FolderCreateViewModel } from '../../services/models/folder/folder.view-create.model';
import { FileViewCreateModel } from '../../services/models/file/file.view-create.model';
import { FileService } from '../../services/file.service';
import { ToastrService } from 'ngx-toastr';
import { FileGetViewModel } from '../../services/models/common/getall.view/file.get.view-model';
import { FolderPaginationViewModel } from '../../services/models/folder/folder.pagination.view-model';
import { BreadCrumbItem } from "@progress/kendo-angular-navigation";



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

    public fileData:Array<FolderPaginationViewModel>=[];

    //Function NgOnit
    ngOnInit(): void {
        this.getAll();
    }

    //Function Get select file in fileSource
    public onChange(event: any) {
        if (event.target.files.length > 0) {
            const file: File = event.target.files[0];
            this.fileSource = file;
        }
    }

    //Function (ngOnInit) GetAll Folders and Files
    public getAll(): void {
        this._serviceFolder.getFolder().subscribe({
            next: (response) => {
                this.fileData=response;
            },
            error: (err) => {
                this.toastr.warning('Get all warning!');
            },
        });
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
