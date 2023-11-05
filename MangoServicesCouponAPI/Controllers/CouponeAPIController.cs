using AutoMapper;
using MangoServicesCouponAPI.Data;
using MangoServicesCouponAPI.Models;
using MangoServicesCouponAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangoServicesCouponAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponeAPIController : ControllerBase
{
    private readonly AppDbContext _db;
    private ResponseDTO _response;
    private IMapper _mapper;

    public CouponeAPIController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _response = new ResponseDTO();
        _mapper = mapper;
    }

    [HttpGet]
    public ResponseDTO Get()
    {
        try
        {
            IEnumerable<Coupon> objList = _db.Coupons.ToList();
            _response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return _response;
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public ResponseDTO Get(int id)
    {
        try
        {
            var obj = _db.Coupons.First(c =>c.CouponId == id);
            _response.Result = _mapper.Map<CouponDTO>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return _response;
    }
    
    [HttpGet]
    [Route("GetByCode({code}")]
    public ResponseDTO GetByCode(string code)
    {
        try
        {
            var obj = _db.Coupons.FirstOrDefault(c =>c.CouponCode.ToLower() == code.ToLower());
            if (obj == null)
            {
                _response.IsSuccess = false;
            }
            _response.Result = _mapper.Map<CouponDTO>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return _response;
    }
    
    [HttpPost]
    public ResponseDTO Post([FromBody] CouponDTO couponDTO)
    {
        try
        {
            Coupon obj = _mapper.Map<Coupon>(couponDTO);
            _db.Coupons.Add(obj);
            _db.SaveChanges();
            
            _response.Result = _mapper.Map<CouponDTO>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return _response;
    }
    
    [HttpPut]
    public ResponseDTO Put([FromBody] CouponDTO couponDTO)
    {
        try
        {
            Coupon obj = _mapper.Map<Coupon>(couponDTO);
            _db.Coupons.Update(obj);
            _db.SaveChanges();
            
            _response.Result = _mapper.Map<CouponDTO>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return _response;
    }
    
    [HttpDelete]
    public ResponseDTO Delete(int id)
    {
        try
        {
            Coupon obj = _db.Coupons.First(c => c.CouponId == id);
            _db.Coupons.Remove(obj);
            _db.SaveChanges(); 
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return _response;
    }
}