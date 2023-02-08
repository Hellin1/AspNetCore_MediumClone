using AutoMapper;
using MediumClone.Business.Mappings.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Helpers
{
    public static class ProfileHelper
    {
        public static List<Profile> GetProfiles()
        {
            return new List<Profile>
            {
                new AppRoleProfile(),
                new BlogProfile(),
                new CategoryProfile(),
                new CommentProfile()
            };
        }
    }
}
