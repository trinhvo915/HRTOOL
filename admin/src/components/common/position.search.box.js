import React from "react";
import PlacesAutocomplete, {
  geocodeByAddress,
  getLatLng
} from "react-places-autocomplete";
import { Input } from "reactstrap";

export default class PositionSearchBox extends React.Component {
  constructor(props) {
    super(props);
    this.state = { address: "" };
  }

  handleChange = address => {
    this.setState({ address });
  };

  handleSelect = address => {
    const { onSelectPosition } = this.props;
    geocodeByAddress(address)
      .then(results => getLatLng(results[0]))
      .then(latLng => onSelectPosition(latLng))
      .catch(error => {});
  };

  render() {
    return (
      <PlacesAutocomplete
        value={this.state.address}
        onChange={this.handleChange}
        onSelect={this.handleSelect}
      >
        {({ getInputProps, suggestions, getSuggestionItemProps, loading }) => (
          <div>
            <Input
              {...getInputProps({
                placeholder: "Tìm kiếm địa điểm",
                className: "custom-form-coltrol"
              })}
            />
            <div className="autocomplete-dropdown-container">
              {suggestions.slice(0, 3).map(suggestion => {
                return (
                  <div
                    {...getSuggestionItemProps(suggestion, {
                      className: "suggestion-place"
                    })}
                  >
                    <span>{suggestion.description}</span>
                  </div>
                );
              })}
            </div>
          </div>
        )}
      </PlacesAutocomplete>
    );
  }
}
