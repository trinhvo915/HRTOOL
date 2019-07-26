import { withGoogleMap, GoogleMap, Marker } from "react-google-maps";
import React, { Component } from "react";
class PlacePicker extends Component {
  constructor(props) {
    super(props);
    this.state = {
      defaultCenter:
        props.defaultCenter &&
        props.defaultCenter.lat &&
        props.defaultCenter.lng
          ? props.defaultCenter
          : {
              lat: 16.0369351,
              lng: 108.1993426
            }
    };
  }
  componentDidMount = async () => {
    const { props } = this;
    if (
      !(
        props.defaultCenter &&
        props.defaultCenter.lat &&
        props.defaultCenter.lng
      )
    )
      navigator.geolocation.getCurrentPosition(
        position => {
          const { latitude, longitude } = position.coords;
          this.map.panTo({ lat: latitude, lng: longitude });
        },
        () => {}
      );
  };

  render = () => {
    const { onClick, position, onMapMounted } = this.props;
    const { defaultCenter } = this.state;
    return (
      <GoogleMap
        onClick={onClick}
        ref={ref => {
          this.map = ref;
          onMapMounted(ref);
        }}
        defaultZoom={16}
        defaultCenter={
          (defaultCenter.lat && defaultCenter.lng && defaultCenter) || {
            lat: 13.727286,
            lng: 100.568995
          }
        }
      >
        {position && position.lat && position.lng && (
          <Marker position={position} />
        )}
      </GoogleMap>
    );
  };
}

export default withGoogleMap(PlacePicker);
