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
        private static Dictionary<string, int[]> vectors;
        private static Semaphore parse = new Semaphore(0, 2);
        private static Mutex mutex = new Mutex();
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            // TODO comment the following line THEN fill your code here
            vectors = new Dictionary<string, int[]>();

            object params1= createParamObject(doc1FilePath, 0);
            object params2= createParamObject(doc2FilePath, 1);

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

        private static void parseDocument(object parameters)
        {
            //Parameters
            ArrayList parameter = (ArrayList)parameters;
            string path = (string)parameter[0];
            int docNo = (int)parameter[1];


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
                    mutex.WaitOne();
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
                    mutex.ReleaseMutex();
                    word = "";

                }
            }
            if (word.Length > 0)
            {
                word = word.ToLower();
                mutex.WaitOne();

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
                mutex.ReleaseMutex();

            }
            parse.Release();
        }
        private static ArrayList createParamObject(string path, int docNo)
        {
            ArrayList parameters = new ArrayList();
            parameters.Add(path);
            parameters.Add(docNo);
            return parameters;
        }
    }
}
