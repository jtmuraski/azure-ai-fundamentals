using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAiFundamentals.Core.Models
{
    public class TextModerationResponse
    {
        public string? AnalyzedText { get; set; }

        public int HateSeverityScore { get; set; }
        public int ViolenceSeverityScore { get; set; }
        public int SelfHarmSeverityScore { get; set; }
        public int SexualSeverityScore { get; set; }

        public bool RequiresReview =>
            HateSeverityScore > 4 ||
            ViolenceSeverityScore > 4 ||
            SelfHarmSeverityScore > 4 ||
            SexualSeverityScore > 4;

        public string RiskLevel
        {
            get
            {
                var maxSeverity = new[]
                {
                    HateSeverityScore,
                    ViolenceSeverityScore,
                    SelfHarmSeverityScore,
                    SexualSeverityScore
                }.Max();

                return maxSeverity switch
                {
                    <= 0 => "Low",
                    <= 4 => "Medium",
                    _ => "High"
                };
            }
        }
    }
}
