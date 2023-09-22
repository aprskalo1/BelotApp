using BelotApp.Models;
using BelotApp.ViewModels;

namespace BelotApp.Mapping
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameVM, Game>();
            CreateMap<Game, GameVM>();

            CreateMap<GameResultsVM, GameResult>();
            CreateMap<GameResult, GameResultsVM>();

            CreateMap<UsersVM, AspNetUsers>();
            CreateMap<AspNetUsers, UsersVM>();
        }
    }
}
