using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.DTO;

namespace RestAPI.Infrastructure.Services
{
    public interface ICurvePointService
    {
        Task<IList<CurvePointDto>> GetCurvePoint();

        Task<CurvePointDto> GetCurvePoint(int id);

        Task UpdateCurvePoint(CurvePointDto curvePointDto);

        Task SaveCurvePoint(CurvePointDto curvePointDto);

        Task DeleteCurvePoint(int id);
    }
}