using WebApiFssp.Models;
using WebApiFssp.Services.anticaptcha;

namespace WebApiFssp.Services
{
    public class FsspData<T> : IFsspData where T: IAnticaptcha, new()
    {
        public string Data => _data;
        private T _anticaptcha;

        public FsspData()
        {
            _anticaptcha = new T();
        }

        private string _data;

        public void GetDataFromFssp(FsspPerson person)
        {
            _data = person.ToString() + _anticaptcha.ToString();
        }
    }
}