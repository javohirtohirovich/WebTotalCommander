import { Injectable, inject } from "@angular/core";
import { FolderApiService } from "../api/folder.api-service";
import { Observable, map } from "rxjs";
import { FolderCreateViewModel } from "./models/folder/folder.view-create.model";
import { FolderGetAllViewModel } from "./models/common/folder.getall.view-model";
import { FolderDeleteViewModel } from "./models/folder/folder.view-delete.model";
import { FolderGetAllModel } from "../api/models/common/folder.getall-model";
import { FolderFileViewModel } from "./models/common/folder.file.view-model";
import { PaginationMetaDataView } from "./models/common/pagination.data";

@Injectable({ providedIn: "root" })
export class FolderService {
    //Variable Inject FolderApiService
    private folderApiService: FolderApiService = inject(FolderApiService)

    //Function (request)
    public getFolder(folderPath: string, skip: number, take: number): Observable<FolderGetAllViewModel> {
        return this.folderApiService.getAllFolder(folderPath, skip, take).pipe(
            map(apiModel => this.toModel(apiModel))
        );
    }

    //Function (request) Create Folder
    public addFolder(folder: FolderCreateViewModel): Observable<any> {
        return this.folderApiService.addFolder(folder);
    }

    //Function (request) Download Folder Zip format
    public downloadFolderZip(folderName: string, folderPath: string): Observable<any> {
        return this.folderApiService.downloadFolderZip(folderPath, folderName);
    }

    //Function (request) Delete Folder
    public deleteFolder(folder: FolderDeleteViewModel): Observable<any> {
        return this.folderApiService.deleteFolder(folder);
    }

    //Function FolderGetAllModel to FolderGetAllViewModel
    private toModel(apiModel: FolderGetAllModel): FolderGetAllViewModel {

        const result: FolderGetAllViewModel = new FolderGetAllViewModel();
        for (let i = 0; i < apiModel.folderFile.length; i++) {
            const folderFileModel: FolderFileViewModel = new FolderFileViewModel();
            folderFileModel.name = apiModel.folderFile[i].name;
            folderFileModel.extension = apiModel.folderFile[i].extension;
            folderFileModel.path = apiModel.folderFile[i].path;
            
            result.folderFile.push(folderFileModel);
        }
        const pageData: PaginationMetaDataView = new PaginationMetaDataView();
        pageData.currentPage = apiModel.paginationMetaData.currentPage;
        pageData.hasNext = apiModel.paginationMetaData.hasNext;
        pageData.hasPrevious = apiModel.paginationMetaData.hasPrevious;
        pageData.pageSize = apiModel.paginationMetaData.pageSize;
        pageData.totalItems = apiModel.paginationMetaData.totalItems;
        pageData.totalPages = apiModel.paginationMetaData.totalPages;

        result.paginationMetaData = pageData;

        return result;
    }
}