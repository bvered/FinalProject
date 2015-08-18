using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace WebServer.App_Data.Models
{
    [DataContract(IsReference = true)]  
    public class AverageRatings : IPersistent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public IList<int> AverageRatingsList { get; set; }
        [DataMember]
        public IList<int> SumOfRatingsList { get; set; }
        [DataMember]
        public int Counter { get; set; }

        public AverageRatings() { }

        public AverageRatings(int numberOfRatings) {
            AverageRatingsList = new List<int>();
            SumOfRatingsList = new List<int>();
            for (var i = 0; i < numberOfRatings; i++) {
                AverageRatingsList.Add(0);
                SumOfRatingsList.Add(0);
            }
        }

        public void AddRatings(List<int> ratings) {
            Counter++;
            for (var i = 0; i < ratings.Count; i++) {
                SumOfRatingsList[i] += ratings[i];
                AverageRatingsList[i] = SumOfRatingsList[i] / Counter;
            }
        }

        public int GetAverageOfRatings()
        {
            return AverageRatingsList.Sum() / AverageRatingsList.Count / Counter;
        }
    }
}