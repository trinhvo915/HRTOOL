import requestHelper from './request.helper'
import { URL_ENPOINTS } from '../constant/urls'

export const uploadFile = (folder, file) => {
  let formData = new FormData()
  formData.append('file', file)
  formData.append('fileName', Date.now())
  formData.append('folder', folder)

  return requestHelper
    .post(URL_ENPOINTS.MEDIA, formData)
    .then(res => {
      return res
    })
    .catch(err => {})
}
