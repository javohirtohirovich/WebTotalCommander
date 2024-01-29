import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderModel } from "./models/folder/folder.model";

@Injectable({providedIn:"root"})
export class FolderApiService{
    private client: HttpClient=inject(HttpClient);
    public getAllFolder():Observable<FolderModel>{
        const url="https://localhost:7251/api/folder?folder_path=javo&folder_name=islom"
        return this.client.get<FolderModel>(url).pipe(
            catchError((error) => {
                console.error("Error in getFolders:", error);
                throw error;
              })
        );
    }

}