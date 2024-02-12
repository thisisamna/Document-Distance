using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Threading.Tasks;

namespace DocumentDistance
{
    class DocDistance
    {
        // *****************************************
        // DON'T CHANGE CLASS OR FUNCTION NAME
        // YOU CAN ADD FUNCTIONS IF YOU NEED TO
        // *****************************************
        /// <summary>
        /// Write an efficient algorithm to calculate the distance between two documents
        /// </summary>
        /// <param name="doc1FilePath">File path of 1st document</param>
        /// <param name="doc2FilePath">File path of 2nd document</param>
        /// <returns>The angle (in degree) between the 2 documents</returns>
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            // TODO comment the following line THEN fill your code here
            string[] doc = new string[2];

            doc[0] = File.ReadAllText(doc1FilePath);
            doc[1]= File.ReadAllText(doc2FilePath);

            Dictionary<string, int>[] vector = new Dictionary<string, int>[2];
            vector[0] = new Dictionary<string, int>();
            vector[1] = new Dictionary<string, int>();
            string word;
            Regex regex = new Regex("(A-Za-z0-9)*");
            for (int i = 0; i < 2; i++)
            {
                MatchCollection matches = regex.Matches(doc[i]);

                foreach (Match result in matches)
                {
                    word = result.ToString();
                    if(vector[i].ContainsKey(word))
                    {
                        vector[i][result.ToString()] += 1;
                    }
                    else
                    {
                        vector[i][result.ToString()] = 1;

                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
