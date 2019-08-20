using AutoMapper;
using PriorMoney.DesktopApp.Model;
using PriorMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorMoney.DesktopApp.Startup
{
    public static class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfgExp =>
            {
                cfgExp.CreateMap<CardOperation, CardOperationModel>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
