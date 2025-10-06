using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureAI.ContentModeration.Text.Models;

namespace AzureAI.ContentModeration.Text.Utils
{
    public static class SeverityCheck
    {
        public static SeverityLevel CheckSeverity(int score)
        {
            if(score <= 2)
                return SeverityLevel.Low;
            else if(score <= 4)
                return SeverityLevel.Medium;
            else if(score <= 6)
                return SeverityLevel.High;
            else
                return SeverityLevel.Severe;
        }
    }
}
