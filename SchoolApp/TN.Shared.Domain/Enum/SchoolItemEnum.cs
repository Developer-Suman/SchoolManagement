using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public class SchoolItemEnum
    {
        public enum ItemStatus
        {
            Available = 1,
            Damaged = 2,
            Replaced = 3,
            Lost = 4,
            Disposed = 5
        }

        public enum ItemCondition
        {
            New = 1,
            Good = 2,
            Fair = 3,
            Poor = 4
        }

        public enum UnitType
        {
            // 1. Countable Items (pieces)
            [Display(Name = "टुक्रा / Piece")]
            Pcs,

            [Display(Name = "सेट / Set")]
            Set,

            [Display(Name = "बाकस / Box")]
            Box,

            [Display(Name = "प्याकेट / Packet")]
            Packet,

            [Display(Name = "गाँठो / Bundle")]
            Bundle,

            [Display(Name = "दर्जन / Dozen")]
            Dozen,

            // 2. Weight-Based Items
            [Display(Name = "किलोग्राम / kg")]
            Kg,

            [Display(Name = "ग्राम / g")]
            Gram,

            [Display(Name = "मिलिग्राम / mg")]
            Mg,

            [Display(Name = "टन / Ton")]
            Ton,

            [Display(Name = "क्विन्टल / Quintal")]
            Quintal,

            // 3. Volume-Based Items
            [Display(Name = "लिटर / Litre")]
            Litre,

            [Display(Name = "मिलिलिटर / ml")]
            Ml,

            [Display(Name = "क्यान / Can")]
            Can,

            [Display(Name = "बोतल / Bottle")]
            Bottle,

            [Display(Name = "जार / Jar")]
            Jar,

            [Display(Name = "ड्रम / Drum")]
            Drum
        }

    }
}
