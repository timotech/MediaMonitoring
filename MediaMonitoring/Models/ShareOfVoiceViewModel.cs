using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ShareOfVoiceViewModel
    {
        public IEnumerable<ShareOfExpenditure> ShareOfExpenditures { get; set; }
        public IEnumerable<ShareOfVoice> ShareOfVoices { get; set; } 
        public IEnumerable<ShareOfCompetitiveWatchDog> ShareOfCompetitiveWatchDogs { get; set; }
        public IEnumerable<ShareOfCompetitiveWatchDogPress> ShareOfCompetitiveWatchDogsPress { get; set; }
        public IEnumerable<ShareOfCompetitiveWatchDogOutdoor> ShareOfCompetitiveWatchDogsOutdoor { get; set; }
        public IEnumerable<ShareOfExpeditureAllMedia> ShareOfExpeditureAllMedias { get; set; }
        public IEnumerable<ShareOfVoiceAllMedia> ShareOfVoiceAllMedias { get; set; }
    }
}
