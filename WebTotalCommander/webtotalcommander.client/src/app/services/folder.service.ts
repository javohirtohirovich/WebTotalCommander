import { Injectable, inject } from "@angular/core";
import { FolderApiService } from "../api/folder.api-service";
import { Observable, map } from "rxjs";
import { FolderCreateViewModel } from "./models/folder/folder.view-create.model";
import { FolderGetAllViewModel } from "./models/common/folder.getall.view-model";
import {  FolderGetAllMode } from "../api/models/common/folder.getall-model";

@Injectable({ providedIn: "root" })
export class FolderService {
    //Inject FolderApiService
    private folderApiService: FolderApiService = inject(FolderApiService)


    public getFolder(folderPath:string): Observable<Array<FolderGetAllViewModel>> {
        return this.folderApiService.getAllFolder(folderPath).pipe(
            map(apiModel => this.toModel(apiModel))
        );
    }

    //Function Create Folder
    public addFolder(folder:FolderCreateViewModel):Observable<any>{
        return this.folderApiService.addFolder(folder);
    }

    //Function Download Folder Zip format
    public downloadFolderZip(folderName:string,folderPath:string):Observable<any>{
      return this.folderApiService.downloadFolderZip(folderPath,folderName);  
    }

   
    private toModel(apiModel: Array< FolderGetAllMode>): Array<FolderGetAllViewModel> {
       
        const result:Array<FolderGetAllViewModel>=[];
        for(let i=0;i<apiModel.length;i++){
            const folderGet:FolderGetAllViewModel=new FolderGetAllViewModel();
            folderGet.name=apiModel[i].name;
            folderGet.extension=apiModel[i].extension;
            folderGet.path=apiModel[i].path;
            result.push(folderGet);
        }
        return result;
    }
}