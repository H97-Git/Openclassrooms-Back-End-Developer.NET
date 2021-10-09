using AutoMapper;
using The_Car_Hub.Data;
using The_Car_Hub.Models;

namespace The_Car_Hub.Mapping
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<InventoryViewModel,Inventory>().ReverseMap();
            CreateMap<InventoryViewModel,Car>().ReverseMap();
            CreateMap<InventoryViewModel,InventoryStatus>().ReverseMap();
        }
    }
}