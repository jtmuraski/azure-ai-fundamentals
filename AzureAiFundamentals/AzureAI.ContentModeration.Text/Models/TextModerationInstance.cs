using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureAI.ContentModeration.Text.Models;
using AzureAI.ContentModeration.Text.Utils;

namespace AzureAI.ContentModeration.Text.Models
{
    public class TextModerationInstance
    {
        // ---Properties---
        public string TextToModerate { get => textToModerate; set => textToModerate = value; }
        public List<string> BlockList { get => blockList; set => blockList = value; }
        public int? HateScore { get => hateScore; set => hateScore = value; }
        public int? SelfHarmScore { get => selfHarmScore; set => selfHarmScore = value; }
        public int? SexualScore { get => sexualScore; set => sexualScore = value; }
        public int? ViolenceScore { get => violenceScore; set => violenceScore = value; }

        public SeverityLevel HateSeverity 
        { 
            get 
            {
                if(hateScore.HasValue)
                    return SeverityCheck.CheckSeverity(hateScore.Value);
                else
                    return SeverityLevel.Low;
            }
        }

        public SeverityLevel SelfHarmSeverity
        {
            get
            {
                if (selfHarmScore.HasValue)
                    return SeverityCheck.CheckSeverity(selfHarmScore.Value);
                else
                    return SeverityLevel.Low;
            }
        }

        public SeverityLevel SexualSeverity
        {
            get
            {
                if (sexualScore.HasValue)
                    return SeverityCheck.CheckSeverity(sexualScore.Value);
                else
                    return SeverityLevel.Low;
            }
        }

        public SeverityLevel ViolenceSeverity
        {
            get
            {
                if (violenceScore.HasValue)
                    return SeverityCheck.CheckSeverity(violenceScore.Value);
                else
                    return SeverityLevel.Low;
            }
        }   

        // ---Fields---
        private string textToModerate;
        private List<string> blockList;
        private int? hateScore;
        private int? selfHarmScore;
        private int? sexualScore;
        private int? violenceScore;

        // ---Constructors---
        public TextModerationInstance()
        {
            textToModerate = string.Empty;
            blockList = new List<string>();
            hateScore = 0;
            selfHarmScore = 0;
            sexualScore = 0;
            violenceScore = 0;
        }



    }
}
