using System;
using System.Collections.Generic;

namespace ImpersonationTest.Models
{
    public class InternalMeeting
    {
        public int MeetingId { get; set; }
        public int MeetingFamilyId { get; set; }
        public string Biennium { get; set; }
        public DateTime? MeetingDate { get; set; }
        public DateTime? StartTime { get; set; }
        public string CommitteeType { get; set; }
        public string ChangeNoticeInformation { get; set; }
        public string Note { get; set; }
        public string Phone { get; set; }
        public bool Tvwflag { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? RevisedStateModifiedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? RepublishedDate { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRevised { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsUnsubmitted { get; set; }
        public bool IsDraft { get; set; }
        public bool IsRecorded { get; set; }

        public bool IsNoMeeting { get; set; }

        public bool IsDateTBA { get; set; }
        public bool IsTimeTBA { get; set; }
    }
}
