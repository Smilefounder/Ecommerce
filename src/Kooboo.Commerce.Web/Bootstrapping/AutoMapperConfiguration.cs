using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            var profiles = new List<Profile>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsAbstract && typeof(Profile).IsAssignableFrom(type))
                {
                    profiles.Add((Profile)Activator.CreateInstance(type));
                }
            }

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}