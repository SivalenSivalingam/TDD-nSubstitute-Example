using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class UserService
	{
		private readonly IHttpClientFactory _httpFactory;

		public UserService(IHttpClientFactory httpFactory)
		{
			_httpFactory = httpFactory;
		}

		public async Task<List<User>> Get(string url)
		{
			using (HttpClient httpclient = _httpFactory.CreateClient())
			{
				using (HttpResponseMessage response = await httpclient.GetAsync(url))
				{
					if (response.StatusCode == HttpStatusCode.OK)
					{
						return JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
					}
					return null;
				}
			}
		}

		public async Task<HttpStatusCode> Post(string url)
		{
			using (HttpClient httpclient = _httpFactory.CreateClient())
			{
				using (HttpResponseMessage response = await httpclient.PostAsync(url, null))
				{
					return response.StatusCode;
				}
			}
		}
	}
}
