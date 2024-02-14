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
        private static Dictionary<string, int[]> vectors;
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            // TODO comment the following line THEN fill your code here
            vectors = new Dictionary<string, int[]>();


            parseDocument(doc1FilePath, 0, vectors);
            parseDocument(doc2FilePath, 1, vectors);



            //foreach(KeyValuePair<string, int[]> element in vectors)
            //{
            //    Console.WriteLine(element.Key + "\t" + element.Value[0] + "\t" + element.Value[1]);
            //}

            double D1xD2 = 0;
            double D1 = 0;
            double D2 = 0;

            double D1Squared = 0;
            double D2Squared = 0;
            foreach (string key in vectors.Keys)
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
            angle = angle * 180 / Math.PI;
            Console.WriteLine(angle);
            return angle;
        }

        public static void parseDocument(string path, int docNo, Dictionary<string, int[]> vectors)
        {
            string doc = File.ReadAllText(path);
            int length = doc.Length;
            string word = "";
            char letter;
            for (int i = 0; i < length; i++)
            {
                letter = doc[i];
                if (Char.IsLetterOrDigit(letter))
                {
                    word += letter;
                }
                else if (word.Length > 0)
                {
                    word = word.ToLower();

                    if (vectors.ContainsKey(word))
                    {
                        vectors[word][docNo] += 1;
                    }
                    else
                    {
                        vectors[word] = new int[2];
                        vectors[word][docNo] = 1;
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
                    vectors[word][docNo] += 1;
                }
                else
                {
                    vectors[word] = new int[2];
                    vectors[word][docNo] = 1;
                    //vectors[word][i+1%2] = 0;


                }
            }
        }
    }
}
