using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Redmine.Net.Api;
using Redmine.Net.Api.Async;
using Redmine.Net.Api.Exceptions;
using Redmine.Net.Api.Types;
using RedmineBot.Helpers;

namespace RedmineBot.Services
{
    public class RedmineService : IRedmineService
    {
        private readonly NameValueCollection _defaultParams = new NameValueCollection { { RedmineKeys.STATUS_ID, RedmineKeys.ALL } };

        public RedmineManager Manager { get; set; }

        public RedmineService(IOptions<RedmineConfiguration> config, RedmineManager manger = null)
        {
            Manager = manger ?? new RedmineManager(config.Value.Host, config.Value.ApiKey);
        }

        public Task<PaginatedObjects<T>> GetAll<T>(NameValueCollection parameters = null) where T : class, new()
        {
            if (parameters == null)
                parameters = _defaultParams;

            return Manager.GetPaginatedObjectsAsync<T>(parameters);
        }

        public Task<User> GetCurrentUser()
        {
            return Manager.GetCurrentUserAsync();
        }


        #region CRUD
        public Task<T> Create<T>(T rdObject) where T: class, new()
        {
            return Manager.CreateObjectAsync(rdObject);
        }

        public Task<T> Get<T>(string rdObjectId, NameValueCollection parameters = null ) where T : class, new()
        {
            if (string.IsNullOrEmpty(rdObjectId))
                throw new ArgumentNullException(nameof(rdObjectId), "Parameter can't be empty");

            //remove
            if (parameters == null)
                parameters = _defaultParams;

            return Manager.GetObjectAsync<T>(rdObjectId, parameters);
        }

        public async Task<T> Update<T>(string rdObjectId, T rdObject) where T : class, new()
        {
            if (!(rdObject is Issue issue))
                throw new NotSupportedException($"Only type {nameof(Issue)} supported");

            if (issue.Id.ToString() != rdObjectId)
                throw new ApplicationException("Mismatch between rdObjectId <---> id of issue");

            await Manager.UpdateObjectAsync(rdObjectId, issue);
            return await Manager.GetObjectAsync<T>(rdObjectId, null);

        }

        public async Task<bool> Delete<T>(string rdObjectId) where T : class, new()
        {
            if (string.IsNullOrEmpty(rdObjectId))
                throw new ArgumentNullException(nameof(rdObjectId), "Parameter can't be empty");
             
            try
            {
                await Manager.DeleteObjectAsync<T>(rdObjectId, null);
            }
            catch (NotFoundException e)
            {
                throw new NotFoundException($"Object {nameof(rdObjectId)} = {rdObjectId} not found");
            }
            catch (RedmineException rex)
            {
                throw new RedmineException($"Delete object returned exception {rex.Message}");
            }

            return true;
        }
        #endregion

    }
}
