//Angular local Libraries
import { Component, OnInit, inject } from '@angular/core';

//begin:: Kendo

//Kendo BreadCrumb
import { BreadCrumbItem } from "@progress/kendo-angular-navigation";
//Kenod Grid Libraries
import { CellClickEvent, PageChangeEvent, PagerPosition, PagerType } from '@progress/kendo-angular-grid';

//Filter
import { CompositeFilterDescriptor, FilterDescriptor, SortDescriptor } from '@progress/kendo-data-query';

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
import { FolderFileViewModel } from '../../services/models/common/folder.file.view-model';

//ViewModel Filter
import { SubFilter } from '../models/sub-filter';
import { GridState } from '../models/grid-state';
import { SortViewModel } from '../../services/models/common/sort.view-model';

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
    public _iconsKendo: KendoIcons = inject(KendoIcons);

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

    //Variable (get event when click folder or file)
    public cellArgs!: CellClickEvent;

    //Variable Loader
    public isLoading: boolean = false;

    //Arrays Back and Forwar (buttons) history
    public backHistory: Array<BreadCrumbItem[]> = [];

    public forwardHistory: Array<BreadCrumbItem[]> = [];

    //Function NgOnit
    ngOnInit(): void {
        this.getAll(this.skip, this.pageSize);
    }

    //Function (ngOnInit) GetAll Folders and Files
    public getAll(skip: number, take: number): void {
        this.isLoading = true;
        const filter = {
            'Filter.Logic': 'and',
            'Filter.Filters': this.convertFilters(this.gridState.filter),
        };

        const sortViewModel: SortViewModel = new SortViewModel();
        sortViewModel.dir = this.sort[0].dir;
        sortViewModel.field = this.sort[0].field;

        const path: string = this.toCollectPath();
        this._serviceFolder.getFolder(path, skip, take, sortViewModel, filter).subscribe({

            next: (response) => {
                this.fileData = response;
                this.gridView.data = response.folderFile;
                this.gridView.total = response.paginationMetaData.totalItems;
                if (path.length !== 0) {
                    const exitRow: FolderFileViewModel = new FolderFileViewModel();
                    exitRow.name = "...";
                    exitRow.extension = "";
                    exitRow.path = "";

                    this.gridView.data.unshift(exitRow);
                }
                this.isLoading = false;
            },
            error: (err) => {
                this.toastr.warning('Get all warning!');
                this.isLoading = false;
            },
        });
    }

    //Functionn For Pagination (Change page)
    public pageChange({ skip, take }: PageChangeEvent): void {
        this.skip = skip;
        this.pageSize = take;
        this.getAll(this.skip, this.pageSize)
    }

    public gridState: GridState = this.creteInitialState();

    private creteInitialState(): GridState {
        return {
            filter: {
                filters: [],
                logic: 'and',
            },
            sort: null,
        };
    }

    private convertFilters(filter: CompositeFilterDescriptor): SubFilter[] {
        const result: SubFilter[] = [];
        for (let i = filter.filters.length - 1; i >= 0; i--) {
            const currentFilter: CompositeFilterDescriptor = <any>filter.filters[i];
            if (!currentFilter || !currentFilter.logic) {
                filter.filters.splice(i, 1);
            }
        }
        for (let i = 0; i < filter.filters.length; i++) {
            const currentFilter: CompositeFilterDescriptor = <any>filter.filters[i];
            if (currentFilter)
                result.push({
                    logic: currentFilter.logic,
                    filters:
                        currentFilter.filters?.map((x) => {
                            const descriptor: FilterDescriptor = <any>x;
                            let strVal;
                            if (
                                typeof descriptor.value == 'object' &&
                                descriptor.value.constructor == Date
                            ) {
                                if (
                                    descriptor.operator === 'lte' ||
                                    descriptor.operator === 'gt'
                                ) {
                                    const oneDayInMs = 24 * 60 * 60 * 1000;
                                    strVal = new Date(
                                        descriptor.value.getTime() + oneDayInMs
                                    ).toISOString();
                                } else {
                                    strVal = descriptor.value.toISOString();
                                }
                            } else {
                                strVal = descriptor.value;
                            }
                            return {
                                field: <string>(<any>descriptor.field),
                                operator: <string>(<any>descriptor.operator),
                                value: strVal,
                            };
                        }) || [],
                });
        }
        return result;
    }

    public filterChange(ev: CompositeFilterDescriptor): void {
        if (ev) {
            this.gridState.filter = ev;
        } else {
            this.gridState.filter = {
                logic: 'and',
                filters: [],
            };
        }
        this.skip = 0;
        this.getAll(this.skip, this.pageSize);
    }

    public sort: SortDescriptor[] = [
        {
            field: "name",
            dir: undefined,
        },
    ];
    public sortChange(sort: SortDescriptor[]): void {
        this.sort = sort;
        this.getAll(this.skip, this.pageSize)
    }

    //begin:: Edit Modal ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //Function (button) Close EditModal
    public close(): void {
        this.opened = false;
    }

    //Function (button) Open EditModal
    public openEditTxtModal(fileName: string): void {
        this.isLoading = true;
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
                this.isLoading = false;
            },
            error: (err) => {
                this.toastr.warning('Error retrieving file content!');
                this.isLoading = false;
            },
        });
    }

    //Function (button) Save EditModal
    public submit(): void {
        this.isLoading = true;
        // Call the editTxtFile method with the filename and updated content
        const fileEditModel = new FileViewEditModel();

        fileEditModel.filePath = this.toCollectPath() + this.fileNameToEdit;
        fileEditModel.file = new File([this.txtFileContent], this.fileNameToEdit)


        this._serviceFile.editTxtFile(fileEditModel).subscribe({
            next: (response) => {
                this.isLoading = false;
                this.toastr.success('File content updated successfully!');
                this.close();

            },
            error: (err) => {
                this.isLoading = false;
                this.toastr.warning('Error updating file content!');
            },
        });
        this.close();
    }

    //end:: Edit Modal--------------------------------------------------------------------------

    //begin:: Delete Folder Modal+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public closeDeleteModalFolder(status: string): void {
        if (status === 'yes') {
            this.isLoading = true
            const folder: FolderDeleteViewModel = new FolderDeleteViewModel();
            folder.folderName = this.folderNameDelete;
            folder.folderPath = this.toCollectPath();
            this._serviceFolder.deleteFolder(folder).subscribe({
                next: (response) => {
                    this.toastr.success('Delete folder success!');
                    this.getAll(this.skip, this.pageSize);
                    this.isLoading = false;
                },
                error: (err) => {
                    this.isLoading = false;
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
            this.isLoading = true;
            const file: FileViewDeleteModel = new FileViewDeleteModel();
            file.fileName = this.fileNameDelete;
            file.filePath = this.toCollectPath();
            this._serviceFile.deleteFile(file).subscribe({
                next: (response) => {
                    this.toastr.success('Delete file success!');
                    this.getAll(this.skip, this.pageSize);
                    this.isLoading = false;
                },
                error: (err) => {
                    this.isLoading = false;
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
        this.backHistory.push(this.defaultItems.slice());
        this.items = this.items.slice(0, index + 1);
        this.defaultItems = this.defaultItems.slice(0, index + 1);
        this.skip = 0;
        this.getAll(this.skip, this.pageSize);
    }

    // Function (Button) Refresh button
    public refreshBreadCrumb(): void {
        this.path = "";
        this.getAll(this.skip, this.pageSize);
        this.items = [...this.defaultItems];
    }
    //end:: BreadCrumb----------------------------------------------------------------------------------

    //Function (Tables Items Folder and File dbclick)
    public onDblClick(): void {
        if (this.cellArgs.dataItem.extension === "folder") {
            this.backHistory.push(this.defaultItems.slice());
            this.defaultItems.push({ text: this.cellArgs.dataItem.name, title: this.cellArgs.dataItem.name })
            this.refreshBreadCrumb();
        }
        else if (this.cellArgs.dataItem.extension === "") {
            this.backHistory.push(this.defaultItems.slice());
            this.defaultItems.pop();
            this.skip = 0;
            this.refreshBreadCrumb();
        }
        else {
            const path: string = `${this.toCollectPath()}${this.cellArgs.dataItem.name}`
            this.downloadFile(path, this.cellArgs.dataItem.name);
        }

    }

    //Function get event when click folder or file
    public cellClickHandler(args: CellClickEvent): void {
        this.cellArgs = args;
    }

    //Fuction make path
    public toCollectPath(): string {
        let result: string = "";
        if (this.defaultItems.length === 1) {
            return result
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
                    this.getAll(this.skip, this.pageSize);
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
            this.isLoading = true;
            const fileViewCreateModel = new FileViewCreateModel();
            fileViewCreateModel.file = this.fileSource;
            fileViewCreateModel.filePath = this.toCollectPath();
            this._serviceFile.addFile(fileViewCreateModel).subscribe({
                next: (response) => {
                    this.toastr.success('File success upload!');
                    this.isLoading = false;
                    this.getAll(this.skip, this.pageSize);
                },
                error: (err) => {
                    this.isLoading = false;
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
        this.isLoading = true;
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
                this.isLoading = false;
                this.toastr.success('File download successful!');
            },
            (error) => {
                this.isLoading = false;
                this.toastr.warning('File download error!');
            }
        );
    }

    //Download Folder Zip
    public downloadFolderZip(folderName: string): void {
        const folderPath: string = this.toCollectPath();
        this.isLoading = true;
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
                this.isLoading = false;
            },
            (error) => {
                this.isLoading = false;
                this.toastr.warning('Folder download error!');
            }
        );
    }

    //Function Back (button)
    public backFolder(): void {
        if (this.backHistory.length > 0) {
            let Value: BreadCrumbItem[] = this.backHistory.pop()!;
            this.forwardHistory.push(this.defaultItems.slice());
            this.defaultItems = Value.slice();
            this.refreshBreadCrumb();
        }
    }

    //Function Forward (button)
    public forwardFolder(): void {
        if (this.forwardHistory.length > 0) {
            let value: BreadCrumbItem[] = this.forwardHistory.pop()!;
            this.backHistory.push(this.defaultItems.slice());
            this.defaultItems = value.slice();
            this.refreshBreadCrumb();
        }
    }

    //Up (button)
    public upFolder(): void {
        if (this.defaultItems.length > 1) {
            this.backHistory.push(this.defaultItems.slice());
            this.defaultItems.pop();
            this.skip = 0;
            this.refreshBreadCrumb();
        }
    }
}
