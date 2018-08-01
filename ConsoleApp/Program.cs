using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Globalization;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var url = "https://api.github.com/orgs/dotnet/repos";

            var repositories = ProcessGitHubRepositories(url).Result;

            foreach (var repo in repositories)
            {
                Console.WriteLine(repo.Name);
                Console.WriteLine(repo.Description);
                Console.WriteLine(repo.GitHubHomeUri);
                Console.WriteLine(repo.Homepage);
                Console.WriteLine(repo.Watchers);
                Console.WriteLine(repo.LastPush);
                Console.WriteLine("------------------");
                Console.WriteLine();
            }


            Console.ReadLine();
        }

        private static async Task<List<Repository>> ProcessGitHubRepositories(string GitHubUrl)
        {

            var serializer = new DataContractJsonSerializer(typeof(List<Repository>));


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                var streamTask = client.GetStreamAsync(GitHubUrl);
                var repositories = serializer.ReadObject(await streamTask) as List<Repository>;
                return repositories;
           
        }
    }


    [DataContract(Name ="repo")]
    public class Repository
    {   
        [DataMember(Name = "name")]
        private string _name;

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name ="html_uei")]
        public Uri GitHubHomeUri { get; set; }

        [DataMember(Name ="homepage")]
        public Uri Homepage { get; set; }
        
        [DataMember(Name = "watchers")]
        public int Watchers { get; set; }

        [DataMember(Name = "pushed_at")]
        private string JsonData { get; set; }

        [IgnoreDataMember]
        public DateTime LastPush
        {
            get
            {
                return DateTime.ParseExact(JsonData, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if(_name != value)
                {
                    _name = value;
                }
            }
        }

    }
}
