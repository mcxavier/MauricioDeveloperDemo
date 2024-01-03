using System;
using System.Threading.Tasks;
using LinxIO.Dtos;

namespace LinxIO.Interfaces
{
    public interface IDisposable
    {
        public Task<LinxIOClientConfig> GetConfig(Guid companyId);
    }
}