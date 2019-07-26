import axios from "axios";
import qs from "qs";
import cookie from "react-cookies";
import { toastError } from "./toast.helper";

const instance = axios.create({
  timeout: 100000
});

const handleError = error => {
  // eslint - disable - next - line;
  if (error.response) {
    const messages = error.response && error.response.data;
    if (Array.isArray(messages)) {
      // eslint-disable-next-line
      console.log("Error coming from array");
      messages.map(message =>
        toastError(`${message.errorMessage}`)
      );
    } else {
      console.log("Error coming from object");
      toastError(messages.message);
      //toastError(message);
    }
  } else if (error.request) {
    //eslint-disable-next-line
    console.log("error.request", "Network error!");
    toastError(error.request);
  } else {
    //eslint-disable-next-line
    console.log("An unknown error has occurred!");
    toastError("An unknown error has occurred!");
  }
};

export default class RequestHelper {
  static async getHeader(config = {}) {
    return {
      accept: "application/json",
      contentType: "application/json",
      "x-access-token": cookie.load("token"),
      ...config
    };
  }
  static async get(apiUrl, params) {
    const header = await this.getHeader();
    return instance
      .get(apiUrl, {
        headers: header,
        params,
        paramsSerializer: params => {
          return qs.stringify(params, { arrayFormat: "repeat" });
        }
      })
      .then(data => {
        return data.data;
      })
      .catch(e => {
        handleError(e);
        throw e;
      });
  }
  static async post(apiUrl, data, config) {
    return instance({
      method: "post",
      url: apiUrl,
      headers: await this.getHeader(config),
      data: data
    })
      .then(data => {
        return data.data;
      })
      .catch(e => {
        handleError(e);
        throw e;
      });
  }
  static async put(apiUrl, data) {
    return instance({
      method: "put",
      url: apiUrl,
      headers: await this.getHeader(),
      data: data
    })
      .then(data => {
        return data.data;
      })
      .catch(e => {
        handleError(e);
        throw e;
      });
  }
  static async delete(apiUrl) {
    return instance({
      method: "delete",
      url: apiUrl,
      headers: await this.getHeader()
    })
      .then(data => {
        return data.data;
      })
      .catch(e => {
        handleError(e);
        throw e;
      });
  }

  static async postAndDownloadPDF(apiUrl, data) {
    return instance({
      method: "post",
      url: apiUrl,
      headers: {
        accept: "application/pdf",
        contentType: "application/json",
        "Access-Token": cookie.load("token")
      },
      responseType: "blob",
      data: data
    })
      .then(data => {
        return data.data;
      })
      .catch(e => {
        handleError(e);
        throw e;
      });
  }
}
