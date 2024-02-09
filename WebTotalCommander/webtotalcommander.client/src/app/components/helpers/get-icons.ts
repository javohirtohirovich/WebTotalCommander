import { Injectable } from "@angular/core";
//Kenod Icons
import {
    arrowRotateCcwIcon, homeIcon, SVGIcon, filePdfIcon, fileExcelIcon,
    fileWordIcon, downloadIcon, trashIcon, pencilIcon, fileImageIcon, fileTxtIcon,
    fileAudioIcon, fileTypescriptIcon, fileVideoIcon, filePptIcon, folderIcon,
    exeIcon, fileProgrammingIcon, xIcon, fileZipIcon,arrowLeftIcon,arrowRightIcon,arrowUpIcon,folderUpIcon
} from "@progress/kendo-svg-icons";

@Injectable({ providedIn: "root" })
export class KendoIcons{
    //Variables SVGIcon
    public homeIcon: SVGIcon = homeIcon;
    public downloadIcon: SVGIcon = downloadIcon;
    public rotateIcon: SVGIcon = arrowRotateCcwIcon;
    public deleteIcon: SVGIcon = trashIcon;
    public editIcon: SVGIcon = pencilIcon;
    public arrowLeft:SVGIcon=arrowLeftIcon;
    public arrowRight:SVGIcon=arrowRightIcon;
    public arrowUp:SVGIcon=arrowUpIcon;
    
     //Dictionary FileIcons
     private fileIcons: { [key: string]: SVGIcon } = {
        'default': xIcon,
        'folder': folderIcon,
        '.pdf': filePdfIcon,
        '.jpg': fileImageIcon,
        '.jpeg': fileImageIcon,
        '.png': fileImageIcon,
        '.gif': fileImageIcon,
        '.xlsx': fileExcelIcon,
        '.xls': fileExcelIcon,
        '.docx': fileWordIcon,
        '.doc': fileWordIcon,
        '.txt': fileTxtIcon,
        '.mp4': fileVideoIcon,
        '.exe': exeIcon,
        '.py': fileProgrammingIcon,
        '.js': fileProgrammingIcon,
        '.mp3': fileAudioIcon,
        '.ts': fileTypescriptIcon,
        '.zip': fileZipIcon,
        '.ppt': filePptIcon,
        '.pptx': filePptIcon,
        '':folderUpIcon
    };
    
    //Get File and Folder extension
    public getIconForExtension(extension: string): SVGIcon {
        return this.fileIcons[extension.toLowerCase()] || fileTypescriptIcon;
    }

}