using AutoMapper;
using KeyStone.Data.Models.Identity;
using KeyStone.Shared.Models.Identity;

namespace KeyStone.Identity
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            CreateMap<Role, RoleInfo>()
                .ReverseMap();
        }
    }
}
