import { Injectable, inject } from "@angular/core";
import { FolderApiService } from "../api/folder.api-service";
import { Observable, map } from "rxjs";
import { FolderCreateViewModel } from "./models/folder/folder.view-create.model";
import { FolderGetAllModel } from "../api/models/common/getall/folder.get-all.model";
import { FolderGetAllViewModel } from "./models/common/getall.view/folder.getall.view-model";
import { FileGetViewModel } from "./models/common/getall.view/file.get.view-model";
import { FolderGetViewModel } from "./models/common/getall.view/folder.get.view-model";

@Injectable({ providedIn: "root" })
export class FolderService {
    //Inject FolderApiService
    private folderApiService: FolderApiService = inject(FolderApiService)

    //Function GetAll Folders and Files
    public getFolder(): Observable<FolderGetAllViewModel> {
        return this.folderApiService.getAllFolder().pipe(
            map(apiModel => this.toModel(apiModel))
        );
    }

    //Function Create Folder
    public addFolder(folder:FolderCreateViewModel):Observable<any>{
        return this.folderApiService.addFolder(folder);
    }

    //Function ToModel FolderModel to FolderViewModel
    private toModel(apiModel: FolderGetAllModel): FolderGetAllViewModel {
        const result: FolderGetAllViewModel = new FolderGetAllViewModel();
        if(apiModel.files){
            for(let i=0;i<apiModel.files.length;i++){
                const fileGet:FileGetViewModel= new FileGetViewModel();
                fileGet.fileName=apiModel.files[i].fileName;
                fileGet.fileExtension=apiModel.files[i].fileExtension;
                fileGet.filePath=apiModel.files[i].filePath;
    
                result.files.push(fileGet);
            }
        }
        if(apiModel.folders){
            for(let i=0;i<apiModel.folders.length;i++){
                const folderGet:FolderGetViewModel= new FolderGetViewModel();
                folderGet.folderName=apiModel.folders[i].folderName;
                folderGet.folderPath=apiModel.folders[i].folderPath;
    
                result.folders.push(folderGet);
            }
        }
       
        console.log(result);
        
        return result;
    }
}