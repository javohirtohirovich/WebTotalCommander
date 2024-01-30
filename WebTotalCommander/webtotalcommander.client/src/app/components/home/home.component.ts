import { Component, OnInit, inject } from '@angular/core';
import { FolderService } from '../../services/folder.service';
import { FolderCreateViewModel } from '../../services/models/folder/folder.view-create.model';
import { FileViewCreateModel } from '../../services/models/file/file.view-create.model';
import { FileService } from '../../services/file.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

    //Inject FolderService
    private _serviceFolder: FolderService = inject(FolderService);
    private _serviceFile: FileService = inject(FileService);

    //Modal error folder name
    public folderNameError: string = '';
    public fileSourceError: string = '';

    //Variable folder name
    public folderName: string = '';
    public fileSource: File | null = null;

    ngOnInit(): void {
        this.getAll();
    }


    onChange(event: any) {
        debugger;

        if (event.target.files.length > 0) {
            const file:File = event.target.files[0];
            this.fileSource=file;
        }
    }

    //GetAll Folders and Files
    public getAll(): void {
      this._serviceFolder.getFolder().subscribe({
        next: (response) => {
          console.log("Success");
        },
        error: (err) => {
          console.log("Error");

        },
      });
    }

    //Add Folder
    public saveAddChanges(): void {
        if (this.fileSource) {
            const folderViewCreateModel = new FolderCreateViewModel();
            folderViewCreateModel.folderName = this.folderName;
            folderViewCreateModel.folderPath = "javo";
            this._serviceFolder.addFolder(folderViewCreateModel).subscribe({
                next: (response) => {
                    console.log("Success");
                },
                error: (err) => {
                    console.log("Error");

                },
            });
        }

    }

    //Upload File
    public saveFileChanges(): void {
        const fileViewCreateModel = new FileViewCreateModel();
        fileViewCreateModel.file = this.fileSource;
        fileViewCreateModel.filePath = "javo";
        this._serviceFile.addFile(fileViewCreateModel).subscribe({
            next: (response) => {
                console.log("Success");
            },
            error: (err) => {
                console.log("Error");

            },
        });
    }

}
