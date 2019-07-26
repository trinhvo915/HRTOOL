import cookie from 'react-cookies';

export default class SessionHelper {
  
  static getToken() {
    return cookie.load("token");
  }

  static getUser() {
    const _user = cookie.load("token").split('.')[1];
    try {
      return JSON.parse(window.atob(_user));
    } catch (e) {
      return null;
    }
  }
}