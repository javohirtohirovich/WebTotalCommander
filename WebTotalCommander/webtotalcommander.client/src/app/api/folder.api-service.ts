import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderModel } from "./models/folder/folder.model";
import { FolderCreateModel } from "./models/folder/folder.create-model";

@Injectable({providedIn:"root"})
export class FolderApiService{
    private client: HttpClient=inject(HttpClient);
    private url:string="https://localhost:7251/api/folder"

    //GetAll Folders and Files
    public getAllFolder():Observable<FolderModel>{
        return this.client.get<FolderModel>(`${this.url}?folder_path=javo&folder_name=islom`).pipe(
            catchError((error) => {
                console.error("Error in getFolders:", error);
                throw error;
              })
        );
    }

    //Create Folder
    public addFolder(folder:FolderCreateModel):Observable<any>{
        return this.client.post(this.url,folder)
    }

}   