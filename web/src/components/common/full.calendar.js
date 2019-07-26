import React, { Component } from 'react'
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";

export default class FullCalendarCustom extends Component {
  constructor(props) {
    super(props)
    this.state = {
      events: []
    }
  }

  clickFunc = (type) => {
    switch (type) {
      case "next":
        {
          this.calendarRef.calendar.next();
          break;
        }
      case "prev":
        {
          this.calendarRef.calendar.prev();
          break;
        }
      default:
        {
          this.calendarRef.calendar.today();
        }
    }
    this.props.changeDate(this.calendarRef.calendar.state.dateProfile.currentRange);
  }

  componentDidMount = () => {
    this.props.changeDate(this.calendarRef.calendar.state.dateProfile.currentRange);
  }

  getEvents = (calendars) => {
    const events = calendars.map(element => {
      return {
        title: element.calendarType.name,
        start: new Date(element.dateStart),
        end: new Date(element.dateEnd),
        ...element,
        color: '#c2dff5',
        textColor: '#004377'
      }
    });
    return events;
  }

  render() {
    const { height, handleClick, eventClick, calendars } = this.props;
    const events = this.getEvents(calendars);
    return (
      <FullCalendar
        ref={c => { this.calendarRef = c }}
        height={height}
        defaultView="dayGridMonth"
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
        events={events}
        customButtons={{
          nextButton: {
            icon: 'fc-icon fc-icon-chevron-right',
            click: () => this.clickFunc("next")
          },
          prevButton: {
            icon: 'fc-icon fc-icon-chevron-left',
            click: () => this.clickFunc("prev")
          },
          todayButton: {
            text: 'Today',
            click: () => this.clickFunc("today")
          },
        }}
        header={{
          left: "prevButton,nextButton todayButton",
          center: "title",
          right: "dayGridMonth,timeGridWeek"
        }}
        eventLimit={true}
        selectable={true}
        dateClick={(event) => handleClick(event)}
        eventClick={(event) => eventClick(event.event._def)}
      />
    )
  }
}