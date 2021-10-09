using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using RestAPI.Models.DTO;
using RestAPI.Properties;

namespace RestAPI.Infrastructure.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointService(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public async Task<IList<CurvePointDto>> GetCurvePoint()
        {
            var curvePoints = await _curvePointRepository.GetCurvePoint();

            return curvePoints.Adapt<IList<CurvePointDto>>();
        }

        public async Task<CurvePointDto> GetCurvePoint(int id)
        {
            var curvePoint = await _curvePointRepository.GetCurvePoint(id);

            return curvePoint == null
                ? throw new KeyNotFoundException(Resources.CurvePointNotFound)
                : curvePoint.Adapt<CurvePointDto>();
        }

        public async Task UpdateCurvePoint(CurvePointDto curvePointDto)
        {
            var curvePoint = await _curvePointRepository.GetCurvePoint(curvePointDto.Id);
            if (curvePoint == null)
            {
                throw new KeyNotFoundException(Resources.CurvePointNotFound);
            }

            curvePointDto.Adapt(curvePoint);
            await _curvePointRepository.UpdateCurvePoint(curvePoint);
        }

        public async Task SaveCurvePoint(CurvePointDto curvePointDto)
        {
            var curvePoint = curvePointDto.Adapt<CurvePoint>();
            await _curvePointRepository.SaveCurvePoint(curvePoint);
        }

        public async Task DeleteCurvePoint(int id)
        {
            var curvePoint = await _curvePointRepository.GetCurvePoint(id);
            if (curvePoint == null)
            {
                throw new KeyNotFoundException(Resources.CurvePointNotFound);
            }

            await _curvePointRepository.DeleteCurvePoint(id);
        }
    }
}