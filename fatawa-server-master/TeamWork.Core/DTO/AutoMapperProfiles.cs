using AutoMapper;
using System.Linq;
using TeamWork.Core.DTO.Chat;
using TeamWork.Core.Entity;
using TeamWork.Core.Entity.Consultation;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.Library;
using TeamWork.Core.Entity.Membership;
using TeamWork.Core.Entity.Notification;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Entity.Selection;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Entity.Video;
using TeamWork.Core.Models.Chat;
using TeamWork.Core.Models.Consultation;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Models.Library;
using TeamWork.Core.Models.Membership;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Models.Project;
using TeamWork.Core.Models.Security;
using TeamWork.Core.Models.Selection;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Models.Video;
using TeamWork.DTO.Security;

namespace TeamWork.DTO
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //just for test
            CreateMap<Job, JobModel>().ReverseMap();
            CreateMap<User, UserList>().ReverseMap();
            CreateMap<User, UserRegister>().ReverseMap();
            CreateMap<User, UserLogin>().ReverseMap();
            CreateMap<GeneralSetting, GeneralSettingModel>().ReverseMap();
            CreateMap<Fatawa, FatawaModel>()
                .ForMember(vm => vm.Name, m => m.MapFrom(u => u.FatawaTranslations.FirstOrDefault().Name))
                .ForMember(vm => vm.TranslatorName, m => m.MapFrom(u => u.FatawaTranslations.FirstOrDefault().TranslatorName))
                .ForMember(vm => vm.Description, m => m.MapFrom(u => u.FatawaTranslations.FirstOrDefault().Description))
                .ReverseMap();

            CreateMap<FatawaTranslation, FatawaTranslationModel>().ReverseMap();
            CreateMap<FatawaDefaultSetting, FatawaDefaultSettingModel>()
                .ForMember(vm => vm.Name, m => m.MapFrom(u => u.FatawaDefaultSettingTranslations.FirstOrDefault().Name))
                .ForMember(vm => vm.TranslatorName, m => m.MapFrom(u => u.FatawaDefaultSettingTranslations.FirstOrDefault().TranslatorName))
                .ForMember(vm => vm.Description, m => m.MapFrom(u => u.FatawaDefaultSettingTranslations.FirstOrDefault().Description))
                .ReverseMap();

            CreateMap<FatawaDefaultSettingTranslation, FatawaDefaultSettingTranslationModel>().ReverseMap();

            CreateMap<FatawaDepartment, FatawaDepartmentModel>().ForMember(dest => dest.NodeName, opt => opt.MapFrom(src => src.FatawaDepartmentTranslations.Select(x => x.NodeName).FirstOrDefault()))
                                                                .ForMember(dest => dest.FatawaDepartmentTranslateId, opt => opt.MapFrom(src => src.FatawaDepartmentTranslations.Select(x => x.FatawaDepartmentId).FirstOrDefault()))
                                                                .ForMember(dest => dest.FatawaDepartmentTranslations, opt => opt.Ignore())
                                                                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.FatawaDepartmentTranslations.Select(x => x.LanguageId).FirstOrDefault())).ReverseMap();


            CreateMap<UserTree, UsertreeModel>().ReverseMap();



            CreateMap<FatawaDepartmentTranslation, FatawaDepartmentTranslationModel>().ReverseMap();
            CreateMap<FatawaType, FatawaTypeModel>().ReverseMap();
            CreateMap<Question, QuestionModel>().ReverseMap();
            CreateMap<QuestionDiscussion, QuestionDiscussionModel>().ReverseMap();

            //Member module
            CreateMap<Member, MemberModel>().ReverseMap();
            CreateMap<MembershipType, MembershipTypeModel>().ReverseMap();

            //Selection module
            CreateMap<SelectionType, SelectionTypeModel>().ReverseMap();
            CreateMap<SelectionItem, SelectionItemModel>().ReverseMap();

            //Library module
            CreateMap<LibraryItem, LibraryItemModel>().ReverseMap();
            CreateMap<LibraryType, LibraryTypeModel>().ReverseMap();

            //Consultation module
            CreateMap<Consultation, ConsultationModel>().ReverseMap();
            CreateMap<ConsultationType, ConsultationTypeModel>().ReverseMap();
            CreateMap<ConsultationDiscussion, ConsultationDiscussionModel>().ReverseMap();

            //Video module
            CreateMap<YoutubeVideo, YoutubeVideoModel>().ReverseMap();
            CreateMap<VideoType, VideoTypeModel>().ReverseMap();
            CreateMap<Playlist, PlaylistModel>().ReverseMap();

            //System Definition module
            CreateMap<Country, CountryModel>().ReverseMap();
            CreateMap<Language, LanguageModel>().ReverseMap();


            //Notification
            CreateMap<NotificationItem, NotificationItemModel>()
                .ForMember(vm => vm.MessageText, m => m.MapFrom(u => u.NotificationItemTranslations.FirstOrDefault().MessageText))
                .ReverseMap();

            CreateMap<NotificationType, NotificationTypeModel>().ReverseMap();

            //DTOs
            CreateMap<ChatMessageModel, ChatMessageDTO>().ReverseMap();
        }
    }
}
