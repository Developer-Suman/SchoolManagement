using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public enum ActivityCategory
    {
        Sports,           // Athletics, Games
        Academic,         // Quiz, Debates
        CreativeArts,     // Decoration, Crafts
        Environmental,    // Flowering, Gardening
        PerformingArts,   // Music, Dance
        Technical,        // Robotics, Coding
        SocialService,    // Community work
        Vocational        // Practical life skills
    }

    public enum AwardPosition
    {
        // The Classic Winners
        FirstPlace = 1,
        SecondPlace = 2,
        ThirdPlace = 3,

        // High Performance (For Quiz/Sports)
        RunnerUp = 4,
        HonorableMention = 5,

        // Specialized Recognition (For Decoration/Flowering/Creativity)
        GoldStandard = 6,      // Exceptional quality
        CreativeExcellence = 7, // Unique approach
        BestTeamLeader = 8,    // Leadership role

        // Universal Appreciation
        ActiveParticipant = 9, // Ensures "Every Student" is recognized
        OutstandingEffort = 10  // For students who worked hard but didn't "win"
    }


    public enum EventType
    {
        Academic = 1,
        Sports = 2,
        Cultural = 3,
        Seminar = 4,
        Workshop = 5,
        Competition = 6,
        Meeting = 7,
        Celebration = 8,
        Holiday = 9,
        Examination = 10,
        Other = 99
    }
}
