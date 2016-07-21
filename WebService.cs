using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;


namespace Motion.Core.WSHandler
{
	public class WebService : IWebServicesWrapper
	{
		public WebService()
		{
		}

		public async Task<Dictionary<string, object>> GetData(string URL, string param = "")
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetAsync(URL + param);

				string responseStr = await result.Content.ReadAsStringAsync();
				Dictionary<string, object> responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseStr);

				return responseDict;
			}
		}

		public async Task<Dictionary<string, object>> PostData(string URL, Dictionary<string, object> data)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
				var response = await client.PostAsync(URL, content);

				string responseStr = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(responseStr);
				Dictionary<string, object> responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseStr);
				return responseDict;
			}
		}

		public async Task<Dictionary<string, object>> PostMultiPartData(string URL, byte file, string filename)
		{
			using (var client = new HttpClient())
			{
				using (var content =
					new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
				{
					content.Add(new StreamContent(new MemoryStream(file)), filename, filename);

					using (var message = await client.PostAsync(URL, content))
					{
						string responseStr = await message.Content.ReadAsStringAsync();
						Dictionary<string, object> responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseStr);
						return responseDict;
					}
				}
			}
		}
	}
}

