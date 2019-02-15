using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json.Linq;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ViewModels;

namespace TopTenBites.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuItem, MenuItemViewModel>()
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuItemId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count(x => x.IsLike)))
                .ForMember(dest => dest.DislikeCount, opt => opt.MapFrom(src => src.Likes.Count(x => !x.IsLike)))
                .ForMember(dest => dest.YelpBusinessId, opt =>
                    opt.ResolveUsing((src, dest, destMember, context) => context.Options.Items["YelpBusinessId"])
                 )
                .ForMember(dest => dest.CurrentVisitorHasLiked, opt =>
                    opt.ResolveUsing((src, dest, destMember, context) => context.Options.Items["CurrentVisitorHasLiked"])
                 )
                .ForMember(dest => dest.CurrentVisitorHasDisliked, opt =>
                    opt.ResolveUsing((src, dest, destMember, context) => context.Options.Items["CurrentVisitorHasDisliked"])
                 );

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CommentId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Sentiment, opt => opt.MapFrom(src => src.Sentiment))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<Image, ImageViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<Business, BusinessApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.YelpBusinessId, opt => opt.MapFrom(src => src.YelpBusinessId))
                .ForMember(dest => dest.YelpBusinessAlias, opt => opt.MapFrom(src => src.YelpBusinessAlias))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ReverseMap()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<MenuItem, MenuItemApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuItemId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ReverseMap()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<Comment, CommentApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CommentId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Sentiment, opt => opt.MapFrom(src => src.Sentiment))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.MenuItemId, opt => opt.MapFrom(src => src.MenuItemId))
                .ReverseMap()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<Like, LikeApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LikeId))
                .ForMember(dest => dest.IsLike, opt => opt.MapFrom(src => src.IsLike))
                .ForMember(dest => dest.UserFingerPrintHash, opt => opt.MapFrom(src => src.UserFingerPrintHash))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.MenuItemId, opt => opt.MapFrom(src => src.MenuItemId))
                .ReverseMap()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<Image, ImageApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.MenuItemId, opt => opt.MapFrom(src => src.MenuItemId))
                .ReverseMap()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        }
    }
}