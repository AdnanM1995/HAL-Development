﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ITMatching.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ITMatching.ViewModels;
using ITMatching.Models.Abstract;
using System.Threading.Tasks;

namespace ITMatching.Controllers
{
    public class MatchingController : Controller
    {
        private readonly ILogger<MatchingController> logger;
        private readonly UserManager<IdentityUser> _userManager;
        ITMatchingAppContext context;
        private readonly IItmuserRepository _itmuserRepo;
        private readonly IExpertRepository _expertRepo;
        private readonly IMeetingRepository _meetingRepo;
        private readonly IHelpRequestRepository _helpRequestRepo;

        public MatchingController(ILogger<MatchingController> logger, UserManager<IdentityUser> userManager, ITMatchingAppContext ctx,
            IItmuserRepository itmuserRepo, IExpertRepository expertRepo, IMeetingRepository meetingRepo, IHelpRequestRepository helpRequestRepo)
        {
            this.logger = logger;
            _userManager = userManager;
            context = ctx;
            _itmuserRepo = itmuserRepo;
            _expertRepo = expertRepo;
            _meetingRepo = meetingRepo;
            _helpRequestRepo = helpRequestRepo;
        }

        [Authorize]
        public IActionResult RequestForm()
        {
            RequestFormViewModel viewModel = new RequestFormViewModel();
            viewModel.Services = context.Services.ToList();
            viewModel.HelpRequest = new HelpRequest();

            return View(viewModel);
        }

