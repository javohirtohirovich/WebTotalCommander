//Angular local Libraries
import { Component, OnInit, inject } from '@angular/core';

//begin:: Kendo

//Kendo BreadCrumb
import { BreadCrumbItem } from "@progress/kendo-angular-navigation";  
//Kenod Grid Libraries
import { CellClickEvent, PageChangeEvent, PagerPosition, PagerType } from '@progress/kendo-angular-grid';

//end:: Kendo

//Toastr
import { ToastrService } from 'ngx-toastr';
//Service Folder
import { FolderService } from '../../services/folder.service';
//ServiceFile
import { FileService } from '../../services/file.service';
//ViewModels Common
import { GridTDataView } from '../../services/models/folder/grid.data.view';
import { FolderGetAllViewModel } from '../../services/models/common/folder.getall.view-model';
//ViewModel File
import { FileViewCreateModel } from '../../services/models/file/file.view-create.model';
import { FileViewDeleteModel } from '../../services/models/file/file.view-delete.model';
import { FileViewEditModel } from '../../services/models/file/file.view-edit.model';
//ViewModel Folder
import { FolderCreateViewModel } from '../../services/models/folder/folder.view-create.model';
import { FolderDeleteViewModel } from '../../services/models/folder/folder.view-delete.model';
import { KendoIcons } from '../helpers/get-icons';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
    //Konstruktor
    constructor(private toastr: ToastrService) { }

    //Inject FolderService, FileService, KendoIcons (helper)
    private _serviceFolder: FolderService = inject(FolderService);
    private _serviceFile: FileService = inject(FileService);
    public _iconsKendo:KendoIcons=inject(KendoIcons);

    //Variables folder name
    public folderName: string = '';
    public fileSource: File | null = null;
    
    //Variables Array Folder GetALL
    public fileData: FolderGetAllViewModel = new FolderGetAllViewModel();
    public path: string = "";

    //BreadCrumb Defult Value
    private defaultItems: BreadCrumbItem[] = [
        {
            text: "Home",
            title: "Home",
            svgIcon: this._iconsKendo.homeIcon,
        }
    ];

    //Array BreadCrumb
    public items: BreadCrumbItem[] = [...this.defaultItems];

    //Variables for Pagination
    public totalCount = 1;
    public type: PagerType = "numeric";
    public buttonCount = 5;
    public info = true;
    public pageSizes = true;
    public previousNext = true;
    public position: PagerPosition = "bottom";
    public gridView: GridTDataView = new GridTDataView();
    public pageSize = 5;
    public skip = 0;

    //Variables Edit Txt File (Modal)
    public txtFileContent: string = '';
    public opened = false;
    public fileNameToEdit: string = '';

    //Variables Delete Folder (Modal)
    public openedFolder = false;
    public folderNameDelete: string = "";

    //Variables Delete File (Modal)
    public openedFile = false;
    public fileNameDelete: string = "";

    //Functionn For Pagination (Change page)
    public pageChange({ skip, take }: PageChangeEvent): void {
        this.skip = skip;
        this.pageSize = take;
        this.getAll(this.skip, this.pageSize)
    }

    //Function NgOnit
    ngOnInit(): void {
        this.getAll(this.skip, this.pageSize);
    }

    //Function (ngOnInit) GetAll Folders and Files
    public getAll(skip: number, take: number): void {

        const path: string = this.toCollectPath();
        this._serviceFolder.getFolder(path, skip, take).subscribe({
            next: (response) => {
                this.fileData = response;
                this.gridView.data = response.folderFile;
                this.gridView.total = response.paginationMetaData.totalItems;
                this.totalCount = response.paginationMetaData.totalItems;
                this.toastr.success('Success!');
            },
            error: (err) => {
                this.toastr.warning('Get all warning!');
            },
        });
    }

    //begin:: Edit Modal ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //Function (button) Close EditModal
    public close(): void {
        this.opened = false;
    }

    //Function (button) Open EditModal
    public openEditTxtModal(fileName: string): void {
        this.opened = true;
        this.fileNameToEdit = fileName;
        this._serviceFile.getTxtFile(this.toCollectPath() + fileName).subscribe({
            next: (response) => {
                // Assuming the response is a Blob containing the text file content
                const reader = new FileReader();
                reader.onload = () => {
                    this.txtFileContent = reader.result as string;
                };
                reader.readAsText(response);
            },
            error: (err) => {
                this.toastr.warning('Error retrieving file content!');
            },
        });
    }

    //Function (button) Save EditModal
    public submit(): void {
        // Call the editTxtFile method with the filename and updated content
        const fileEditModel = new FileViewEditModel();

        fileEditModel.filePath = this.toCollectPath() + this.fileNameToEdit;
        fileEditModel.file = new File([this.txtFileContent], this.fileNameToEdit)


        this._serviceFile.editTxtFile(fileEditModel).subscribe({
            next: (response) => {
                this.toastr.success('File content updated successfully!');
                this.close();
            },
            error: (err) => {
                this.toastr.warning('Error updating file content!');
            },
        });
        this.close();
    }

    //end:: Edit Modal--------------------------------------------------------------------------

    //begin:: Delete Folder Modal+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public closeDeleteModalFolder(status: string): void {
        if (status === 'yes') {
            const folder: FolderDeleteViewModel = new FolderDeleteViewModel();
            folder.folderName = this.folderNameDelete;
            folder.folderPath = this.toCollectPath();
            this._serviceFolder.deleteFolder(folder).subscribe({
                next: (response) => {
                    this.toastr.success('Delete folder success!');
                    this.getAll(this.skip,this.pageSize);
                },
                error: (err) => {
                    this.toastr.warning('Delete folder warning!');
                },
            });

            this.openedFolder = false;
        }
        else if (status === 'no') {
            this.openedFolder = false;
        }
        else if (status === 'cancel') {
            this.openedFolder = false;
        }

    }
    public openDeleteModalFolder(folderName: string): void {
        this.folderNameDelete = folderName;
        this.openedFolder = true;
    }

    //end:: Delete Folder Modal-----------------------------------------------------------------------

    //begin:: Delete File Modal+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public closeDeleteModalFile(status: string): void {

        if (status === 'yes') {
            const file: FileViewDeleteModel = new FileViewDeleteModel();
            file.fileName = this.fileNameDelete;
            file.filePath = this.toCollectPath();
            this._serviceFile.deleteFile(file).subscribe({
                next: (response) => {
                    this.toastr.success('Delete file success!');
                    this.getAll(this.skip,this.pageSize);
                },
                error: (err) => {
                    this.toastr.warning('Delete file warning!');
                },
            });

            this.openedFile = false;
        }
        else if (status === 'no') {
            this.openedFile = false;
        }
        else if (status === 'cancel') {
            this.openedFile = false;
        }


    }
    public openDeleteModalFile(fileName: string): void {
        this.fileNameDelete = fileName;
        this.openedFile = true;
    }
    //end:: Delete File Modal----------------------------------------------------------------------------

    //begin:: BreadCrumb+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Function BreadCrumb Item click
    public onItemClick(item: BreadCrumbItem): void {
        const index = this.items.findIndex((e) => e.text === item.text);
        this.items = this.items.slice(0, index + 1);
        this.defaultItems=this.defaultItems.slice(0,index+1);
        this.getAll(this.skip,this.pageSize);
    }

    // Function (Button) Refresh button
    public refreshBreadCrumb(): void {
        this.path = "";
        this.getAll(this.skip,this.pageSize);
        this.items = [...this.defaultItems];
    }

    // Function to fill BreadCrumb (Click folder folder name add bread crumb)
    public toFillBreadCrumb(folderName: string, folderPath: string): void {
        for (let i = 0; i < this.fileData.folderFile.length; i++) {
            this.defaultItems.push({ text: folderPath, title: folderName })
        }
    }
    //end:: BreadCrumb----------------------------------------------------------------------------------

    public cellArgs!: CellClickEvent;
    //Function (Tables Items Folder and File click)
    public onDblClick(): void {
        if (this.cellArgs.dataItem.extension === "folder") {
            this.defaultItems.push({ text: this.cellArgs.dataItem.name, title: this.cellArgs.dataItem.name })
            this.refreshBreadCrumb();
            this.getAll(this.skip,this.pageSize);
        }
        else {
            const path: string = `${this.toCollectPath()}${this.cellArgs.dataItem.name}`
            this.downloadFile(path, this.cellArgs.dataItem.name);
        }

    }

    public cellClickHandler(args: CellClickEvent): void {
        this.cellArgs = args;
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
                    this.getAll(this.skip,this.pageSize);
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
                    this.getAll(this.skip,this.pageSize);
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
        const folderPath: string = this.toCollectPath();
        this._serviceFolder.downloadFolderZip(folderName, folderPath).subscribe(
            (response: Blob) => {
                // Create a Blob from the file data
                const blob = new Blob([response], { type: `application/zip` });

                // Create a link element
                const link = document.createElement('a');

                // Set the download attribute and create a URL for the blob
                link.download = `${folderName}.zip`;
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
