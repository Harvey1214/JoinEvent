using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using JoinEventUI.Data.DataObjects;

namespace JoinEventUI.Data
{
    public class DataAccess
    {
        #region Events
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

        /// <summary>
        /// Gets a list of events which didn't start yet ordered in ascending order by start DateTime
        /// </summary>
        /// <returns></returns>
        public List<Event> GetEvents()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                return connection.Query<Event>($"select * from EventsTable where Date >= '{ DateTime.Now }' order by Date asc").ToList();
            }
        }

        public List<Event> GetAllEvents()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                return connection.Query<Event>($"select * from EventsTable order by Date asc").ToList();
            }
        }

        public int InsertEvent(string name, string password, int maxParticipants, DateTime date, string htmlMessage, string htmlPage)
        {
            int newId = GetHighestId() + 1;

            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Execute("insert into EventsTable(Id, Name, Password, ParticipantCount, MaxParticipants, Date, HTMLMessage, HTMLPage) " +
                    "values (@Id, @Name, @Password, @ParticipantsCount, @MaxParticipants, @Date, @HTMLMessage, @HTMLPage)", 
                    new { Id = newId, Name = name, Password = password, ParticipantsCount = 0, MaxParticipants = maxParticipants, Date = date, HTMLMessage = htmlMessage, HTMLPage = htmlPage });
            }

            return newId;
        }

        public void UpdateParticipantCount(int atendeesToAdd, int eventId)
        {
            Event eventToUpdate = GetEvent(eventId);

            int updatedAtendeeCount = eventToUpdate.ParticipantCount + atendeesToAdd;

            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Execute("update EventsTable set ParticipantCount = @ParticipantCount where Id = @Id",
                    new { ParticipantCount = updatedAtendeeCount, Id = eventId });
            }
        }

        private int GetHighestId()
        {
            List<Event> events = GetAllEvents();

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
        #endregion Events

        #region Participants
        public List<Participant> GetAllParticipants()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                return connection.Query<Participant>($"select * from ParticipantsTable").ToList();
            }
        }

        public List<Participant> GetParticipants(int eventId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                return connection.Query<Participant>($"select * from ParticipantsTable where EventId = @EventId", new { EventId = eventId }).ToList();
            }
        }

        public void InsertParticipant(int eventId, string fullName, string email, string phoneNumber, int atendeeCount)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Execute("insert into ParticipantsTable(Id, EventId, FullName, Email, PhoneNumber, AtendeeCount) " +
                    "values (@Id, @EventId, @FullName, @Email, @PhoneNumber, @AtendeeCount)",
                    new { Id = GetHighestParticipantId() + 1, EventId = eventId, FullName = fullName, Email = email, PhoneNumber = phoneNumber, AtendeeCount = atendeeCount });
            }
        }

        private int GetHighestParticipantId()
        {
            List<Participant> participants = GetAllParticipants();

            int maxId = 0;
            foreach (var participant in participants)
            {
                if (participant.Id > maxId)
                {
                    maxId = participant.Id;
                }
            }

            return maxId;
        }
        #endregion Participants
    }
}
