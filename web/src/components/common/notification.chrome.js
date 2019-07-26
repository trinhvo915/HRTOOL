import React, { Component } from 'react';
import Notification from "react-web-notification";
class NotificationChrome extends Component {
    render() {
        let { 
            ignore, 
            notSupported,
            onPermissionGranted,
            onPermissionDenied, 
            onShow, 
            onClick, 
            onClose,
            onError,
            title,
            options
        } = this.props
        return (
            <div>
                <Notification
                    ignore={ignore}
                    notSupported={notSupported}
                    onPermissionGranted={onPermissionGranted}
                    onPermissionDenied={onPermissionDenied}
                    onShow={onShow}
                    onClick={onClick}
                    onClose={onClose}
                    onError={onError}
                    timeout={5000}
                    title={title}
                    options={options}
                />
                <audio id='sound' preload='auto'>
                    <source src='./sound.mp3' type='audio/mpeg' />
                    <source src='./sound.ogg' type='audio/ogg' />
                    <embed hidden='true' autostart='false' loop='false' src='./sound.mp3' />
                </audio>
            </div>
        );
    }
}

export default NotificationChrome;