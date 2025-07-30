using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGameProj
{
    /// <summary>
    /// Класс, проверяющий победу какого - либо игрока
    /// </summary>
    public class WinCheck
    {
        /// <summary>
        /// Координата прямой, которую нужно сделать видимой
        /// </summary>
        public int WinCoordinate=0;
        /// <summary>
        /// Тип победной линии
        /// </summary>
        public string WinType = "";
        public WinCheck() 
        { 

        }
        /// <summary>
        /// Проверка победы по диагонали
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsDiagonalWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = true;
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i][i].Text.ToString() != symbol.ToString())
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                WinType = "IsDiagonalWin";
                WinCoordinate = -1;
            }
            return result;
        }
        /// <summary>
        /// Проверка победы по побочной диагонали
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsSecondDiagonalWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = true;
            for (int i = Buttons.Count - 1, j = 0; i >= 0; i--, j++)
            {
                if (Buttons[i][j].Text.ToString() != symbol.ToString())
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                WinType = "IsSecondDiagonalWin";
                WinCoordinate = -2;
            }
            return result;
        }
        /// <summary>
        /// Проверка победы по вертикали
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsVerticalWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = false;
            bool stopflag = false;
            for (int i = 0; i < Buttons.Count; i++)
            {
                for (int j = 0; j < Buttons.Count; j++)
                {
                    // Если в столбце хоть один символ не равен искомому, победы явно нет
                    if (Buttons[j][i].Text.ToString() != symbol.ToString())
                    {
                        stopflag = false;
                        break;
                    }
                    stopflag = true;
                }
                // Если после проверки всего столбца значение флага истинно, игрок победил
                if (stopflag == true)
                {
                    result = true;
                    WinCoordinate = i;
                    break;
                }
            }
            if (result)
            {
                WinType = "IsVerticalWin";
            }
            return result;
        }
        /// <summary>
        /// Проверка победы по горизонтале
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        private bool IsHorizontallWin(List<List<Button>> Buttons, char symbol)
        {
            bool result = false;
            bool stopflag = false;
            for (int i = 0; i < Buttons.Count; i++)
            {
                for (int j = 0; j < Buttons.Count; j++)
                {
                    // Если в строке хоть один символ не равен искомому, победы явно нет
                    if (Buttons[i][j].Text.ToString() != symbol.ToString())
                    {
                        stopflag = false;
                        break;
                    }
                    stopflag = true;
                }
                // Если после проверки всей строки значение флага истинно, игрок победил
                if (stopflag == true)
                {
                    result = true;
                    WinCoordinate = i;
                    break;
                }
            }
            if (result)
            {
                WinType = "IsHorizontallWin";
            }
            return result;
        }
        /// <summary>
        /// Проверка на победу
        /// </summary>
        /// <param name="Buttons">Матрица с кнопками</param>
        /// <param name="symbol">Символ, последовательность из которого нужно искать</param>
        /// <returns></returns>
        public bool IsWin(List<List<Button>> Buttons, char symbol)
        {
            if (IsDiagonalWin(Buttons, symbol) == true || IsVerticalWin(Buttons, symbol) == true ||
                IsHorizontallWin(Buttons, symbol) == true || IsSecondDiagonalWin(Buttons, symbol) == true)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
