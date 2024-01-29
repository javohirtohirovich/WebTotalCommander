import { Injectable, inject } from "@angular/core";
import { FolderApiService } from "../api/folder.api-service";
import { Observable, map } from "rxjs";
import { FolderViewModel } from "./models/folder/folder.view-model";
import { FolderModel } from "../api/models/folder/folder.model";

@Injectable({providedIn:"root"})
export class FolderService{
   private folderApiService:FolderApiService=inject(FolderApiService) 

   public getFolder(): Observable<FolderViewModel> {
    return this.folderApiService.getAllFolder().pipe(
        map(apiModel => this.toModel(apiModel))
    );
}


   private toModel(apiModel:FolderModel):FolderViewModel{
    const result:FolderViewModel=new FolderViewModel();
    result.files=apiModel.files;
    result.folderNames=apiModel.folderNames;
    return result;
   }
}