        [Authorize]
        public IActionResult HelpRequestAdded()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HelpRequest(HelpRequest helpRequest, int?[] TagIds)
        {
            if (ModelState.IsValid)
            {
                string id = _userManager.GetUserId(User);
                Itmuser itUser = context.Itmusers.Where(u => u.AspNetUserId == id).FirstOrDefault();
                helpRequest.ClientId = itUser.Id;
                int ID;

                int matchingRequestsCount = context.HelpRequests.Where(hr => hr.RequestTitle == helpRequest.RequestTitle && hr.RequestDescription == helpRequest.RequestDescription && hr.ClientId == helpRequest.ClientId).Count();

                if (matchingRequestsCount > 0)
                {
                    HelpRequest existingRequest = context.HelpRequests.Where(hr => hr.RequestTitle == helpRequest.RequestTitle && hr.RequestDescription == helpRequest.RequestDescription && hr.ClientId == helpRequest.ClientId).FirstOrDefault();
                    existingRequest.IsOpen = true;

                    context.HelpRequests.Update(existingRequest);
                    ID = existingRequest.Id;
                }
                else
                {
                    helpRequest.IsOpen = true;
                    context.HelpRequests.Add(helpRequest);

                    ID = context.HelpRequests.Count() + 1;
                }

                foreach (int i in TagIds)
                {
                    Debug.WriteLine("Tag ID is " + i);
                    RequestService entry = new RequestService();
                    entry.RequestId = ID;
                    entry.ServiceId = i;
                    int matchingRequestServiceCount = context.RequestServices.Where(rs => rs.RequestId == ID && rs.ServiceId == i).Count();

                    if (matchingRequestServiceCount == 0)
                    {
                        context.RequestServices.Add(entry);
                    }

                }

                List<int> requestServiceIDsInDataTable = context.RequestServices.Select(rs => rs.ServiceId).ToList();

                foreach (int i in requestServiceIDsInDataTable)
                {
                    if (!TagIds.Contains(i))
                    {
                        RequestService serviceToRemove = context.RequestServices.Where(rs => rs.ServiceId == i).FirstOrDefault();
                        context.RequestServices.Remove(serviceToRemove);
                    }
                }

                List<RequestSchedule> currentHours = context.RequestSchedules.Where(rs => rs.RequestId == ID).ToList();
                foreach (RequestSchedule rs in currentHours)
                {
                    context.RequestSchedules.Remove(rs);
                }

                context.SaveChanges();
                return RedirectToAction("HelpRequestAdded");
            }

            return RedirectToAction("RequestForm", "Matching");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HelpRequestWithSchedule(HelpRequest helpRequest, int?[] TagIds)
        {
            if (ModelState.IsValid)
            {
                string id = _userManager.GetUserId(User);
                Itmuser itUser = context.Itmusers.Where(u => u.AspNetUserId == id).FirstOrDefault();
                helpRequest.ClientId = itUser.Id;
                int ID;

                int matchingRequestsCount = context.HelpRequests.Where(hr => hr.RequestTitle == helpRequest.RequestTitle && hr.RequestDescription == helpRequest.RequestDescription && hr.ClientId == helpRequest.ClientId).Count();

                if (matchingRequestsCount > 0)
                {
                    HelpRequest existingRequest = context.HelpRequests.Where(hr => hr.RequestTitle == helpRequest.RequestTitle && hr.RequestDescription == helpRequest.RequestDescription && hr.ClientId == helpRequest.ClientId).FirstOrDefault();
                    existingRequest.IsOpen = true;

                    context.HelpRequests.Update(existingRequest);
                    ID = existingRequest.Id;
                }
                else
                {
                    helpRequest.IsOpen = true;
                    context.HelpRequests.Add(helpRequest);

                    ID = context.HelpRequests.Count() + 1;
                }

                foreach (int i in TagIds)
                {
                    Debug.WriteLine("Tag ID is " + i);
                    RequestService entry = new RequestService();
                    entry.RequestId = ID;
                    entry.ServiceId = i;
                    int matchingRequestServiceCount = context.RequestServices.Where(rs => rs.RequestId == ID && rs.ServiceId == i).Count();

                    if (matchingRequestServiceCount == 0)
                    {
                        context.RequestServices.Add(entry);
                    }

                }

                List<int> requestServiceIDsInDataTable = context.RequestServices.Select(rs => rs.ServiceId).ToList();

                foreach (int i in requestServiceIDsInDataTable)
                {
                    if (!TagIds.Contains(i))
                    {
                        RequestService serviceToRemove = context.RequestServices.Where(rs => rs.ServiceId == i).FirstOrDefault();
                        context.RequestServices.Remove(serviceToRemove);
                    }
                }

                List<RequestSchedule> currentHours = context.RequestSchedules.Where(rs => rs.RequestId == ID).ToList();
                foreach (RequestSchedule rs in currentHours)
                {
                    context.RequestSchedules.Remove(rs);
                }

                context.SaveChanges();
                return RedirectToAction("RequestScheduler");
            }

            return RedirectToAction("RequestForm", "Matching");
        }

        public IActionResult ResubmitHelpRequest(int helpRequestID)
        {
            //Use HelpRequestID passed in from history page to get list of the helpRequest's already selected checkboxes
            List<int> checkedServiceBoxes = context.RequestServices.Where(rs => rs.RequestId == helpRequestID).Select(id => id.ServiceId).ToList();

            ResubmitFormViewModel viewModel = new ResubmitFormViewModel();
            viewModel.Services = context.Services.ToList();
            viewModel.HelpRequest = context.HelpRequests.Where(hr => hr.Id == helpRequestID).FirstOrDefault();
            viewModel.checkedBoxes = checkedServiceBoxes;

            return View(viewModel);
        }


        public IActionResult RequestScheduler()
        {

            return View();
        }

        [HttpPost]
        //Attempt to meet with an online expert, and either navigate to a meeting room to talk to them, or to a page listing expert information if no matching experts are available.
        public IActionResult ClientExpertMatching(Meeting meeting)
        {
            //Compile a list of IDs from all experts who are currently available for matching
            List<int> onlineExperts = context.Experts.Where(e => e.IsAvailable == true).Select(e => e.Id).ToList();

            //Compile a list of the IDs of the services tagged by the client for their help request
            List<int> helpRequestServiceIDs = context.RequestServices.Where(rs => rs.RequestId == meeting.HelpRequestId).Select(rs => rs.ServiceId).ToList();

            //Find the maximum value of points associated with a client's help request to compare experts against
            int clientMaxPoints = GetServicePoints(helpRequestServiceIDs);

            //If the client's max points associated with their help request is 0, then they have no tags, so return an empty list because they will not be able to match with anyone.
            if (clientMaxPoints == 0)
            {
                List<(int, double)> emptyList = new List<(int, double)>();
                return View(emptyList);
            }

            //Create a list to store IDs and matching scores of experts who meet the matching score threshold "List<(ExpertID, matchingScore)>"
            List<(int, double)> thresholdMeetingExperts = FindThresholdMeetingExperts(onlineExperts, helpRequestServiceIDs, clientMaxPoints);

            //Attempt to meet with online experts who meet the matching score threshold for this help request
            if (thresholdMeetingExperts.Any())
            {
                //Create new list for Experts who meet the threshold sorted by descending order
                List<(int, double)> sortedExperts = thresholdMeetingExperts.OrderByDescending(t => t.Item2).ToList();

                //Iterate through each expert in the list, starting from highest matching score, until an expert accepts to meet, or until the list is exhausted
                foreach ((int, double) ex in sortedExperts)
                {
                    int expertId = ex.Item1;
                    meeting.ExpertId = ex.Item1;
                    meeting.MatchExpireTimestamp = DateTime.UtcNow.AddMinutes(2);
                    string meetingStatus;
                    context.Meetings.Update(meeting);
                    context.SaveChanges();
                    while (expertId != 0)
                    {
                        expertId = context.Meetings.Where(m => m.Id == meeting.Id).Select(e => e.ExpertId).SingleOrDefault();
                        meetingStatus = context.Meetings.Where(m => m.Id == meeting.Id).Select(s => s.Status).SingleOrDefault();
                        if (meetingStatus == "Matched")
                        {
                            return RedirectToAction("Meeting", "Matching", new { id = meeting.Id });
                        }

                        //Set timestamp for expert waiting room to verify that we are still around
                        meeting.ClientTimestamp = DateTime.UtcNow;
                        
                        context.Meetings.Update(meeting);
                        context.SaveChanges();

                        //If expert does not update timestamp for 30 seconds, they are assumed to be afk and are set to unavailable
                        if (ExpertIsNotThere(meeting.ExpertTimestamp))
                        {
                            Expert afkExpert = context.Experts.Where(e => e.Id == expertId).FirstOrDefault();
                            afkExpert.IsAvailable = false;
                            context.Update(afkExpert);
                            context.SaveChanges();
                            expertId = 0;
                        }

                        if (DateTime.Compare(DateTime.UtcNow, meeting.MatchExpireTimestamp) > 0)
                        {
                            expertId = 0;
                        }
                    }
                }
            }

            //perform 2nd pass of algorithm since matching with a currently-available expert has failed

            //Check database to see if there are preferred hours for this help request then do one of two second passes depending on if any preferred hours are found
            bool helpRequestHasSchedule = context.RequestSchedules.Where(rs => rs.RequestId == meeting.HelpRequestId).Any();

            //Algorithm Second Pass (Trying to find offline experts to meet with later)
            List<(int, double)> offlineExpertIdsAndScores = FindUnavailableMatchingExperts(meeting, helpRequestHasSchedule, helpRequestServiceIDs, clientMaxPoints);

            return View(offlineExpertIdsAndScores);
        }

        public List<(int, double)> FindThresholdMeetingExperts(List<int> expertIDs, List<int> helpRequestServiceIDs, int clientMaxPoints)
        {
            //Initialize values for generating matching score
            double threshold = 0.75;
            (int, double) IdMatchingScorePair = (0, 0);

            //Create list to fill with threshold meeting experts to then pass back
            List<(int, double)> thresholdMeetingExperts = new List<(int, double)>();

            foreach (int id in expertIDs)
            {
                //Set initial expert values to 0
                double expertMatchingScore = 0;

                //Compile list of the IDs of the services tagged by the expert
                List<int> ExpertServiceIDs = context.ExpertServices.Where(es => es.ExpertId == id).Select(es => es.ServiceId).ToList();

                //Compile list of the IDs of the services shared by the expert and the help request
                List<int> sharedServiceIDs = ExpertServiceIDs.Where(id => helpRequestServiceIDs.Contains(id)).ToList();

                //Determine the value of tags that the current expert shares with the client's help request
                int expertMatchingPoints = GetServicePoints(sharedServiceIDs);

                //Calculate matching score for current expert
                expertMatchingScore = expertMatchingPoints / clientMaxPoints;
                if (expertMatchingScore >= threshold)
                {
                    IdMatchingScorePair = (id, expertMatchingScore);
                    thresholdMeetingExperts.Add(IdMatchingScorePair);
                }
            }
            return thresholdMeetingExperts;
        }

        //Returns a list of threshold-meeting experts who are unavailable to meet with right now
        public List<(int, double)> FindUnavailableMatchingExperts(Meeting meeting, bool helpRequestHasSchedule, List<int> helpRequestServiceIDs, int clientMaxPoints)
        {
            List<int> unavailableExpertIds = context.Experts.Select(e => e.Id).ToList();

            List<(int, double)> thresholdMeetingExperts = FindThresholdMeetingExperts(unavailableExpertIds, helpRequestServiceIDs, clientMaxPoints);

            //Check if any experts meet the threshold 
            if (thresholdMeetingExperts.Any())
            {
                if (helpRequestHasSchedule)
                {
                    //Get schedule values that have a matching helpRequestId to our meeting's helpRequestId
                    List<RequestSchedule> requestSchedule = context.RequestSchedules.Where(rs => rs.RequestId == meeting.HelpRequestId).ToList();
                    int clientAvailableHoursCount = requestSchedule.Count;

                    for (int i = 0; i < thresholdMeetingExperts.Count; i++)
                    {
                        //Get schedule values that have a matching value in the expert's schedule
                        List<WorkSchedule> expertSchedule = context.WorkSchedules.Where(ws => ws.ExpertId == thresholdMeetingExperts[i].Item1).ToList();
                        List<WorkSchedule> expertScheduleMatchingClientHours = new List<WorkSchedule>();
                        
                        //iterate through expert's work schedule and create a list of only the times that they share with the helpRequest
                        foreach(WorkSchedule ws in expertSchedule)
                        {
                            foreach(RequestSchedule rs in requestSchedule)
                            {
                                if (ws.Day == rs.Day && ws.Hour == rs.Hour)
                                {
                                    expertScheduleMatchingClientHours.Add(ws);
                                }
                            }
                            
                        }

                        int expertMatchingHoursWithClient = expertScheduleMatchingClientHours.Count;
                        double hoursPercentage = expertMatchingHoursWithClient / clientAvailableHoursCount;
                        double newMatchingScore = thresholdMeetingExperts[i].Item2 * hoursPercentage;
                        thresholdMeetingExperts[i] = (thresholdMeetingExperts[i].Item1, newMatchingScore);


                    }
                }

                //Create new list for Experts who meet the threshold sorted by descending order
                List<(int, double)> sortedExperts = thresholdMeetingExperts.OrderByDescending(t => t.Item2).ToList();

                if (sortedExperts.Count > 10)
                {
                    double maxScore = sortedExperts[0].Item2;
                    List<(int, double)> tiedExperts = new List<(int, double)>();
                    tiedExperts.Add(sortedExperts[0]);

                    //check if and how many ties for highest matching score exist
                    for(int i = 1; i < sortedExperts.Count; i++)
                    {
                        if (sortedExperts[i].Item2 < maxScore)
                        {
                            break;
                        }

                        tiedExperts.Add(sortedExperts[i]);
                    }

                    //check if more than 10 experts tie for the highest matching score
                    if (tiedExperts.Count > 10)
                    {
                        //if more than 10 Experts tie for the highest matching score, they will be randomized to reduce alphabetical or other bias in listing them
                        Random random = new Random();
                        List<(int, double)> shuffledTiedExperts = tiedExperts.OrderBy(item => random.Next()).ToList();
                        List<(int, double)> top10ShuffledTiedExperts = shuffledTiedExperts.Take(10).ToList();
                        return top10ShuffledTiedExperts;
                    }

                    List<(int, double)> top10sortedExperts = sortedExperts.Take(10).ToList();
                    return top10sortedExperts;
                }

            }

                List<(int, double)> emptyList = new List<(int, double)>();
                return emptyList;
        }

        //Calculates the total point value of corresponding services from a list of serviceIDs
        public int GetServicePoints(List<int> serviceIDs)
        {
            int totalPoints = 0;

            foreach (int id in serviceIDs)
            {
                Service tag = context.Services.Where(s => s.Id == id).FirstOrDefault();

                switch (tag.ServiceCategory)
                {
                    case "OS":
                        totalPoints += 12;
                        break;

                    case "CommunicationMethod":
                        totalPoints += 15;
                        break;

                    default:
                        totalPoints += 10;
                        break;
                }
            }

            return totalPoints;
        }

        //Returns True if client has not updated their timestamp on a pending meeting object for over 30 seconds, likely indicating they are no longer in their waiting room
        public Boolean ClientIsNotThere(DateTime dbTime)
        {
            DateTime current = DateTime.UtcNow;
            TimeSpan span = current - dbTime;
            if (span.TotalSeconds > 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Returns True if expert has not updated their timestamp on a pending meeting object for over 30 seconds, likely indicating they are no longer in their waiting room

        public Boolean ExpertIsNotThere(DateTime dbTime)
        {
            DateTime current = DateTime.UtcNow;
            TimeSpan span = current - dbTime;
            if (span.TotalSeconds > 90)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Authorize]
        public async Task<IActionResult> ExpertWaitingRoom()
        {
            string id = _userManager.GetUserId(User);
            Itmuser itUser = await _itmuserRepo.GetByAspNetUserIdAsync(id);
            Expert eUser = await _expertRepo.GetByItmUserIdAsync(itUser.Id);
            if (eUser != null)
            {
                var meetingRequests = (await _meetingRepo.GetMatchingMeetingsByExpertIdAsync(eUser.Id))
                    .ToDictionary(m => m.Id, m => m.HelpRequestId);
                var meetings = (await _helpRequestRepo.GetListByIdsAsync(meetingRequests.Values.ToList()))
                    .Select(hr => new { MeetingId = meetingRequests.First(mr => mr.Value == hr.Id).Key, HelpRequest = hr })
                    .ToDictionary(o => o.MeetingId, o => o.HelpRequest);
                var ewrVM = new ExpertWaitingRoomViewModel
                {
                    Expert = eUser,
                    Meetings = meetings
                };
                return View(ewrVM);
            }
            else
            { return BadRequest(); }
        }

        [Authorize]
        public async Task<IActionResult> ClientWaitingRoom()
        {
            string id = _userManager.GetUserId(User);
            Itmuser itUser = await _itmuserRepo.GetByAspNetUserIdAsync(id);

            if (itUser != null)
            {
                Meeting meeting = new Meeting();
                meeting.Date = DateTime.UtcNow;
                meeting.ClientId = itUser.Id;
                meeting.ExpertId = 0;
                meeting.HelpRequestId = context.HelpRequests.Where(hr => hr.ClientId == itUser.Id && hr.IsOpen == true).Select(i => i.Id).FirstOrDefault();
                meeting.Status = "Matching";

                context.Meetings.Add(meeting);

                HelpRequest helpRequest = context.HelpRequests.Where(hr => hr.ClientId == itUser.Id && hr.IsOpen == true).FirstOrDefault();

                var clientWaitingRoomVM = new ClientWaitingRoomViewModel
                {
                    HelpRequest = helpRequest,
                    Meeting = meeting
                };

                return View(clientWaitingRoomVM);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeExpertStatus(int expertId)
        {
            await _expertRepo.ToggleStatusAsync(expertId);
            return RedirectToAction("ExpertWaitingRoom");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeMeetingStatus(int meetingId, string status)
        {
            await _meetingRepo.UpdateStatusAsync(meetingId, status);
            if (status != "accept")
            { return RedirectToAction("ExpertWaitingRoom"); }
            else
            { return RedirectToAction("Meeting", new { meetingId }); }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
