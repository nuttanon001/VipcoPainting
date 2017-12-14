using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoPainting.Helpers
{
    public class ConverterTableToVM
    {
        private IMapper mapper;

        public ConverterTableToVM(IMapper map)
        {
            this.mapper = map;
        }

        public List<MapType> ConverterTableToViewModel<MapType, TableType>(ICollection<TableType> tables)
        {
            var listData = new List<MapType>();
            foreach (var item in tables)
                listData.Add(this.mapper.Map<TableType, MapType>(item));
            return listData;
        }
    }
}
