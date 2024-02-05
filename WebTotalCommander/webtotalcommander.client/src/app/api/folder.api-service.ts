import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderCreateModel } from "./models/folder/folder.create-model";
import { FolderGetAllMode } from "./models/common/folder.getall-model";

@Injectable({ providedIn: "root" })
export class FolderApiService {
    private client: HttpClient = inject(HttpClient);
    private url: string = "https://localhost:7251/api/folder"

    public getAllFolder(folderPath: string): Observable<Array<FolderGetAllMode>> {
        if (folderPath.length === 0) {
            return this.client.get<Array<FolderGetAllMode>>(`${this.url}`).pipe(
                catchError((error) => {
                    console.error("Error in getFolders:", error);
                    throw error;
                })
            );
        }
        else {
            return this.client.get<Array<FolderGetAllMode>>(`${this.url}?folder_path=${folderPath}`).pipe(
                catchError((error) => {
                    console.error("Error in getFolders:", error);
                    throw error;
                })
            );
        }

    }

    //Create Folder
    public addFolder(folder: FolderCreateModel): Observable<any> {
        return this.client.post(this.url, folder)
    }

    public downloadFolderZip(folderPath:string,folderName:string):Observable<any>
    {
        if(folderPath.length===0){
            return this.client.get(`${this.url}/zip?folderName=${folderName}`,{
                responseType: 'blob'
            });
        }
        else{
            return this.client.get(`${this.url}/zip?folderPath=${folderPath}&folderName=${folderName}`, {
                responseType: 'blob'
            });
        }
       
    }

}   