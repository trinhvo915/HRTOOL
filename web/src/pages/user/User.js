import React, { Component, Suspense } from "react";
import { Redirect, Route, Switch } from "react-router-dom";
import { Container } from "reactstrap";
import Notification from "react-web-notification";
import cookie from "react-cookies";

import {
  AppAside,
  AppBreadcrumb,
  AppFooter,
  AppHeader,
  AppSidebar,
  AppSidebarFooter,
  AppSidebarForm,
  AppSidebarHeader,
  AppSidebarMinimizer,
  AppSidebarNav
} from "@coreui/react";
// sidebar nav config
import navigation from "../../_nav";
// routes config
import routes from "../../routes";
import { getProfile } from "../../actions/profile.action";
import { connect } from "react-redux";
import { connectHubNotificationCalendar } from "../../actions/hub.notification.calendar.action";
import IconNotification from "../../components/common/image/Notifications_button_24.png";
import SoundNotification from "../../components/common/sound/sound.mp3";
import { appConfig } from "../../config/app.config";
import CookieHelper from "../../helpers/cookie.helper";

const DefaultAside = React.lazy(() => import("./DefaultLayout/DefaultAside"));
const DefaultFooter = React.lazy(() => import("./DefaultLayout/DefaultFooter"));
const DefaultHeader = React.lazy(() => import("./DefaultLayout/DefaultHeader"));

class DefaultLayout extends Component {
  constructor(props) {
    super(props);
    this.state = {
      ignore: true,
      title: '',
      options : null,
    };
  }

  handlePermissionGranted(){
    this.setState({
      ignore: false
    });
  }

  handlePermissionDenied(){
    this.setState({
      ignore: true
    });
  }
  
  handleNotSupported(){
    this.setState({
      ignore: true
    });
  }

  handleNotificationOnClick(e, tag){
    window.location.href = "http://localhost:3001/calendars";
  }

  handleNotificationOnError(e, tag){
    console.log(e, 'Notification error tag:' + tag);
  }

  handleNotificationOnClose(e, tag){
    console.log(e, 'Notification closed tag:' + tag);
  }

  handleButtonClick(nameSender, IdUserAdd, IdUserDelete, secalendarDescription, type) {

    if(this.state.ignore) {
      return;
    }

    const now = Date.now();
    let type_event = '';
    let typeEventUser = '';
    if(type === appConfig.calendar_add){
      type_event = 'add calendar';
    }else if(type === appConfig.calendar_update){
      let idCurent = CookieHelper.getUser().JwtPayload.UserId;
      type_event = 'update calendar';
      if(IdUserAdd.indexOf(idCurent)!==-1){
        typeEventUser = 'You have receive a calendar'
      }else if(IdUserDelete.indexOf(idCurent)!==-1){
        typeEventUser = 'You were removed a calendar'
      }
    }
    
    if(type === appConfig.job_update){
      let idCurent = CookieHelper.getUser().JwtPayload.UserId;
      type_event = 'update Job';
      if(IdUserAdd.indexOf(idCurent)!==-1){
        typeEventUser = 'You have receive a job'
      }else if(IdUserDelete.indexOf(idCurent)!==-1){
        typeEventUser = 'You were removed a job'
      }
    }
    const title = 'Have A Notification Type Of '+ type_event;
    const body = nameSender + " was sent a "+ type_event +" with description " + secalendarDescription+ (typeEventUser!== '' ? ' and '+ typeEventUser : '');
    const tag = now;
    const icon = IconNotification;
    const options = {
      tag: tag,
      body: body,
      icon: icon,
      lang: 'en',
      dir: 'ltr',
      sound: SoundNotification,  
    }
    
    this.setState({
      title: title,
      options: options
    });
  }

  loading = () => (
    <div className="animated fadeIn pt-1 text-center">Đang tải...</div>
  );

  signOut(e) {
    e.preventDefault();
    cookie.remove("token", { path: "/" });
    this.props.history.push("/login");
  }

  componentDidMount = async ()  => {
    this.props.getProfile();
    await  this.props.connectHubNotificationCalendar();
    this.props.hubConnectionCalendar && this.props.hubConnectionCalendar.on('Receive', (nameSender, idUsers, secalendarDescription, type) => {
      if(type !== appConfig.comment_add ||type !== appConfig.comment_add  ){
        this.handleButtonClick(nameSender, idUsers, null , secalendarDescription, type);
      }
    });

    this.props.hubConnectionCalendar && this.props.hubConnectionCalendar.on('ReceiveUpdate', (nameSender, IdUserAdd, IdUserDelete, secalendarDescription, type) => {
      this.handleButtonClick(nameSender, IdUserAdd, IdUserDelete, secalendarDescription, type);
    });
  };

  getEmployeeSidebar = () => {
    const { profile } = this.props.profile;
    const permissions = profile.permissions || [];
    const menus = profile.admin
      ? navigation.items
      : navigation.items.filter(
        item =>
          !item.permissions ||
          item.permissions.find(el => permissions.indexOf(el) !== -1)
      );
    return menus;
  };

  render() {
    const items = this.getEmployeeSidebar();
    return (
      <>
        <div className="app">
          <AppHeader fixed>
            <Suspense fallback={this.loading()}>
              <DefaultHeader onLogout={e => this.signOut(e)} />
            </Suspense>
          </AppHeader>

          <div className="app-body">
            <AppSidebar fixed display="lg">
              <AppSidebarHeader />
              <AppSidebarForm />
              <Suspense>
                <AppSidebarNav navConfig={{ items }} {...this.props} />
              </Suspense>
              <AppSidebarFooter />
              <AppSidebarMinimizer />
            </AppSidebar>

            <main className="main">
              <AppBreadcrumb appRoutes={routes} />
              <Container fluid>
                <Suspense fallback={this.loading()}>
                  <Switch>
                    {routes.map((route, idx) => {
                      return route.component ? (
                        <Route
                          key={idx}
                          path={route.path}
                          exact={route.exact}
                          name={route.name}
                          render={props => <route.component {...props} />}
                        />
                      ) : null;
                    })}
                    <Redirect from="/" to="/dashboard" />
                  </Switch>
                </Suspense>
              </Container>
            </main>

            <AppAside fixed>
              <Suspense fallback={this.loading()}>
                <DefaultAside />
              </Suspense>
            </AppAside>
          </div>

          <AppFooter>
            <Suspense fallback={this.loading()}>
              <DefaultFooter />
            </Suspense>
          </AppFooter>

          <div>
            <Notification
              ignore={this.state.ignore && this.state.title !== ''}
              notSupported={this.handleNotSupported.bind(this)}
              onPermissionGranted={this.handlePermissionGranted.bind(this)}
              onPermissionDenied={this.handlePermissionDenied.bind(this)}
              onClick={this.handleNotificationOnClick.bind(this)}
              onClose={this.handleNotificationOnClose.bind(this)}
              onError={this.handleNotificationOnError.bind(this)}
              title={this.state.title}
              options={this.state.options}
            />
          </div>
        </div>
      </>
    );
  }
}

export default connect(
  state => ({
    profile: state.profile,
    hubConnectionCalendar: state.hubConnectionCalendar.hubConnectionCalendar,
  }),
  {
    getProfile,
    connectHubNotificationCalendar
  }
)(DefaultLayout);
