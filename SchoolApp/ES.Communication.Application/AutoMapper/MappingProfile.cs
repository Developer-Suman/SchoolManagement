using AutoMapper;
using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.Communication.Command.UnPublishNotice;
using ES.Communication.Application.Communication.Queries.FilterNotice;
using ES.Communication.Application.Communication.Queries.NoticeById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Communication.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Publish and UnPublish Notice
            CreateMap<PublishNoticeResponse, UnPublishNoticeCommand>().ReverseMap();
            CreateMap<UnPublishNoticeResponse, UnPublishNoticeCommand>().ReverseMap();
            #endregion


            #region Attendance
            CreateMap<AddNoticeResponse, Notice>().ReverseMap();
            CreateMap<NoticeByIdResponse, Notice>().ReverseMap();
            CreateMap<FilterNoticeResponse, Notice>().ReverseMap();
            CreateMap<PagedResult<Notice>, PagedResult<FilterNoticeResponse>>().ReverseMap();
            #endregion
        }
    }
}
