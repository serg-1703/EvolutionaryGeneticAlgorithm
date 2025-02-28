using System.Collections.Generic;

public class MainProgram
{
    static public int Max(int x, int y)
    {
        return x > y ? x : y;
    }


    static public List<List<int>> MakeParents(int count, int appCount)
    {
        List<List<int>> parents = new List<List<int>>(); 
        Random rnd = new Random();
        

        for (int i = 0; i < count; i++)
        {
            parents.Add(new List<int>());
            for (int j = 0; j < appCount; j++)
            {
                parents[i].Add(j + 1);
            }
        }
       

        for (int i = 0; i < count; i++)
        {
            
            for (int j = 0; j < parents[i].Count(); j++)
            {
                int ind = rnd.Next(0, appCount);
                (parents[i][j], parents[i][ind]) = (parents[i][ind], parents[i][j]) ;
            }
 
        }

        return parents;
    }

    static public List<int> Mutation(List<int> per)
    {
        Random rnd = new Random();
        List<int> a = new List<int>();
        a = per;
        int ind = rnd.Next(0, a.Count() - 1);
        (a[ind], a[ind + 1]) = (a[ind + 1], a[ind]);

        return a;
    }

    static  public List<List<int>> Otbor(int deviceCount, int appCount, List<List<int>> children, int parentsCount, List<List<int>> T, List<int> D)
    {
        List<List<int>> b = new List<List<int>>();
        List<List<int>> a = new List<List<int>>();
        a = children;


        for (int i = 0; i < parentsCount; ++i)
        {
            int curIndex = -1;
            int max = int.MaxValue;
            for(int j = 0; j < a.Count(); j++)
            {
                int res = Result(deviceCount, appCount, a[j], T, D);
                if (res < max) {max = res; curIndex = j;}
            }
            b.Add(Mutation(a[curIndex]));
            a.RemoveAt(curIndex);
        }
        return b;
    }

    static public void TheBest(int deviceCount, int appCount, List<List<int>> children, int parentsCount, List<List<int>> T, List<int> D)
    {
        
        int curIndex = -1;
        int max = int.MaxValue;
        int res = 0;
        for (int j = 0; j < parentsCount; j++)
        {
            res = Result(deviceCount, appCount, children[j], T, D);
            if (res < max) { max = res; curIndex = j; }
        }
        Console.WriteLine("\n||_Лучшее решение_||\n");
        Console.Write(string.Join(", ", children[curIndex]));
        Console.WriteLine(" -- " + max);


    }
    static public(List<int> c1, List<int> c2) CrossOX(List<int> a, List<int> b)
    {
        List<int> c1 = new List<int>();
        List<int> c2 = new List<int>();
        for(int i = 0; i < a.Count(); i++)
        {
            c1.Add(0);
            c2.Add(0);
        }

        int l = 5; int r = 9;
        for (int i = l; i < r; i++)
        {
            c1[i] = a[i];
            c2[i] = b[i];
        }
        
        for (int i = 0; i < a.Count(); i++)
        {
            if (c1[i] == 0)
            {
                for (int j = 0; j < b.Count(); j++)
                {
                    if (!c1.Contains(b[j]))
                    {
                        c1[i] = b[j];
                        break;
                    }
                }
            }

            if (c2[i] == 0)
            {
                for (int j = 0; j < a.Count(); j++)
                {
                    if (!c2.Contains(a[j]))
                    {
                        c2[i] = a[j];
                        break;
                    }
                    
                }
            }

        }

        return (c1, c2);
    }
   
    static void Main()
    {
        List<List<int>> parents = new List<List<int>>();

        string path = @"C:\Users\User\source\repos\EGAlab\EGAlab\data.txt";
        List<int> perestanovka = new List<int>() {2, 4, 3, 5, 6, 7, 1, 9, 8};
        int deviceCount;
        int appCount;
        List<int> D = new List<int>();
        List<List<int>> T = new List<List<int>>();
        int fine = 0;

       

        using (StreamReader reader = new StreamReader(path))
        {
            List<string> data = new List<string>();
            string line;
            line = reader.ReadLine();

            deviceCount = int.Parse(line.Split().ToList()[0]);
            appCount = int.Parse(line.Split().ToList()[1]);
            
            for (int i = 0; i < deviceCount; i++)
            {
                line = reader.ReadLine();
                data = line.Split().ToList();
                T.Add(new List<int>());
                for (int j = 0; j < appCount; j++)
                {
                    T[i].Add(int.Parse(data[j]));
                   
                }

            }
            line = reader.ReadLine();
            data = line.Split().ToList();
            for(int i = 0;i < appCount; i++)
                D.Add(int.Parse(data[i]));
        }

        Console.Write("Введите кол-во особей в нач. популяции: ");
        int parentsCount = Convert.ToInt32(Console.ReadLine());
        parents = MakeParents(parentsCount, appCount);
        
        Console.Write("Введите кол-во поколений: ");
        int age = Convert.ToInt32(Console.ReadLine());

        
        List < List<int> > children = new List<List<int>>();

        for (int i = 0; i < age; i++)
        {
            Console.WriteLine("\n << Поколение: " + (i + 1) + " >>\n");
            Console.Write("Начальная популяция:");
            WriteParents(parents, deviceCount, appCount, T, D);
            List<int> child1 = new List<int>();
            List<int> child2 = new List<int>();
            for (int j = 0; j < parentsCount - 1; j++)
            {
                for (int k = j+1; k < parentsCount; k++)
                {
                    (child1, child2) = CrossOX(parents[j], parents[k]);
                    children.Add(child1);
                    children.Add(child2);
                    
                }
            }
            parents = Otbor(deviceCount, appCount, children, parentsCount, T, D);
            children.Clear();
            Console.Write("Конечная популяция:");
            WriteParents(parents, deviceCount, appCount, T, D);

        }
        TheBest(deviceCount, appCount, parents, parentsCount, T, D);


    }

   

    static public void WriteParents(List<List<int>> parents, int dC, int aC, List<List<int>> T, List<int> D)
    {
        Console.WriteLine("____________");
        for (int i = 0; i < parents.Count(); i++)
        {
            Console.Write(string.Join(", ", parents[i]));
            Console.WriteLine(" -- " + Result(dC, aC, parents[i], T, D));
        }

    }
     

    static public int Result(int deviceCount, int appCount, List<int> perestanovka, List<List<int>> T, List<int> D)
    {
        int fine = 0;
        int[,] X = new int[appCount, deviceCount];
        int oi = 0;
        for (int u = 0; u < appCount; u++)
        {
            int i = perestanovka[u] - 1;
            for (int j = 0; j < deviceCount; j++)
            {
                if (u == 0)
                {
                    X[u, j] = j == 0 ? 0 : X[u, j - 1] + T[j - 1][i];
                }
                else
                {
                    if (j == 0) X[u, j] = X[u - 1, j] + T[j][oi];
                    else
                    {
                        if (X[u, j - 1] + T[j - 1][i] <= X[u - 1, j] + T[j][oi])
                        {
                            X[u, j] = X[u - 1, j] + T[j][oi];
                        }
                        else
                        {
                            X[u, j] = X[u, j - 1] + T[j - 1][i];
                        }
                    }
                }
            }
            oi = perestanovka[u] - 1;
            fine += Max(0, X[u, deviceCount - 1] + T[deviceCount - 1][i] - D[i]);

        }
        return fine;

    }
}

