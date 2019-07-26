import React, { Component } from "react";
import Datetime from "react-datetime";
import { FormGroup, Label } from "reactstrap";
import PropTypes from "prop-types";

class DatetimeSelect extends Component {

    render() {
        const { title, name, defaultValue, value, dateFormat, isValidDate, timeFormat, onChange, utc } = this.props;
        return (
            <FormGroup>
                <Label className="label-input" for={name}>
                    <strong>{title}</strong>
                </Label>
                <Datetime
                    defaultValue={defaultValue}
                    value={value}
                    dateFormat={dateFormat}
                    timeFormat={timeFormat}
                    isValidDate={isValidDate}
                    onChange={onChange}
                    utc={utc}
                    closeOnSelect={true}
                />
            </FormGroup>
        )
    }
}

DatetimeSelect.propTypes = {
    title: PropTypes.string,
    name: PropTypes.string,
    dateFormat: PropTypes.string || PropTypes.bool,
    timeFormat: PropTypes.string || PropTypes.bool
}

export default DatetimeSelect;