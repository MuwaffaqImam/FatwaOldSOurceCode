namespace TeamWork.Core.DTO
{
    public class SystemEnum
    {
        public enum FatawaType
        {
            FatawaArchived = 1,
            FatawaLive = 2,
            FatawaImport = 3,

        }

        public enum FatawaStatusId
        {
            WaitForApprove = 1,
            Approved = 2,
            NotApproved = 3,
        }

        public enum FatawaMathhabId
        {
            Hanafi = 1,
            Maliki = 2,
            Shafii = 3,
            Hanbali = 4,
            NotSelected = 5
        }

        public enum NotificationType
        {
            Fatawa = 10,
            Question = 20,
            Chatting = 30,
        }

        public enum RolesType
        {
            SuperAdmin = 1,
            Admin = 2,
            AdminGroup = 3,
            Student = 4,
            Guest = 5,
            UsersAdmin = 6,
            StudentAdmin = 7,
        }

        public enum QuestionStatus
        {
            Available = 1,
            InProgress = 2,
            Rejected = 3,
            Duplicated = 4,
            Closed = 5,
        }
    }
}
