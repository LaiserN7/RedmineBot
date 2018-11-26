using System.Collections.Specialized;
using System.Threading.Tasks;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace RedmineBot.Services
{
    public interface IRedmineService
    {
        Task<PaginatedObjects<T>> GetAll<T>(NameValueCollection parameters = null) where T : class, new();
        Task<T> Create<T>(T rdObject) where T: class, new();
        Task<T> Get<T>(string rdObjectId, NameValueCollection parameters = null) where T : class, new();
        Task<T> Update<T>(string rdObjectId, T rdObject) where T : class, new();
        Task<bool> Delete<T>(string rdObjectId) where T : class, new();
        Task<User> GetCurrentUser();

        RedmineManager Manager { get; set; }
    }
}
