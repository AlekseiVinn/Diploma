using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ScopeLap.DataBaseEngine;

namespace ScopeLap.Models
{
    public class ListSessionViewModel
    {

        public int? Id { get; set; }

        public int? Time { get; set; }

        public String PrintTime { get; set; }

        public int CarID { get; set; }

        public String CarName { get; set; }

        public int UserId { get; set; }

        public String Username { get; set; }

        public int TrackId { get; set; }

        public int TrackConfigId { get; set; }

        public String TrackName { get; set; }

        public DateOnly SessionDate { get; set; }

        public String? SessionNote { get; set; }

        public static async Task<ListSessionViewModel> getBestAsync(ScopeLapDbContext context, int userID) {

            var userSessions = await context.Sessions
                .Where(session => session.AccountId == userID)
                .OrderBy(session => session.LapTime)
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track)
                .Select(res => 
                    new ListSessionViewModel{
                        Id = res.Id,
                        Time = res.LapTime,
                        PrintTime = (
                            $"{(res.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                            $":{(res.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                            $":{(res.LapTime % 1000).ToString().PadLeft(3, '0')}"
                        ),
                        CarID = res.Car.Id,
                        CarName = $"{res.Car.Manufacturer} {res.Car.Model}",
                        UserId = res.Account.Id,
                        Username = $"{res.Account.Firstname} {res.Account.Lastname}",
                        TrackId = res.Track.Track.Id,
                        TrackConfigId = (int)res.TrackId,
                        TrackName = $"{res.Track.Track.Name}: {res.Track.Name} - {res.Track.Length} м",
                        SessionDate = res.TrackDate
                    }
               ).FirstOrDefaultAsync();

            return userSessions;
        }

        public static async Task<ListSessionViewModel> getSessionByIdAsync(ScopeLapDbContext context, int sessionId)
        {
            var userSession = await context.Sessions
                .Where(session => session.Id == sessionId)
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track)
                .Select(
                     session => new ListSessionViewModel
                     {
                         Id = session.Id,
                         PrintTime = (
                             $"{(session.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                             $":{(session.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                             $":{(session.LapTime % 1000).ToString().PadLeft(3, '0')}"
                         ),
                         CarID = session.Car.Id,
                         CarName = $"{session.Car.Manufacturer} {session.Car.Model}",
                         UserId = session.Account.Id,
                         Username = $"{session.Account.Firstname} {session.Account.Lastname}",
                         TrackId = session.Track.Track.Id,
                         TrackConfigId = (int)session.TrackId,
                         TrackName = $"{session.Track.Track.Name}: {session.Track.Name} - {session.Track.Length} м",
                         SessionDate = session.TrackDate,
                         SessionNote = session.LapNote
                     }
                 )
                .FirstOrDefaultAsync();

            return userSession;
        }

        public static async Task<List<ListSessionViewModel>> getMySessions(ScopeLapDbContext context, int userID) {
            
            var userSessions = await context.Sessions
                .Where(acc => acc.AccountId == userID)
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track)
                .Select(session => 
                    new ListSessionViewModel{
                        Id = session.Id,
                        Time = session.LapTime,
                        PrintTime = (
                            $"{(session.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 1000).ToString().PadLeft(3, '0')}"
                        ),
                        CarID = session.Car.Id,
                        CarName = $"{session.Car.Manufacturer} {session.Car.Model}",
                        UserId = session.Account.Id,
                        Username = $"{session.Account.Firstname} {session.Account.Lastname}",
                        TrackId = session.Track.Track.Id,
                        TrackConfigId = (int)session.TrackId,
                        TrackName = $"{session.Track.Track.Name}: {session.Track.Name} - {session.Track.Length} м",
                        SessionDate = session.TrackDate
                    }
                )
                .ToListAsync();

            return userSessions;
        }

        public static async Task<List<ListSessionViewModel>> getTop100(ScopeLapDbContext context)
        {

            var userSessions = await context.Sessions
                .Take(100)
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track)
                .Select(session =>
                    new ListSessionViewModel
                    {
                        Id = session.Id,
                        Time = session.LapTime,
                        PrintTime = (
                            $"{(session.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 1000).ToString().PadLeft(3, '0')}"
                        ),
                        CarID = session.Car.Id,
                        CarName = $"{session.Car.Manufacturer} {session.Car.Model}",
                        UserId = session.Account.Id,
                        Username = $"{session.Account.Firstname} {session.Account.Lastname}",
                        TrackId = session.Track.Track.Id,
                        TrackConfigId = (int)session.TrackId,
                        TrackName = $"{session.Track.Track.Name}: {session.Track.Name} - {session.Track.Length} м",
                        SessionDate = session.TrackDate
                    }
                )
                .OrderBy(l => l.TrackConfigId)
                .ThenBy(l => l.Time)
                .ToListAsync();
            return userSessions;
        }

    }

}
