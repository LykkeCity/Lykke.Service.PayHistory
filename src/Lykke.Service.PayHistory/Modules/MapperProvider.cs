using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration;
using Lykke.Service.PayHistory.Core.Domain;
using Lykke.Service.PayHistory.Models;

namespace Lykke.Service.PayHistory.Modules
{
    public class MapperProvider
    {
        public IMapper GetMapper()
        {
            var mce = new MapperConfigurationExpression();

            CreateHistoryOperationMaps(mce);

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            return new Mapper(mc);
        }

        private void CreateHistoryOperationMaps(MapperConfigurationExpression mce)
        {
            mce.CreateMap<IHistoryOperationView, HistoryOperationViewModel>();
            mce.CreateMap<IHistoryOperation, HistoryOperationModel>();
        }
    }
}
