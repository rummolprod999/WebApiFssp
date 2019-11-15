using WebApiFssp.Helpers;
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
            var firstUrl = CreateUrlFirstRequest(person);
            var captchaRaw = NetworkHelpers.DownLUserAgent(firstUrl);
            ExtractImg(captchaRaw);
            
        }

        private string CreateUrlFirstRequest(FsspPerson person)
        {
            var lastName = person.LastName ?? "";
            var firstName = person.FirstName ?? "";
            var patronymic = person.MiddleName ?? "";
            var birthDate = person.BirthDate ?? "";
            var region = person.Regions.Count > 0 ? person.Regions[0] : -1;
            return $"https://is.fssprus.ru/ajax_search?callback=jQuery3400049965671950707335_1573640973805&system=ip&is[extended]=1&nocache=1&is[variant]=1&is[region_id][0]={region}&is[last_name]={lastName}&is[first_name]={firstName}&is[drtr_name]=&is[ip_number]=&is[patronymic]={patronymic}&is[date]={birthDate}&is[address]=&is[id_number]=&is[id_type][0]=&is[id_issuer]=&_=1573640973810";
        }

        private void ExtractImg(string raw)
        {
            var imageBase = raw.GetDataFromRegex(@"data:image/jpeg;base64,(.+?)\\\"" id=\\""");
            _data = _anticaptcha.DateFromImage(imageBase);
        }
    }
}