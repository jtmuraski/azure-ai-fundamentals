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
        public int? HateScore { get => hateScore; set => hateScore = value; }
        public int? SelfHarmScore { get => selfHarmScore; set => selfHarmScore = value; }
        public int? SexualScore { get => sexualScore; set => sexualScore = value; }
        public int? ViolenceScore { get => violenceScore; set => violenceScore = value; }

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
