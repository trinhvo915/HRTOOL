import requestHelper from './request.helper'
import { URL_ENPOINTS } from '../constant/urls';
import { appConfig } from "../config/app.config"

export const uploadFiles = (folder, files) => {
    let formData = new FormData()
    for (var x = 0; x < files.length; x++) {
        formData.append('files', files[x]);
    }
    formData.append('fileName', Date.now())
    formData.append('folder', folder)

    return requestHelper
        .post(appConfig.apiUrl + URL_ENPOINTS.MEDIA + URL_ENPOINTS.UPLOAD_FILES, formData)
        .then(res => {
            return res
        })
        .catch(err => { })
}
