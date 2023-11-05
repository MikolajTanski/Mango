using MangoServicesCouponAPI.Models;
using MangoServicesCouponAPI.Models.DTO;

namespace MangoServicesCouponAPI;

using AutoMapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CouponDTO, Coupon>();
            config.CreateMap<Coupon, CouponDTO>();
        });

        return mapperConfig;
    }
}
