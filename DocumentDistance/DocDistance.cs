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

            Dictionary<string, int[]> vectors = new Dictionary<string, int[]>();
            string word;
            Regex regex = new Regex("([A-Z]|[a-z]|[0-9])+");
            for (int i = 0; i < 2; i++)
            {
                MatchCollection matches = regex.Matches(doc[i]);

                foreach (Match result in matches)
                {
                    word = result.Value.ToLower();
                   
                    if(vectors.ContainsKey(word))
                    {
                        vectors[word][i] += 1;
                    }
                    else
                    {
                        vectors[word] = new int[2];
                        vectors[word][i] = 1;
                        //vectors[word][i+1%2] = 0;


                    }
                }
            }

            //foreach(KeyValuePair<string, int[]> element in vectors)
            //{
            //    Console.WriteLine(element.Key + "\t" + element.Value[0] + "\t" + element.Value[1]);
            //}

            int D1xD2 = 0;
            int D1 = 0;
            int D2 = 0;

            int D1Squared = 0;
            int D2Squared = 0;
            foreach(string key in vectors.Keys)
            {
                D1 = vectors[key][0];
                D2 = vectors[key][1]; 
                D1xD2 += D1 * D2; //not squared yet
                D1Squared += D1 * D1; ;
                D2Squared += D2 * D2;
            }
            double productOfSquares = D1Squared * D2Squared;
            double quotient = D1xD2 / (Math.Sqrt(productOfSquares));
            double angle = Math.Acos(quotient);
            Console.WriteLine(angle);
            return angle;
        }
    }
}
