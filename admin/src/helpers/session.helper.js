import jQuery from 'jquery';

const TOKEN = "token";
const USER = "user"

export default class SessionHelper {
  static setToken(_token) {
    localStorage.setItem(TOKEN, _token);
  }
  static getToken() {
    return localStorage.getItem(TOKEN);
  }
  static setUser(_user) {
    if (_user === null || jQuery.isEmptyObject(_user)) {
      localStorage.removeItem(USER);
      return;
    }
    localStorage.setItem(USER, JSON.stringify(_user));
  }
  static getUser() {
    const _user = localStorage.getItem(USER);
    try {
      return JSON.parse(_user);
    } catch (e) {
      return null;
    }
  }

  static clear() {
    localStorage.clear();
  }
}