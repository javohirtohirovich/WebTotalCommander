import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderCreateModel } from "./models/folder/folder.create-model";
import { FolderDeleteModel } from "./models/folder/folder.delete-model";
import { FolderGetAllModel } from "./models/common/folder.getall-model";

@Injectable({ providedIn: "root" })
export class FolderApiService {
    private client: HttpClient = inject(HttpClient);
    private url: string = "https://localhost:7251/api/folder"

    //Folder GetAll
    public getAllFolder(folderPath: string,skip:number,take:number): Observable<FolderGetAllModel> {
        if (folderPath.length === 0) {
            return this.client.get<FolderGetAllModel>(`${this.url}?skip=${skip}&take=${take}`).pipe(
                catchError((error) => {
                    console.error("Error in getFolders:", error);
                    throw error;
                })
            );
        }
        else {
            return this.client.get<FolderGetAllModel>(`${this.url}?folder_path=${folderPath}&pageNumber=${skip}&pageSize=${take}`).pipe(
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

    //Download Folder Zip
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

    //Delete Folder

    public deleteFolder(folder:FolderDeleteModel):Observable<any>{
        return this.client.delete(this.url,{body:folder})
    }

}   