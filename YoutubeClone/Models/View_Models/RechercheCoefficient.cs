using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace YoutubeClone.Models.View_Models
{
    public class RechercheCoefficient
    {
        //Search parameters
        public int id;
        public string searchQuery;
        public string itemName;
        public string itemDescription;
        public string itemTags;

        //Total coefficient
        public double coefficient = 0.001;
        //Amount of points for each matches as tiebreaker/troubleshooting help
        public double MatchesName = 0;
        public double MatchesDescription = 0;
        public double MatchesTags = 0;
        //Variables to tune the algorythm
        private double coefficientName = 2.5;
        private double coefficientDescription = 1.25;
        private double coefficientTags = 1.0;
        private string[] keywords;

        public RechercheCoefficient(int id) {
            this.id = id;
        }

        public RechercheCoefficient(string searchQuery, string itemName, string itemDescription, string itemTags, int id)
        {
            this.searchQuery = searchQuery.ToLower();
            this.itemName = itemName.ToLower();
            this.itemDescription = itemDescription.ToLower();
            this.itemTags = itemTags;
            this.id = id;

            keywords = searchQuery.Split(' ');

            foreach (string keyword in keywords)
            {
                MatchesName += CountPoints(itemName, keyword);
                if (itemTags != null)
                    MatchesTags += CountPoints(itemTags.ToLower(), keyword);
                MatchesDescription += CountPoints(itemDescription, keyword);
            }

            coefficient = MatchesDescription * coefficientDescription + MatchesName * coefficientName + MatchesTags * coefficientTags;
        }

        private static double CountPoints(string itemText, string keyword)
        {
            // find every occurences of keyword
            double points = 0;
            int i = 0;
            //Points per keyword size
            double pointSmall = 1;
            double pointMedium = 1.5;
            double pointLarge = 2.0;
            //Defined sizes
            int sizeSmall = 3;
            int sizeLarge = 8;

            while ((i = itemText.IndexOf(keyword, i)) != -1)
            {
                i += keyword.Length;
                //apply point according to keyword size
                if (keyword.Length <= sizeSmall)
                    points += pointSmall;
                else if (keyword.Length <= sizeLarge)
                    points += pointMedium;
                else
                    points += pointLarge;
            }
            return points;
        }
    }
}