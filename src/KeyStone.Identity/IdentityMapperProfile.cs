﻿using AutoMapper;
using KeyStone.Concerns.Identity;
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
