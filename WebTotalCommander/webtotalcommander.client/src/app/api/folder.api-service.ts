import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderCreateModel } from "./models/folder/folder.create-model";
import { FolderGetAllModel } from "./models/common/getall/folder.get-all.model";
import { FolderPaginationModel } from "./models/folder/folder.pagination-model";

@Injectable({providedIn:"root"})
export class FolderApiService{
    private client: HttpClient=inject(HttpClient);
    private url:string="https://localhost:7251/api/folder"

    //GetAll Folders and Files
    // public getAllFolder():Observable<FolderGetAllModel>{
    //     return this.client.get<FolderGetAllModel>(`${this.url}?folder_path=javo&folder_name=islom`).pipe(
    //         catchError((error) => {
    //             console.error("Error in getFolders:", error);
    //             throw error;
    //           })
    //     );
    // }

    public getAllFolder():Observable<Array<FolderPaginationModel>>{
        return this.client.get<Array<FolderPaginationModel>>(`${this.url}?folder_path=""&folder_name=islom`).pipe(
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