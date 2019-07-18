using ClassLibrary;
using Newtonsoft.Json;
using NSubstitute;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject
{
	public class UnitTest
	{
		[Fact]
		public async Task WhenACorrectUrlIsProvided_ServiceShouldReturnAlistOfUsers()
		{
			// Arrange
			var users = new List<User>
			{
				new User
				{
					FirstName = "Bill",
					LastName = "Gates"
				},
			};

			var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
			var url = "http://fake.com";
			var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json")
			});
			var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

			httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);

			// Act
			var userService = new UserService(httpClientFactoryMock);
			var result = await userService.Get(url);

			// Assert
			Assert.Equal("Bill", result[0].FirstName);
			Assert.Equal("Gates", result[0].LastName);
		}

		[Fact]
		public async Task WhenABadUrlIsProvided_ServiceShouldReturnNotFound()
		{
			// Arrange
			var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
			var url = "http://fake.com";
			var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
			{
				StatusCode = HttpStatusCode.NotFound
			});
			var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

			httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);

			// Act
			var userService = new UserService(httpClientFactoryMock);
			var result = await userService.Post(url);

			// Assert
			Assert.Equal(HttpStatusCode.NotFound, result);
		}
	}
}
