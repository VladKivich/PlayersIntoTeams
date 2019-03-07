using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2_1
{
    public class Element : IEquatable<Element>
    {
        List<int[]> Cells;

        int Count { get; }
        int Columns { get; }

        public Element(int Players, int Columns)
        {
            Cells = new List<int[]>(Columns);
            for (int i = 0; i < Columns; i++)
            {
                Cells.Add(new int[Players]);
            }
            Count = Columns;
            this.Columns = Columns;
        }

        /// <summary>
        /// Метод заполнения ячеек элемента.
        /// </summary>
        /// <param name="Numbers">Массив с данными</param>
        /// <param name="Sequence">Массив с последовательностью</param>
        public void FillCells(int[] Numbers, int[] Sequence)
        {
            int temp = 0;
            for (int i = 0; i < Cells.Count; i++)
            {
                if (temp < Numbers.Length)
                {
                    Array.Copy(Numbers, temp, Cells[i], 0, Sequence[i]);
                    temp += Sequence[i];
                }
            }
        }

        private int CellAmount(int[] Array)
        {
            int temp = 0;

            for (int i = 0; i < Array.Length; i++)
            {
                temp += Array[i];
            }
            return temp;
        }

        /// <summary>
        /// Метод выводит сумму ячеек Элемента, в виде строки.
        /// </summary>
        /// <returns></returns>
        public string Amount()
        {
            string Amount = "";
            foreach (int[] Array in Cells)
            {
                Amount += CellAmount(Array).ToString();
            }
            return Amount;
        }

        public bool Equals(Element other)
        {
            if (this.Amount() == other.Amount())
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// Метод вывода в консоль
        /// </summary>
        public void ToScreen()
        {
            int TeamNumber = 1;
            for (int i = 0; i < Cells.Count; i++)
            {
                Console.Write($"T{TeamNumber}: ");
                for (int k = 0; k < Cells[i].Length; k++)
                {
                    if(Cells[i][k] != 0)
                    {
                        Console.Write($"{Cells[i][k]}");
                    }
                }
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Преобразуем массив типа Int32 в массив типа String
        /// </summary>
        /// <returns></returns>
        public string[] ToStringArray()
        {
            string[] TempArray = new string[Columns];
            string TempString = "";

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int k = 0; k < Cells[i].Length; k++)
                {
                    if (Cells[i][k] != 0)
                    {
                        TempString += Cells[i][k];
                    }
                }
                TempArray[i] = TempString;
                TempString = "";
            }
            return TempArray;
        }

        public int[] this[int index]
        {
            get
            {
                return Cells[index];
            }

            set
            {
                Cells[index] = value;
            }
        }

        
    }
    class Program
    {
        /// <summary>
        /// Метод для рассчета суммы элементов массива.
        /// </summary>
        /// <param name="Array">Массив с данными</param>
        /// <param name="Target">Цель рассчета</param>
        /// <returns></returns>
        public static bool ElementsAmount(int[] Array, int Target)
        {
            int temp = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                temp += Array[i];
            }
            return (temp == Target) ? true : false;
        }

        /// <summary>
        /// Метод копирования массива.
        /// </summary>
        /// <param name="Original">Оригинальный массив</param>
        /// <returns></returns>
        public static int[] Copy(int[] Original)
        {
            int[] temp = new int[Original.Length];
            for (int k = 0; k < temp.Length; k++)
            {
                temp[k] = Original[k];
            }
            return temp;
        }

        /// <summary>
        /// Метод сортировки последовательностей.
        /// </summary>
        /// <param name="Sequence">Массив с последовательностью</param>
        /// <returns></returns>
        public static bool VariantsSort(int[] Sequence)
        {
            bool flag = true;
            for (int i = 0; i < Sequence.Length - 1; i++)
            {
                if (Sequence[i] >= Sequence[i + 1])
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// Получаем комбинации последовательностей.
        /// </summary>
        /// <param name="BaseCombinations">Результирующий список</param>
        /// <param name="Players">Начальный массив с игроками</param>
        /// <param name="PlayersBuffer">Буфер</param>
        /// <param name="order">Элемент в буфере</param>
        static void RecursionSequences(List<int[]> BaseSequences, int[] Players, ref int[] PlayersBuffer, int order)
        {
            if (order < PlayersBuffer.Length)
                for (int i = 0; i < Players.Length; i++)
                {
                    PlayersBuffer[order] = Players[i];
                    RecursionSequences(BaseSequences, Players, ref PlayersBuffer, order + 1);
                }

            else
            {
                if (ElementsAmount(PlayersBuffer, (Players.Length - 1)))
                {
                    BaseSequences.Add(Copy(PlayersBuffer));
                }

                for (int i = PlayersBuffer.Length - 1; i < PlayersBuffer.Length; i++)
                {
                    PlayersBuffer[i] = 0;
                }
            }
        }

        static void AllCombinations(List<int[]> FinishList, int[] Players, ref int[] PlayersBuffer, int order)
        {
            if (order < PlayersBuffer.Length)
                for (int i = 0; i < Players.Length; i++)
                {
                    if (order == 0)
                    {
                        PlayersBuffer[order] = Players[i];
                        AllCombinations(FinishList, Players, ref PlayersBuffer, order + 1);
                    }
                    else if (!PlayersBuffer.Contains(Players[i]))
                    {
                        PlayersBuffer[order] = Players[i];
                        AllCombinations(FinishList, Players, ref PlayersBuffer, order + 1);
                    }
                }
            else
            {
                if (!PlayersBuffer.Contains(0))
                {
                    if (!FinishList.Contains(PlayersBuffer))
                    {
                        int[] Temp = new int[PlayersBuffer.Length];
                        Array.Copy(PlayersBuffer, Temp, PlayersBuffer.Length);
                        FinishList.Add(Temp);
                    }
                }

                for (int i = PlayersBuffer.Length - 2; i < PlayersBuffer.Length; i++)
                {
                    PlayersBuffer[i] = 0;
                }
            }
        }

        public static void PlayersIntoTeams(int PlayersNumber, int ColumnsNumber)
        {
            //1) Получаем базовые последовательности.
            List<int[]> BaseSequences = new List<int[]>();

            int[] Buffer = new int[ColumnsNumber];
            int[] Players = new int[(PlayersNumber + 1)];

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i] = i;
            }

            RecursionSequences(BaseSequences, Players, ref Buffer, 0);

            //2) Сортируем базовае последовательности.
            List<int[]> BaseSequenceList = new List<int[]>();

            foreach (int[] item in BaseSequences)
            {
                if (VariantsSort(item))
                {
                    BaseSequenceList.Add(item);
                }
            }

            //Получаем последнюю последовательность.
            int[] LastSequence = BaseSequenceList[BaseSequenceList.Count - 1];

            //Оставшиеся последовательности.
            BaseSequenceList.RemoveAt(BaseSequenceList.Count - 1);


            int[] PlayersS = new int[PlayersNumber];
            int[] BufferS = new int[PlayersNumber];

            List<int[]> BaseNumbers = new List<int[]>();

            for (int i = 0; i < PlayersS.Length; i++)
            {
                PlayersS[i] = (i + 1);
            }

            //3) Получаем список всех возможных комбинаций игроков.
            AllCombinations(BaseNumbers, PlayersS, ref BufferS, 0);

            //4) Получаем список вариантов последовательностей, для перемешивания.
            int[] AllVariants = new int[ColumnsNumber];

            for (int k = 0; k < AllVariants.Length; k++)
            {
                AllVariants[k] = (k + 1);
            }

            List<int[]> Variants = new List<int[]>();
            BufferS = new int[ColumnsNumber];
            AllCombinations(Variants, AllVariants, ref BufferS, 0);


            //5) Получаем все варианты комбинаций элементов во всех последовательностях.
            //Список всех элементов.
            List<Element> AllElements = new List<Element>();

            //Добавляем заранее последнюю комбинацию.
            Element E1 = new Element(PlayersNumber, ColumnsNumber);
            E1.FillCells(BaseNumbers[0], LastSequence);

            if (!AllElements.Contains(E1))
            {
                AllElements.Add(E1);
            }

            List<Element> TempList = new List<Element>();

            foreach (int[] Sequence in BaseSequenceList)
            {
                foreach (int[] Number in BaseNumbers)
                {
                    Element E = new Element(PlayersNumber, ColumnsNumber);
                    E.FillCells(Number, Sequence);

                    if (!TempList.Contains(E))
                    {
                        TempList.Add(E);
                    }
                }
                AllElements.AddRange(TempList);
                TempList.Clear();
            }

            //6) Получаем массивы строк.
            List<string[]> FinalList = new List<string[]>();

            foreach (Element E in AllElements)
            {
                FinalList.Add(E.ToStringArray());
            }

            //7)Перемешиваем массивы строк и превращаем в строки для вывода в консоль.
            List<string> FinalCombinationsList = new List<string>();

            foreach (string[] SA in FinalList)
            {
                foreach (int[] Variant in Variants)
                {
                    int Number = 1;
                    string Temp = "";
                    for (int i = 0; i < Variant.Length; i++)
                    {
                        Temp += ($"T{Number}: {SA[(Variant[i] - 1)]} ");
                        Number++;
                    }
                    if (!FinalCombinationsList.Contains(Temp))
                    {
                        FinalCombinationsList.Add(Temp);
                    }
                }
            }

            //8) Выводим все в консоль.
            foreach (string s in FinalCombinationsList)
            {
                Console.WriteLine(s);
            }
        }

        static void Main(string[] args)
        {
            PlayersIntoTeams(4,4);

            Console.ReadLine();
        }
    }
}
