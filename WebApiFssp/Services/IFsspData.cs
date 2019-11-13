using WebApiFssp.Models;

namespace WebApiFssp.Services
{
    public interface IFsspData
    {
        string Data { get; }
        void GetDataFromFssp(FsspPerson person);
    }
}