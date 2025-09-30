using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.ContentModeration.Text.Models
{
    public class TextModerationInstance
    {
        // ---Properties---
        public string TextToModerate { get => textToModerate; set => textToModerate = value; }
        public List<string> BlockList { get => blockList; set => blockList = value; }
        public double HateScore { get => hateScore; set => hateScore = value; }
        public double SelfHarmScore { get => selfHarmScore; set => selfHarmScore = value; }
        public double SexualScore { get => sexualScore; set => sexualScore = value; }
        public double ViolenceScore { get => violenceScore; set => violenceScore = value; }

        // ---Fields---
        private string textToModerate;
        private List<string> blockList;
        private double hateScore;
        private double selfHarmScore;
        private double sexualScore;
        private double violenceScore;

        // ---Constructors---
        public TextModerationInstance()
        {
            textToModerate = string.Empty;
            blockList = new List<string>();
            hateScore = 0.0;
            selfHarmScore = 0.0;
            sexualScore = 0.0;
            violenceScore = 0.0;
        }



    }
}
