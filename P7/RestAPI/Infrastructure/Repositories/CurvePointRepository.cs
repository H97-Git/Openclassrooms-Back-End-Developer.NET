using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {
        private static ApplicationDbContext _applicationDbContext;

        public CurvePointRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IList<CurvePoint>> GetCurvePoint()
        {
            var curvePoint = await _applicationDbContext.CurvePoints.ToListAsync();
            return curvePoint;
        }

        public async Task<CurvePoint> GetCurvePoint(int id)
        {
            var curvePoint = await _applicationDbContext.CurvePoints.FindAsync(id);
            return curvePoint;
        }

        public async Task UpdateCurvePoint([Required] CurvePoint curvePoint)
        {
            _applicationDbContext.CurvePoints.UpdateRange(curvePoint);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task SaveCurvePoint([Required] CurvePoint curvePoint)
        {
            await _applicationDbContext.CurvePoints.AddAsync(curvePoint);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteCurvePoint(int id)
        {
            var curvePoint = await _applicationDbContext.CurvePoints.FindAsync(id);
            _applicationDbContext.CurvePoints.Remove(curvePoint);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}