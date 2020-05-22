using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetYouServer.Configurations;
using BetYouServer.Models;

namespace BetYouServer.Controllers
{
    public class SportController
    {

        private readonly DatabaseController DBController = Configuration.GetService<DatabaseController>();

        public List<(Match, Team, Team)> GetMatches()
        {
            ExecutionResult result;
            Match match = new Match(); match.Attributes.AddRange(new List<Match.Attribute>()
            {
                Match.Attribute.ID,
                Match.Attribute.Date,
                Match.Attribute.Result
            });
            result = DBController.SelectData(match, new Match(), new List<DatabaseQuery.Condition>());
            match.Attributes.Clear();

            List<DatabaseModel> resultData = result.Data;
            List<(Match, Team, Team)> matches = new List<(Match, Team, Team)>();
            MatchOpponent opponent = new MatchOpponent(); opponent.Attributes.Add(MatchOpponent.Attribute.Team);
            MatchOpponent tempOppo = new MatchOpponent(); tempOppo.Attributes.Add(MatchOpponent.Attribute.Match);
            Team team = new Team(), teamS, teamT; team.Attributes.AddRange(new List<Team.Attribute>()
            {
                Team.Attribute.Name,
                Team.Attribute.City,
                Team.Attribute.StarAmount,
                Team.Attribute.CoupeAmount,
                Team.Attribute.Info
            });
            foreach (DatabaseModel model in resultData)
            {
                match = model as Match;
                tempOppo.Match = match;
                result = DBController.SelectData(opponent, tempOppo, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal });
                teamS = (result.Data.ElementAt(0) as MatchOpponent).Team; teamS.Attributes.Add(Team.Attribute.ID);
                teamT = (result.Data.ElementAt(1) as MatchOpponent).Team; teamT.Attributes.Add(Team.Attribute.ID);
                teamS = DBController.SelectData(team, teamS, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal }).Data.First() as Team;
                teamT = DBController.SelectData(team, teamT, new List<DatabaseQuery.Condition>() { DatabaseQuery.Condition.Equal }).Data.First() as Team;
                matches.Add((match, teamS, teamT));
            }
            opponent.Attributes.Clear(); tempOppo.Attributes.Clear(); team.Attributes.Clear();
            return matches;
        }

    }
}
