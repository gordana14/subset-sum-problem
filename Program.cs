using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PracticalSession2
{
    class Program
    {
        static void generateInstances()
        {
            string res = "";
            Random rnd = new Random();
            int n = rnd.Next(200);
            int k = rnd.Next(200, 3000);
            res += n.ToString() + Environment.NewLine + k.ToString() + Environment.NewLine;
            for (int i = 0; i < n; i++)
            {
                int ai = rnd.Next(200);
                res += ai.ToString() + Environment.NewLine;
            }


            using (StreamWriter writer = new StreamWriter("C:\\Users\\Gordana\\source\\repos\\PracticalSession2\\file.txt"))
            {
                writer.WriteLine(res);
            }
        }

        static List<string> readInstances()
        {

            //
            // Read in a file line-by-line, and store it all in a List.
            //
            List<string> list = new List<string>();
            using (System.IO.StreamReader reader = new StreamReader("C:\\Users\\Gordana\\source\\repos\\PracticalSession2\\file.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to list.
                    Console.WriteLine(line); // Write to console.
                }
                reader.Close();
                reader.Dispose();
            }
            return list;
        }
        static float calculateAvgDistance(List<int> list)
        {
            var arr = list.ToArray();
            Array.Sort(arr);
            float avgDistance;
            int distance = 0;
            Console.WriteLine("Min value is " + arr[0].ToString());
            Console.WriteLine("Max value is " + arr[arr.Length - 1].ToString());
            for (int i = 1; i < arr.Length; i++)
            {
                distance += Math.Abs((arr[i - 1] - arr[i]));
            }
            return avgDistance = distance / (arr.Length - 1);
        }

        static void Main(string[] args)
        {

            int n = 0;
            int k = 0;
            List<int> lsA = null;
            //  generateInstances();
            List<string> list = readInstances();

            n = Int32.Parse(list[0]);
            k = Int32.Parse(list[1]);
            lsA = new List<int>(n);
            for (int i = 1; i <= n; i++)
            {

                lsA.Add(Int32.Parse(list[i + 1]));


            }

            Console.WriteLine("The avg distance is " + calculateAvgDistance(lsA).ToString());


            int resGreedy = Greedy.SubsetSumGreedy(lsA, k);

            Console.WriteLine("The result for Subset sum problem with Greedy method is: " + resGreedy.ToString());


            int resE = ExhastiveSearch.SubsetSumExS(lsA, k);

            Console.WriteLine("The result for Subset sum problem with exhastive search method is: " + resE.ToString());

            double[] epsilon = new double[] { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };

            for (int i = 0; i < epsilon.Length; i++)
            {
                int resFPTAS = FPTAS.SubsetSumFPTAS(lsA, k, 0.1);


            Console.WriteLine("The result for Subset sum problem with Full poly time aprox. scheme method is: " + resFPTAS.ToString() + " ");
            }
            Dynamic ds = new Dynamic();
            int res = ds.SubsetSum(lsA, k);

            Console.WriteLine("The result for Subset sum problem with dyn programming method is: " + res.ToString());
            Thread.Sleep(1000);
        }
    }

    class Greedy
    {
        public static int SubsetSumGreedy(IEnumerable<int> list, int s)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            var arr = list.ToArray();
            Array.Sort(arr);
            Array.Reverse(arr);
            int sum = 0;
            foreach (int x in arr)
            {
                if (sum + x <= s) sum += x;

            }

            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            return sum;
        }
    }

    class FPTAS
    {

        public static List<int> trim(List<int> lst, double sigma)
        {

            List<int> lsRes = new List<int> { };
            lsRes.Add(lst.ElementAt(0));
            int last = lst.ElementAt(0);
            for (int i = 1; i < lst.Count; i++)
            {
                if (last < (1 - sigma) * lst[i]) lsRes.Add(lst[i]);
                last = lst[i];
            }

            return lsRes;
        }

        public static int SubsetSumFPTAS(IEnumerable<int> list, int s, double sigma)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            var arr = list.ToArray();
            Array.Sort(arr);
            //auxilary array 
            List<int> aux = new List<int> { };

            List<int> res = new List<int> { };
            //initial array 
            res.Add(0);
            for (int i = 0; i < arr.Length; i++)
            {

                aux = res;
                aux = aux.Select(x => x + arr[i]).ToList();
                res = res.Concat(aux).ToList();
                res.Sort();
                res = trim(res, sigma / (2 * arr.Length));
                List<int> lsRemoveAndFilter = res.Distinct().ToList().Where(m => m <= s).ToList();
                res = lsRemoveAndFilter;

            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            return res.Max();

        }
    }

}
class ExhastiveSearch
{
    public static int SubsetSumExS(IEnumerable<int> list, int s)
    {
        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();
        var arr = list.ToArray();
        Array.Sort(arr);
        //auxilary array 
        List<int> aux = new List<int> { };

        List<int> res = new List<int> { };
        //initial array 
        res.Add(0);
        for (int i = 0; i < arr.Length; i++)
        {

            aux = res;
            aux = aux.Select(x => x + arr[i]).ToList();
            res = res.Concat(aux).ToList();
            res.Sort();
            List<int> lsRemoveAndFilter = res.Distinct().ToList().Where(m => m <= s).ToList();
            res = lsRemoveAndFilter;

        }
        watch.Stop();

        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

        return res.Max();

    }

}

