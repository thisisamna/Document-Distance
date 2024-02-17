using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private static Dictionary<string, int> vector1;
        private static Dictionary<string, int> vector2;

        private static Semaphore parse = new Semaphore(0, 2);
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            // TODO comment the following line THEN fill your code here
            vector1 = new Dictionary<string, int>();
            vector2 = new Dictionary<string, int>();

            object params1 = createParamObject(doc1FilePath, vector1);
            object params2= createParamObject(doc2FilePath, vector2);

            Thread thread1 = new Thread(new ParameterizedThreadStart(parseDocument));
            Thread thread2 = new Thread(new ParameterizedThreadStart(parseDocument));
            thread1.Start(params1);
            thread2.Start(params2);
            parse.WaitOne();
            parse.WaitOne();

            //parseDocument(createParamObject(doc1FilePath, 0, vectors));
            //parseDocument(createParamObject(doc2FilePath, 1, vectors));



            //foreach(KeyValuePair<string, int[]> element in vectors)
            //{
            //    Console.WriteLine(element.Key + "\t" + element.Value[0] + "\t" + element.Value[1]);
            //}

            double D1xD2 = 0;
            double D1 = 0;
            double D2 = 0;

            double D1Squared = 0;
            double D2Squared = 0;
            foreach (string key in vector1.Keys)
            {
                D1 = vector1[key];
                if (vector2.ContainsKey(key))
                    D2 = vector2[key];
                else
                    D2 = 0;
                D1xD2 += D1 * D2; //not squared yet
                D1Squared += D1 * D1; ;
                D2Squared += D2 * D2;
            }
            foreach (string key in vector2.Keys.Except(vector1.Keys))
            {
                D1 = 0;
                D2 = vector2[key];
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

        private static void parseDocument(object parameters)
        {
            //Parameters
            ArrayList parameter = (ArrayList)parameters;
            string path = (string)parameter[0];
            Dictionary<string, int> vector = (Dictionary<string, int>)parameter[1];


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
                    if (vector.ContainsKey(word))
                    {
                        vector[word] += 1;
                    }
                    else
                    {
                        vector[word] = 1;
                        //vector[word][i+1%2] = 0;


                    }
                    word = "";

                }
            }
            if (word.Length > 0)
            {
                word = word.ToLower();

                if (vector.ContainsKey(word))
                {
                    vector[word] += 1;
                }
                else
                {
                    vector[word] = 1;
                    //vector[word][i+1%2] = 0;

                }
            }
            parse.Release();
        }
        private static ArrayList createParamObject(string path, Dictionary<string, int> vector)
        {
            ArrayList parameters = new ArrayList();
            parameters.Add(path);
            parameters.Add(vector);

            return parameters;
        }
    }
}
