using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_1
{
    class Program
    {
        static void RecursionCombinations(List<string> FinishList, string[] Players, ref string[] PlayersBuffer, int order)
        {
            if (order < PlayersBuffer.Length)
                for (int i = 0; i < Players.Length; i++)
                {
                    if (order == 0)
                    {
                        PlayersBuffer[order] = Players[i];
                        RecursionCombinations(FinishList, Players, ref PlayersBuffer, order + 1);
                    }
                    else if (!PlayersBuffer.Contains(Players[i]) & Players[i] != null)
                    {
                        PlayersBuffer[order] = Players[i];
                        RecursionCombinations(FinishList, Players, ref PlayersBuffer, order + 1);
                    }
                }
            else
            {
                if (!PlayersBuffer.Contains(null))
                {
                    int TeamNumber = 1;
                    string TempString = "";
                    for (int i = 0; i < PlayersBuffer.Length; i++)
                    {
                        TempString += $"T{TeamNumber}: {PlayersBuffer[i]}";
                        TempString += string.Format(" ", 2);
                        TeamNumber++;
                    }
                    if (!FinishList.Contains(TempString))
                    {
                        FinishList.Add(TempString);
                    }
                }
                for (int i = PlayersBuffer.Length - 2; i < PlayersBuffer.Length; i++)
                {
                    PlayersBuffer[i] = null;
                }
            }
        }
        
        public static void PlayersIntoTeams(int PlayersNumber, int ColumnsNumber)
        {
            List<string[]> BaseCombinations = new List<string[]>();
            List<string> FinishList = new List<string>();

            string[] Players = new string[PlayersNumber];

            for(int i = 0; i<PlayersNumber;i++)
            {
                Players[i] = (i+1).ToString();
            }

            int max = Players.Length - (ColumnsNumber - 1);
            bool Double = false;

            while (max >= 2)
            {
                if (max == 2) Double = true;
                for (int i = 0; i < Players.Length; i++)
                {
                    FirstElementRecursive(BaseCombinations, Players, max, ColumnsNumber, i, i + 1, Double);
                }
                max--;
            }

            string[] Buffer = new string[ColumnsNumber];

            foreach(string[] SA in BaseCombinations)
            {
                RecursionCombinations(FinishList, SA, ref Buffer, 0);
            }

            foreach(string s in FinishList)
            {
                Console.WriteLine(s);
            }
        }

        public static void OtherElements(List<string[]> BaseCombinations, string CurrentElement, string[] RemainTemp, int MaxSymbols, int Index, int ColumnsNumber, int CurrentColumnNumber, bool DoubleVarian,  string[] ResultElements = null)
        {
            if (CurrentColumnNumber >= ColumnsNumber) CurrentColumnNumber = ColumnsNumber - 1;

            string[] ResultTemp = new string[ColumnsNumber];
            
            if (ResultElements == null)
            {
                ResultElements = new string[ColumnsNumber];
                Array.Copy(ResultElements, ResultTemp, ColumnsNumber);
            }
            else
            {
                Array.Copy(ResultElements, ResultTemp, ColumnsNumber);
            }

            if (ResultTemp[ResultTemp.Length - 1] != null)
            {
                if(IsFull(ResultTemp, RemainTemp.Length))
                {
                    BaseCombinations.Add(ResultTemp);
                    return;
                }
            }

            string TempString = CurrentElement;

            if(MaxSymbols > 0)
            {
                for (int i = Index; i < RemainTemp.Length; i++)
                {
                    if (RemainTemp[i] != null)
                    {
                        CurrentElement = TempString + RemainTemp[i];
                        string Temp = RemainTemp[i];
                        RemainTemp[i] = null;
                        MaxSymbols--;

                        if (i == RemainTemp.Length - 1 & MaxSymbols > 0)
                        {
                            OtherElements(BaseCombinations, CurrentElement, RemainTemp, MaxSymbols, 0, ColumnsNumber, CurrentColumnNumber, DoubleVarian, ResultTemp);
                        }

                        else if (MaxSymbols > 0)
                        {
                            OtherElements(BaseCombinations, CurrentElement, RemainTemp, MaxSymbols, Index+1, ColumnsNumber, CurrentColumnNumber, DoubleVarian, ResultTemp);
                        }
                        else
                        {
                            ResultTemp[CurrentColumnNumber] = CurrentElement;
                            int Count = PlayersCount(RemainTemp);
                            if (DoubleVarian)
                            {
                                MaxSymbols = DoubleVariants(Count);
                            }
                            else
                            {
                                MaxSymbols = Variants(Count, (ColumnsNumber - (CurrentColumnNumber + 1)));
                            }
                            OtherElements(BaseCombinations, "", RemainTemp, MaxSymbols, 0, ColumnsNumber, CurrentColumnNumber + 1, DoubleVarian, ResultTemp);
                        }
                        RemainTemp[i] = Temp;
                    }
                }
            }
            else
            {
                ResultTemp[CurrentColumnNumber] = CurrentElement;
                int Count1 = PlayersCount(RemainTemp);
                if (DoubleVarian)
                {
                    MaxSymbols = DoubleVariants(Count1);
                }
                else
                {
                    MaxSymbols = Variants(Count1, (ColumnsNumber - (CurrentColumnNumber + 1)));
                }
                OtherElements(BaseCombinations, "", RemainTemp, MaxSymbols, 0, ColumnsNumber, CurrentColumnNumber + 1, DoubleVarian, ResultTemp);
            }
        }

        public static int Variants(int RemainPlayers, int RemainColomns)
        {
            int temp = 0;

            if(RemainPlayers == RemainColomns)
            {
                temp = 1;
            }

            temp = (RemainPlayers - RemainColomns);

            if(temp < 2 & RemainColomns != 0)
            {
                temp += 1;
            }

            return temp;
        }

        public static int DoubleVariants(int RemainPlayers)
        {
            if(RemainPlayers >= 2)
            {
                return 2;
            }
            return 1;
        }

        public static bool IsFull(string[] Array, int MaxCount)
        {
            int temp = 0;
            for(int i = 0; i< Array.Length;i++)
            {
                temp += Array[i].Length;
            }
            return (MaxCount == temp);
        }

        public static int PlayersCount(string[] Array)
        {
            int Count1 = 0;
            for (int j = 0; j < Array.Length; j++)
            {
                if (Array[j] != null)
                {
                    Count1++;
                }
            }
            return Count1;
        }

        public static void FirstElementRecursive(List<string[]> BaseCombinations, string[] Players, int Max, int ColumnsNumber, int FirstIndex, int SecondIndex, bool DoubleVariants)
        {
            if (SecondIndex > Players.Length) return;

            string[] TempArray = new string[Players.Length];
            Array.Copy(Players, TempArray, Players.Length);

            string TempString = TempArray[FirstIndex];

            TempArray[FirstIndex] = null;

            string CurrentElement = "";

            int RemainSymbols = Max - 1;

            for (int i = SecondIndex; i < TempArray.Length; i++)
            {
                CurrentElement = TempString + TempArray[i];
                string Temp = TempArray[i];
                TempArray[i] = null;
                RemainSymbols--;
                OtherElements(BaseCombinations, CurrentElement, TempArray, RemainSymbols, i + 1, ColumnsNumber, 0, DoubleVariants);
                TempArray[i] = Temp;
                RemainSymbols++;
            }
        }

        static void Main(string[] args)
        {
            PlayersIntoTeams(6, 4);
            
            Console.ReadLine();
        }
    }
}
