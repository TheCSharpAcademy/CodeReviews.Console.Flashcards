using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlashCards
{
    internal class StudySession
    {
        public string StackName { get; set; }
        public int StackId { get; set; }
        public DateTime SessionDate { get; set; }
        public int Score { get; set; }
    }

}
