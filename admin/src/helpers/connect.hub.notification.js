import * as signalR from "@aspnet/signalr";
import CookieHelper from "../helpers/cookie.helper";

export default class HubNotification {
    static connectHubNotification (urlConnect){
        var hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(urlConnect, { accessTokenFactory: () => CookieHelper.getToken() }).build();
        try {
            hubConnection.start().then(() => { console.log("connected !!!") }).catch(err => console.error(err, 'red'));
            return hubConnection;
        } catch (e) {
            console.log(e);
        }
    }
}