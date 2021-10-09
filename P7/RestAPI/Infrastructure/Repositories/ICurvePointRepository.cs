using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public interface ICurvePointRepository
    {
        Task<IList<CurvePoint>> GetCurvePoint();

        Task<CurvePoint> GetCurvePoint(int id);

        Task UpdateCurvePoint(CurvePoint curvePoint);

        Task SaveCurvePoint(CurvePoint curvePoint);

        Task DeleteCurvePoint(int id);
    }
}