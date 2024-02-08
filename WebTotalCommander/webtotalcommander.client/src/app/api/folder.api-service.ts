import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderCreateModel } from "./models/folder/folder.create-model";
import { FolderDeleteModel } from "./models/folder/folder.delete-model";
import { FolderGetAllModel } from "./models/common/folder.getall-model";

@Injectable({ providedIn: "root" })
export class FolderApiService {

    //Variable HttpClient Inject
    private client: HttpClient = inject(HttpClient);
    //Variable Backend URL
    private url: string = "https://localhost:7251/api/folder"

    //Function (request) GetAll Folders and Files
    public getAllFolder(folderPath: string,skip:number,take:number): Observable<FolderGetAllModel> {
        //If Folder Path empty
        if (folderPath.length === 0) {
            return this.client.get<FolderGetAllModel>(`${this.url}?skip=${skip}&take=${take}`).pipe(
                catchError((error) => {
                    throw error;
                })
            );
        }
        //If Folder Path full
        else {
            return this.client.get<FolderGetAllModel>(`${this.url}?folder_path=${folderPath}&pageNumber=${skip}&pageSize=${take}`).pipe(
                catchError((error) => {
                    throw error;
                })
            );
        }
    }

    //Function (request) Create Folder
    public addFolder(folder: FolderCreateModel): Observable<any> {
        return this.client.post(this.url, folder)
    }

    //Function (request) Download Folder Zip
    public downloadFolderZip(folderPath:string,folderName:string):Observable<any>
    {
        //If Folder Path empty
        if(folderPath.length===0){
            return this.client.get(`${this.url}/zip?folderName=${folderName}`,{
                responseType: 'blob'
            });
        }
        //If Folder Path full
        else{
            return this.client.get(`${this.url}/zip?folderPath=${folderPath}&folderName=${folderName}`, {
                responseType: 'blob'
            });
        }
       
    }

    //Function (request) Delete Folder
    public deleteFolder(folder:FolderDeleteModel):Observable<any>{
        return this.client.delete(this.url,{body:folder})
    }

}   