using System;
using System.Threading.Tasks;

namespace Nop.Services.BulkEmail
{
    /// <summary>
    /// Tangent material service interface
    /// </summary>
    public interface IBulkEmailService
    {
        Task<string> RegisterClickAndGetRedirectUrlAsync(Guid redirectGuid);
    }
}