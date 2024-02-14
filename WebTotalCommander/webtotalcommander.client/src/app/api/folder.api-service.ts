import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, catchError } from "rxjs";
import { FolderCreateModel } from "./models/folder/folder.create-model";
import { FolderDeleteModel } from "./models/folder/folder.delete-model";
import { FolderGetAllModel } from "./models/common/folder.getall-model";
import { SubFilter } from "../components/models/sub-filter";

@Injectable({ providedIn: "root" })
export class FolderApiService {
    //Variable HttpClient Inject
    private client: HttpClient = inject(HttpClient);
    //Variable Backend URL
    private urlMain: string = "https://localhost:7251/api/folder"

    //Function (request) GetAll Folders and Files
    public getAllFolder(folderPath: string, skip: number, take: number,
        filters?: { 'Filter.Logic': string; 'Filter.Filters': Array<SubFilter>; },
        sort?: { dir?: string, field: string }): Observable<FolderGetAllModel> {
        
        let url: string = `${this.urlMain}?Offset=${skip}&Limit=${take}`; 

        //If Folder Path
        if(folderPath) {
            url= `${this.urlMain}?Path=${folderPath}&Offset=${skip}&Limit=${take}`;
        }
        //If Filter
        if (filters) {
            url = this.addFiltersToQuery(url, filters);
        }
        //If Sort
        if (sort) {
            url = this.addSort(url, sort);
        }

        return this.client.get<FolderGetAllModel>(url).pipe(
            catchError((error) => {
                throw error;
            })
        );
    }

    //Function (request) Create Folder
    public addFolder(folder: FolderCreateModel): Observable<any> {
        return this.client.post(this.url, folder)
    }

    //Function (request) Download Folder Zip
    public downloadFolderZip(folderPath: string, folderName: string): Observable<any> {
        //If Folder Path empty
        if (folderPath.length === 0) {
            return this.client.get(`${this.url}/zip?folderName=${folderName}`, {
                responseType: 'blob'
            });
        }
        //If Folder Path full
        else {
            return this.client.get(`${this.url}/zip?folderPath=${folderPath}&folderName=${folderName}`, {
                responseType: 'blob'
            });
        }

    }

    //Function (request) Delete Folder
    public deleteFolder(folder: FolderDeleteModel): Observable<any> {
        return this.client.delete(this.url, { body: folder })
    }

    //Function (helper) Add filter query in getAll request url
    private addFiltersToQuery(url: string, params: {'Filter.Logic': string; 'Filter.Filters': Array<SubFilter>;}): string {
        let resultUrl = url;
        if (params['Filter.Filters'].length > 0) {
            resultUrl += `&Filter.Logic=${params['Filter.Logic']}`;
            for (let i = 0; i < params['Filter.Filters'].length; i++) {
                const subFilter = params['Filter.Filters'][i];

                if (subFilter.filters && subFilter.filters.length > 0)
                    for (let j = 0; j < subFilter.filters.length; j++) {
                        const filterDefinition = subFilter.filters[j];
                        resultUrl += `&Filter.Filters[${i}].Filters[${j}].Field=${filterDefinition.field}`;
                        resultUrl += `&Filter.Filters[${i}].Filters[${j}].Operator=${filterDefinition.operator}`;
                        resultUrl += `&Filter.Filters[${i}].Filters[${j}].Value=${filterDefinition.value}`;
                    }
                resultUrl += `&Filter.Filters[${i}].Logic=${subFilter.logic}`;
            }
        }
        return resultUrl;
    }

    //Function (helper) Add sort query in getAll request url
    private addSort(url: string, sort: { dir?: string, field: string }): string {
        const dir = typeof sort.dir === "string" ? sort.dir : "asc";
        const resultUrl = url + `&SortDir=${dir}&SortField=${sort.field}`;
        return resultUrl;
    }

}   