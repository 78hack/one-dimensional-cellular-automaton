using System;
using System.Linq;

namespace CellularAutomaton
{
    class Program
    {
        /// <summary>
        /// セル・オートマトンのサイズを保持するフィールド
        /// </summary>
        private static readonly int automatonSize = 600;
        /// <summary>
        /// 描画する世代数を保持するフィールド
        /// </summary>
        private static readonly int stepCount = 300;

        /// <summary>
        /// エントリポイント
        /// </summary>
        /// <param name="args">args</param>
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("please enter Wolfram Code: ");

            // ウルフラム・コードを入力
            if (!int.TryParse(Console.ReadLine(), out var ruleDec) || ruleDec < 0 || ruleDec > 255) return;

            var rule = GetRule(ruleDec);

            // 第1世代のセル・オートマトンを生成し初期状態を与える
            var firstGene = "0".PadLeft(automatonSize + 2, '0').ToCharArray();
            firstGene[firstGene.Count() / 2] = '1';

            var currentGene = string.Join(string.Empty, firstGene);

            for (var t = 0; t < stepCount; t++)
            {
                foreach (var cell in currentGene.ToCharArray())
                {
                    // セルの状態に応じて描画
                    var view = cell == '0' ? " " : "*";
                    Console.Write(view);
                }
                Console.Write("\r\n");

                currentGene = GetNextGene(rule, currentGene);
            }

            Console.ResetColor();
            Console.ReadLine();
        }

        /// <summary>
        /// ルールを取得する
        /// </summary>
        /// <param name="ruleDec">ウルフラム・コード</param>
        /// <returns>ルール</returns>
        private static string[,] GetRule(int ruleDec)
        {
            // ウルフラム・コードを2進数に変換し8桁0埋め
            var ruleBinArray = int.Parse(Convert.ToString(ruleDec, 2)).ToString("D8").ToCharArray();
            // 近傍の状態を表す10進数
            var n = 7;

            var rule = new string[8, 2];
            var count = 0;

            // ルールを生成
            foreach (var bin in ruleBinArray)
            {
                rule[count, 0] = int.Parse(Convert.ToString(n, 2)).ToString("D3");
                rule[count, 1] = bin.ToString();

                count++;
                n--;
            }

            return rule;
        }

        /// <summary>
        /// 次世代のセル・オートマトンを取得する
        /// </summary>
        /// <param name="rule">ルール</param>
        /// <param name="currentGene">現世代のセル・オートマトン</param>
        /// <returns>次世代のセル・オートマトン</returns>
        private static string GetNextGene(string[,] rule, string currentGene)
        {
            // 左端は常に0
            var nextGene = "0";

            // ルールに従って次世代のセル・オートマトンを生成
            for (var i = 1; i < automatonSize + 1; i++)
            {
                for (var j = 0; j < rule.GetLength(0); j++)
                {
                    if (rule[j, 0] == currentGene.Substring(i - 1, 1) + currentGene.Substring(i, 1) + currentGene.Substring(i + 1, 1))
                    {
                        nextGene += rule[j, 1];
                        break;
                    }
                }
            }

            // 右端は常に0
            return nextGene + "0";
        }
    }
}
