using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ModVault.Core.Modrinth.Models
{
    public class PayoutData
    {
        public decimal? balance { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? bio { get; set; }
    }
    public class User
    {
        public string username { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? bio { get; set; }
        public PayoutData? payout_data { get; set; }
        public string id { get; set; }
        public int? github_id { get; set; }
        public string? avatar_url { get; set; }
        public string? created { get; set; }
        public string? role { get; set; }
        public int? badges { get; set; }
    }
    public class TeamMembers
    {
        public string team_id { get; set; }
        public User user { get; set; }
        public string? role { get; set; }
        public int? permissions { get; set; }
        public bool accepted { get; set; }
        public int? payouts_split { get; set; }
        public int? ordering { get; set; }
    }
    public class Team
    {
        public List<TeamMembers> members { get; set; } = new List<TeamMembers>();
    }
}
