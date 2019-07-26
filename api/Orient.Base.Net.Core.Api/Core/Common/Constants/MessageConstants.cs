namespace Orient.Base.Net.Core.Api.Core.Common.Constants
{
    public static class MessageConstants
    {
        public const string INVALID_ACCESS_TOKEN = "Access token wrong. Please check again!";

        public const string CREATED_SUCCESSFULLY = "The record has been created successfully";

        public const string UPDATED_SUCCESSFULLY = "The record has been updated successfully";

        public const string DELETED_SUCCESSFULLY = "The record has been deleted successfully";

        public const string NOT_FOUND = "The record is not found. Please try another!";

        public const string EXISTED_CREATED = "The record already created into the system. Please help to check again!";

        public const string GENDER_INVALIDATE = "Gender not in the Enum";

        public const string EXISTED_EMAIL = "The email already created into the system. Please help to check again";

        public const string INVALID_DATE_START = "The date start invalid. Please check again!";

        public const string INVALID_DATE_END = "The date end invalid. Please check again!";

        public const string INVALID_EMAIL = "The email is invalid. Please help to check again";

        public const string INVALID_FACEBOOK = "The facebook is invalid. Please help to check again.";

        public const string UNEXPECTED_ERROR = "Unexpeected error, Please try again";

        public const string INVALID_TWITTER = "The Twitter is invalid. Please help to check again.";

        public const string INVALID_LINKEDIN = "The LinkedIn is invalid. Please help to check again.";
    }

    public static class UserMessagesConstants
    {
        public const string NOT_FOUND = "The user is not found. Please try another!";

        public const string EMPTY_USER_LIST = "The user list must have at least one record. Please help to check again!";

        public const string NOT_FOUND_USER_IN_LIST = "The user list has not found record. Please try another!";

        public const string COLOR_EXISTED = "This color is already in use";
    }

    public static class CandidateMessagesConstants
    {
        public const string NOT_FOUND = "The candidate is not found. Please try another!";
    }

    public static class CalendarTypeMessagesConstants
    {
        public const string NOT_FOUND = "The calendar type is not found. Please help to check again!";
    }

    public static class CalendarMessagesConstants
    {
        public const string DATE_START_INVALIDATE = "The date start must be greater than today. Please help to check again!";

        public const string DATE_END_INVALIDATE = "The date end must be greater than the date start. Please help to check again!";
    }

    public static class InterviewerMessagesConstants
    {
        public const string EMPTY_INTERVIEWER_LIST = "The interviewer list must have at least one record. Please help to check again!";

        public const string NOT_FOUND_INTERVIEWER_IN_LIST = "The interviewer list has not found record. Please try another!";
    }

    public static class HRMessagesConstants
    {
        public const string EMPTY_HR_LIST = "The HR list must have at least one record. Please help to check again!";

        public const string NOT_FOUND_HR_IN_LIST = "The HR list has not found record. Please try another!";
    }

    public static class InterviewMessagesConstants
    {
        public const string INVALID_INTERVIEW_STATUS = "Invalid status of Interview!";

        public const string INTERVIEW_NOT_PENDING = "The interview status is not pending! Cannot update the interview! ";

        public const string EXISTED_CANDIDATE = "The candidate is already in interview. Please help to check again";
    }

    public static class CommentMessageConstants
    {
        public const string INVALID_CONTENT = "Invalid content";
    }

    public static class AnswerMessagesConstants
    {
        public const string INVALID_CONTENT = "Invalid content";

        public const string NOT_FOUND = "The Answer is not found. Please try another!";

        public const string EXISTED_ANSWER= "This answer is already existed";
    }

    public static class QuestionMessagesConstants
    {
        public const string INVALID_CONTENT = "Invalid content";

        public const string INVALID_QUESTION_STATUS = "Invalid status of Question!";

        public const string NOT_FOUND = "The Question is not found. Please try another!";

        public const string EXISTED_QUESTION_CONTENT = "This content is already created into the system. Please help to check again";
    }

    public static class EmailTemplateMessageConstants
    {
        public static string INVALID_EMAILTEMPLATE_TYPE = "Invalid type of Email Template";
    }
}
