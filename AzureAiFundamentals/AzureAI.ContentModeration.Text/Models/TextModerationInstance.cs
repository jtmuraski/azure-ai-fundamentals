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
        public int HateScore { get => hateScore; set => hateScore = value; }
        public int SelfHarmScore { get => selfHarmScore; set => selfHarmScore = value; }
        public int SexualScore { get => sexualScore; set => sexualScore = value; }
        public int ViolenceScore { get => violenceScore; set => violenceScore = value; }

        public SeverityLevel HateSeverity => SeverityCheck.CheckSeverity(hateScore);
        public SeverityLevel SelfHarmSeverity => SeverityCheck.CheckSeverity(selfHarmScore);
        public SeverityLevel SexualSeverity => SeverityCheck.CheckSeverity(sexualScore);
        public SeverityLevel ViolenceSeverity => SeverityCheck.CheckSeverity(violenceScore);

        // ---Fields---
        private string textToModerate;
        private List<string> blockList;
        private int hateScore = 0;
        private int selfHarmScore = 0;
        private int sexualScore = 0;
        private int violenceScore = 0;

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
