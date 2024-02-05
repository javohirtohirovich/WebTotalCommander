import { Component, OnInit, inject } from '@angular/core';
import { FolderService } from '../../services/folder.service';
import { FolderCreateViewModel } from '../../services/models/folder/folder.view-create.model';
import { FileViewCreateModel } from '../../services/models/file/file.view-create.model';
import { FileService } from '../../services/file.service';
import { ToastrService } from 'ngx-toastr';
import { FolderGetAllViewModel } from '../../services/models/common/folder.getall.view-model';
import { BreadCrumbItem } from "@progress/kendo-angular-navigation";
import {
    arrowRotateCcwIcon, homeIcon, SVGIcon, filePdfIcon, fileExcelIcon, fileWordIcon,
    fileImageIcon, fileTxtIcon, fileAudioIcon, fileTypescriptIcon, fileVideoIcon, filePptIcon, folderIcon, exeIcon, fileProgrammingIcon, xIcon, fileZipIcon
} from "@progress/kendo-svg-icons";

import { CellClickEvent } from '@progress/kendo-angular-grid';

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
    public fileData: Array<FolderGetAllViewModel> = [];
    public path: string = "";

    //BreadCrumb
    private defaultItems: BreadCrumbItem[] = [
        {
            text: "Home",
            title: "Home",
            svgIcon: homeIcon,
        }
    ];

    //
    public items: BreadCrumbItem[] = [...this.defaultItems];
    public homeIcon: SVGIcon = homeIcon;
    public rotateIcon: SVGIcon = arrowRotateCcwIcon;

    //FileIcon Dictionary
    private fileIcons: { [key: string]: SVGIcon } = {
        'default': xIcon,
        'folder': folderIcon, // You can change 'folder' to any other extension if needed
        '.pdf': filePdfIcon,
        '.jpg': fileImageIcon,
        '.jpeg': fileImageIcon,
        '.png': fileImageIcon,
        '.gif': fileImageIcon,
        '.xlsx': fileExcelIcon,
        '.xls': fileExcelIcon,
        '.docx': fileWordIcon,
        '.doc': fileWordIcon,
        '.txt': fileTxtIcon,
        '.mp4': fileVideoIcon,
        '.exe': exeIcon,
        '.py': fileProgrammingIcon,
        '.js': fileProgrammingIcon,
        '.mp3': fileAudioIcon,
        '.ts': fileTypescriptIcon,
        '.zip': fileZipIcon

    };



    public getIconForExtension(extension: string): SVGIcon {
        // Check if the extension exists in the fileIcons object, if not, use the default icon        
        return this.fileIcons[extension.toLowerCase()] || fileTypescriptIcon;
    }


    //Function NgOnit
    ngOnInit(): void {
        this.getAll();
    }

    //Function (ngOnInit) GetAll Folders and Files
    public getAll(): void {
        const path: string = this.toCollectPath();
        this._serviceFolder.getFolder(path).subscribe({
            next: (response) => {
                this.fileData = response;
            },
            error: (err) => {
                this.toastr.warning('Get all warning!');
            },
        });
    }

    //Function BreadCrumb Item click
    public onItemClick(item: BreadCrumbItem): void {
        const index = this.items.findIndex((e) => e.text === item.text);
        this.items = this.items.slice(0, index + 1);
        this.defaultItems.pop();
        this.getAll();
    }

    // Function (Button) Refresh button
    public refreshBreadCrumb(): void {
        this.path = "";
        this.getAll();
        this.items = [...this.defaultItems];
    }

    // Function to fill BreadCrumb (Click folder folder name add bread crumb)
    public toFillBreadCrumb(folderName: string, folderPath: string): void {
        for (let i = 0; i < this.fileData.length; i++) {
            this.defaultItems.push({ text: folderPath, title: folderName })
        }
    }

    //Function (Folder and File click)
    public cellClickHandler(args: CellClickEvent): void {
        if (args.dataItem.extension === "folder") {
            this.defaultItems.push({ text: args.dataItem.name, title: args.dataItem.name })
            this.refreshBreadCrumb();
            this.getAll();
        }
        else {
            const path: string = `${this.toCollectPath()}${args.dataItem.name}`
            this.downloadFile(path, args.dataItem.name);
        }

    }

    //Fuction make path
    public toCollectPath(): string {
        let result: string = "";
        if (this.defaultItems.length === 1) {
            return result;
        }
        else {
            for (let i = 1; i < this.defaultItems.length; i++) {
                result += `${this.defaultItems[i].text}/`
            }
            return result;
        }
    }


    //Function Get select file in fileSource
    public onChange(event: any) {
        if (event.target.files.length > 0) {
            const file: File = event.target.files[0];
            this.fileSource = file;
        }
        else {
            this.toastr.warning("Please select file!");
        }
    }

    //Function (Button) Create Folder
    public saveAddFolder(): void {
        if (this.folderName) {
            const folderViewCreateModel = new FolderCreateViewModel();
            folderViewCreateModel.folderName = this.folderName;
            folderViewCreateModel.folderPath = this.toCollectPath();
            this._serviceFolder.addFolder(folderViewCreateModel).subscribe({
                next: (response) => {
                    this.toastr.success('Folder success created!');
                    this.getAll();
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
        else {
            this.toastr.warning("Please enter a folder name!");
        }

    }

    //Function (Button) Upload file
    public saveUploadFile(): void {
        if (this.fileSource) {
            const fileViewCreateModel = new FileViewCreateModel();
            fileViewCreateModel.file = this.fileSource;
            fileViewCreateModel.filePath = this.toCollectPath();
            this._serviceFile.addFile(fileViewCreateModel).subscribe({
                next: (response) => {
                    this.toastr.success('File success upload!');
                    this.getAll();
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
        else {
            this.toastr.warning("Please select file!");
        }

    }

    //Function Download file
    public downloadFile(filePath: string, fileName: string): void {
        this._serviceFile.downloadFile(filePath).subscribe(
            (response: Blob) => {
                // Create a Blob from the file data
                const blob = new Blob([response], { type: `application/octet-stream` });

                // Create a link element
                const link = document.createElement('a');

                // Set the download attribute and create a URL for the blob
                link.download = `${fileName}`;
                link.href = window.URL.createObjectURL(blob);

                // Append the link to the body and trigger the click event
                document.body.appendChild(link);
                link.click();

                // Clean up: remove the link and revoke the URL
                document.body.removeChild(link);
                window.URL.revokeObjectURL(link.href);
                this.toastr.success('File download successful!');
            },
            (error) => {
                this.toastr.warning('File download error!');
            }
        );
    }

    //Download Folder Zip
    public downloadFolderZip(folderName: string): void {
        debugger;
        const folderPath:string=this.toCollectPath();
        this._serviceFolder.downloadFolderZip(folderName, folderPath).subscribe(
            (response: Blob) => {
                // Create a Blob from the file data
                const blob = new Blob([response], { type: `application/zip` });

                // Create a link element
                const link = document.createElement('a');

                // Set the download attribute and create a URL for the blob
                link.download = `${folderName+'.zip'}`;
                link.href = window.URL.createObjectURL(blob);

                // Append the link to the body and trigger the click event
                document.body.appendChild(link);
                link.click();

                // Clean up: remove the link and revoke the URL
                document.body.removeChild(link);
                window.URL.revokeObjectURL(link.href);
                this.toastr.success('Folder download successful!');
            },
            (error) => {
                this.toastr.warning('Folder download error!');
            }
        );
    }


}
