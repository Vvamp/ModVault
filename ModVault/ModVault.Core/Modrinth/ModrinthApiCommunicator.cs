using ModVault.Core.Modrinth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace ModVault.Core.Modrinth
{
    public class ModrinthApiCommunicator
    {
        private const string BaseUrl = "https://api.modrinth.com/v2";
        private string UserAgent = $"Vvamp/ModVault/{System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()} (vincentvansetten.com)";

        private HttpClient GenerateHttpClient(string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);

            return client;
        }

        public async Task<Project?> GetProject(string id)
        {
            Project dataObject;
            var url = $"{BaseUrl}/project/{id}";
            HttpClient client = GenerateHttpClient(url);

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                dataObject = await response.Content.ReadAsAsync<Project>();
            }
            else
            {
                return null;
            }

            client.Dispose();
            return dataObject;

        }

        public async Task<Team?> GetTeamMembersByProject(string project_id)
        {
            Team dataObject = new Team();
            var url = $"{BaseUrl}/project/{project_id}/members";
            HttpClient client = GenerateHttpClient(url);



            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var members = await response.Content.ReadAsAsync<IEnumerable<TeamMembers>>();
                foreach (var member in members)
                {
                    dataObject.members.Add(member);
                }
            }
            else
            {
                return null;
            }

            client.Dispose();
            return dataObject;
        }

        public async Task<Versions?> GetVersionsByProject(string project_id)
        {
            Versions dataObject = new Versions();
            var url = $"{BaseUrl}/project/{project_id}/version";
            HttpClient client = GenerateHttpClient(url);

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var versions = await response.Content.ReadAsAsync<IEnumerable<Models.Version>>();
                foreach (var version in versions)
                {
                    dataObject.versions.Add(version);
                }
            }
            else
            {
                return null;
            }

            client.Dispose();
            return dataObject;
        }
    }
}
