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
            string[]
                doc = new string[2];

            doc[0] = File.ReadAllText(doc1FilePath);
            doc[1]= File.ReadAllText(doc2FilePath);

            Dictionary<string, int[]> vectors = new Dictionary<string, int[]>();

            string word; char letter;
            for (int d = 0; d < 2; d++)
            {
                int length = doc[d].Length;
                word = "";

                for (int i = 0; i < length; i++)
                {
                    letter = doc[d][i];
                    if (Char.IsLetterOrDigit(letter))
                    {
                        word += letter;
                    }
                    else if (word.Length>0)
                    {
                        word = word.ToLower();
                         
                        if (vectors.ContainsKey(word))
                        {
                            vectors[word][d] += 1;
                        }
                        else
                        {
                            vectors[word] = new int[2];
                            vectors[word][d] = 1;
                            //vectors[word][i+1%2] = 0;


                        }
                        word = "";

                    }
                }
               if (word.Length > 0)
                {
                    word = word.ToLower();

                    if (vectors.ContainsKey(word))
                    {
                        vectors[word][d] += 1;
                    }
                    else
                    {
                        vectors[word] = new int[2];
                        vectors[word][d] = 1;
                        //vectors[word][i+1%2] = 0;


                    }
                }

                }

            //foreach(KeyValuePair<string, int[]> element in vectors)
            //{
            //    Console.WriteLine(element.Key + "\t" + element.Value[0] + "\t" + element.Value[1]);
            //}

            double D1xD2 = 0;
            double D1, D2;

            double D1Squared = 0;
            double D2Squared = 0;
            foreach(string key in vectors.Keys)
            {
                D1 = vectors[key][0];
                D2 = vectors[key][1]; 
                D1xD2 += D1 * D2; //not squared yet
                D1Squared += D1 * D1; ;
                D2Squared += D2 * D2;
            }
            double quotient =  D1xD2 / (Math.Sqrt(D1Squared * D2Squared));
            return Math.Acos(quotient) * 180 / Math.PI;
        }
    }
}
