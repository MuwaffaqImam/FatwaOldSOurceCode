namespace TeamWork.Core.Models.Fatawa
{
    public class QuestionModel: BaseModel
    {
        public string FatawaQuestion { get; set; }
        public int StatusId { get; set; }
        public int? StatusUserId { get; set; }
        public string askedFullName { get; set; }
        public int TransferUserId { get; set; }
        
    }
}
