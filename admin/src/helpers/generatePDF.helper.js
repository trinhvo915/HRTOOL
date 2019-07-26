import RequestHelper from './request.helper'
import {appConfig} from "../config/app.config"

export const generatePDF = (query) => {
  console.log(query)
  return RequestHelper
    .get(appConfig.apiUrl + "questions/generate", query)
    .then(res => {
        console.log(res);
      return res
    })
    .catch(err => {})
}
