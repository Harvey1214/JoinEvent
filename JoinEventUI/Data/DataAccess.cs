using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace JoinEventUI.Data
{
    public class DataAccess
    {
        public Event GetEvent(int id)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                var output = connection.Query<Event>($"select * from EventsTable where Id = @Id", new { Id = id }).ToList();

                if (output.Count > 0)
                {
                    return output.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Event> GetEvents()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                return connection.Query<Event>($"select * from EventsTable where Date >= '{ DateTime.Now }' order by Date asc").ToList();
            }
        }
        
        public void InsertEvent(string name, string password, int maxParticipants, DateTime date, string htmlMessage, string htmlPage)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Execute("insert into EventsTable(Id, Name, Password, ParticipantCount, MaxParticipants, Date, HTMLMessage, HTMLPage) " +
                    "values (@Id, @Name, @Password, @ParticipantsCount, @MaxParticipants, @Date, @HTMLMessage, @HTMLPage)", 
                    new { Id = GetHighestId() + 1, Name = name, Password = password, ParticipantsCount = 0, MaxParticipants = maxParticipants, Date = date, HTMLMessage = htmlMessage, HTMLPage = htmlPage });
            }
        }

        private int GetHighestId()
        {
            List<Event> events = GetEvents();

            int maxId = 0;
            foreach (var eventData in events)
            {
                if (eventData.Id > maxId)
                {
                    maxId = eventData.Id;
                }
            }

            return maxId;
        }
    }
}