class Dynamic
{
    public int[,] copyRows(int[,] arrItems, int s)
    {

        for (int i = 0; i < s + 1; i++)
        {

            arrItems[0, i] = arrItems[1, i];

        }
        return arrItems;
    }

    public int SubsetSum(IEnumerable<int> list, int s)
    {
        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();
        var arr = list.ToArray();
        Array.Sort(arr);
        //matrix for final result
        int[,] lsAuxDict = new int[2, s + 1];
        //first column  from final matrix 
        for (int i = 0; i < 2; i++)
        {
            lsAuxDict[i, 0] = 0;

        }

        //First row from final matrix 
        for (int i = 0; i < s + 1; i++)
        {

            if (i < arr[0]) lsAuxDict[0, i] = 0;
            else lsAuxDict[0, i] = arr[0];
        }
          // Fill the subset table in bottom up manner

        for (int i = 1; i < arr.Length; i++)
        {
            for (int j = 1; j < s + 1; j++)
            {
                if (j - arr[i] <= 0) { lsAuxDict[1, j] = lsAuxDict[0, j]; }
                else { lsAuxDict[1, j] = Math.Max(lsAuxDict[0, j], lsAuxDict[0, j - arr[i]] + arr[i]); }
            }

            lsAuxDict = copyRows(lsAuxDict, s);

       }

        watch.Stop();

        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        return lsAuxDict[1, s];
    }




}
public class DynamicHash
{


    Dictionary<Key, int> lsAuxDict = new Dictionary<Key, int>();

    public class Key
    {
        public int Dimension1;
        public int Dimension2;
        public Key(int p1, int p2)
        {
            Dimension1 = p1;
            Dimension2 = p2;
        }
        // Equals and GetHashCode ommitted
    }
    public bool ExistDict(Key k)
    {
        var s = lsAuxDict.Where(t => t.Key.Dimension1 == k.Dimension1 && t.Key.Dimension2 == k.Dimension2).Count();
        if (s > 0) return true;
        else return false;

    }

    public void updateRecusiveDict(Key k, int[] arr)
    {
        Key kLeft;
        Key kRight;
        int new_i;
        if (k.Dimension1 - 1 < 0) new_i = 0;
        else new_i = k.Dimension1 - 1;
        int current_j;
        if (k.Dimension2 < 0) current_j = 0;
        else current_j = k.Dimension2;
        int new_j;
        if (k.Dimension2 - arr[k.Dimension1] < 0)
        {
            new_j = 0;
            kLeft = new Key(new_i, current_j);
            if (!ExistDict(kLeft)) updateRecusiveDict(kLeft, arr);
            var value21 = lsAuxDict.Where(t => t.Key.Dimension1 == kLeft.Dimension1 && t.Key.Dimension2 == kLeft.Dimension2).Select(t => t.Value).ToArray();
            lsAuxDict.Add(k, value21[0]);
        }
        else
        {
            new_j = k.Dimension2 - arr[k.Dimension1];
            kRight = new Key(new_i, new_j);
            if (!ExistDict(kRight)) updateRecusiveDict(kRight, arr);
            var value22 = lsAuxDict.Where(t => t.Key.Dimension1 == kRight.Dimension1 && t.Key.Dimension2 == kRight.Dimension2).Select(t => t.Value).ToArray();
            lsAuxDict.Add(k, value22[0] + arr[k.Dimension1]);
        }

    }




    public int SubsetSum(IEnumerable<int> list, int s)
    {
        var arr = list.ToArray();
        Array.Sort(arr);

        var dp = new double[arr.Length, s + 1];
        //matrix for final result
        int[] lsRes = new int[arr.Length];



        //first column  from final matrix 
        for (int i = 0; i < arr.Length; i++)
        {
            lsAuxDict.Add(new Key(i, 0), 0);
        }

        //First row from final matrix 
        for (int i = 0; i < s + 1; i++)
        {
            if (i < arr[0]) lsAuxDict.Add(new Key(0, i), 0);
            else lsAuxDict.Add(new Key(0, i), arr[0]);
        }
        lsRes[0] = arr[0];
        // Fill the subset table in bottom up manner
        for (int i = 1; i < arr.Length; i++)
        {
            int new_i;
            if (i - 1 < 0) new_i = 0;
            else new_i = i - 1;
            int new_j;
            if (s - arr[i] < 0) new_j = 0;
            else new_j = s - arr[i];
            Key k1 = new Key(new_i, new_j);

            if (!ExistDict(k1)) updateRecusiveDict(k1, arr);
            var l = lsAuxDict.Where(t => t.Key.Dimension1 == k1.Dimension1 && t.Key.Dimension2 == k1.Dimension2).Select(t => t.Value).ToArray();

            lsRes[i] = Math.Max(lsRes[i - 1], l[0] + arr[i]);

        }
        return lsRes.Max();
    }

}

