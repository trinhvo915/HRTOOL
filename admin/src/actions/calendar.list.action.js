export const GET_CALENDAR_LIST = "[CALENDAR_LIST] GET_CALENDAR_LIST";
export const GET_CALENDAR_LIST_SUCCESS =
  "[CALENDAR_LIST] GET_CALENDAR_LIST_SUCCESS";
export const GET_CALENDAR_LIST_FAILED =
  "[CALENDAR_LIST] GET_CALENDAR_LIST_FAILED";

export const getCalendarList = (params) =>{
    return {
        type : GET_CALENDAR_LIST,
        payload : {
            params
        }
    }
}

export const getCalendarListSuccess = params =>{
    return {
        type : GET_CALENDAR_LIST_SUCCESS,
        payload : params
    }
}

export const getCalendarListFailed = () =>{
    return {
        type : GET_CALENDAR_LIST_FAILED
    }
}
