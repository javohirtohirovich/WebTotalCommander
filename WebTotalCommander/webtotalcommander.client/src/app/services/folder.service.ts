import { Injectable, inject } from "@angular/core";
import { FolderApiService } from "../api/folder.api-service";
import { Observable, map } from "rxjs";
import { FolderViewModel } from "./models/folder/folder.view-model";
import { FolderModel } from "../api/models/folder/folder.model";
import { FolderCreateViewModel } from "./models/folder/folder.view-create.model";

@Injectable({ providedIn: "root" })
export class FolderService {
    //Inject FolderApiService
    private folderApiService: FolderApiService = inject(FolderApiService)

    //Function GetAll Folders and Files
    public getFolder(): Observable<FolderViewModel> {
        return this.folderApiService.getAllFolder().pipe(
            map(apiModel => this.toModel(apiModel))
        );
    }

    //Function Create Folder
    public addFolder(folder:FolderCreateViewModel):Observable<any>{
        return this.folderApiService.addFolder(folder);
    }

    //Function ToModel FolderModel to FolderViewModel
    private toModel(apiModel: FolderModel): FolderViewModel {
        const result: FolderViewModel = new FolderViewModel();
        result.files = apiModel.files;
        result.folderNames = apiModel.folderNames;
        return result;
    }
}